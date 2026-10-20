using System.Globalization;
using System.Text;
using BaobabRidge.Services;

namespace BaobabRidge.Tests;

internal static partial class Program
{
    private static int assertions;

    // Runs dependency-free checks with disposable files and returns a failing exit code on error.
    [STAThread]
    private static int Main(string[] args)
    {
        string directory = Path.Combine(AppContext.BaseDirectory, "BaobabRidgeTests-" + Guid.NewGuid());
        Directory.CreateDirectory(directory);
        try
        {
            TestValidation();
            TestClassification();
            TestFiles(directory);
            TestSummaryReport(directory);
            TestUpdateDeleteFiles(directory);
            Application.EnableVisualStyles();
            TestForm(directory, args.FirstOrDefault());
            TestUpdateDeleteForm(directory, args.FirstOrDefault());
            TestIntegratedWorkflow(directory, args.FirstOrDefault());
            Console.WriteLine($"PASS: {assertions} assertions (validation, classification, files, and form workflow).");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            return 1;
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    // Rejects missing, oversized, delimiter, decimal, non-ASCII ID, and out-of-range input.
    private static void TestValidation()
    {
        Check(AnimalValidator.TryCreate(" wr-0007 ", " Naledi ", " Serval ", " 0 ", " 100 ", out var animal, out _), "Trimmed valid fields");
        Check(animal!.AnimalId == "WR-0007" && animal.Name == "Naledi" && animal.Species == "Serval", "Normalize text");
        foreach (var field in Enum.GetValues<AnimalField>())
        {
            string[] input = ["WR-0001", "Naledi", "Serval", "3", "62"];
            input[(int)field] = " ";
            Check(!AnimalValidator.TryCreate(input[0], input[1], input[2], input[3], input[4], out _, out var error)
                && error!.Field == field, "Required " + field);
        }
        foreach (string id in new[] { "WR-123", "WR-12345", "WR-12A4", "AB-0001", "WR-１２３４", "WR-0001\nX" })
            Check(!AnimalValidator.TryCreate(id, "A", "B", "0", "0", out _, out _), "Reject ID " + id);
        foreach (string text in new[] { new string('a', 31), "a|b", "a\nb", "a\rb" })
        {
            Check(!AnimalValidator.TryCreate("WR-0001", text, "B", "1", "1", out _, out _), "Reject name");
            Check(!AnimalValidator.TryCreate("WR-0001", "A", text, "1", "1", out _, out _), "Reject species");
        }
        foreach (string number in new[] { "-1", "101", "1.0", "1,0", "cat", "999999999999999999", "+5" })
        {
            Check(!AnimalValidator.TryCreate("WR-0001", "A", "B", number, "1", out _, out var error)
                && error!.Field == AnimalField.Age, "Reject age " + number);
            Check(!AnimalValidator.TryCreate("WR-0001", "A", "B", "1", number, out _, out error)
                && error!.Field == AnimalField.RecoveryScore, "Reject score " + number);
        }
        Check(AnimalValidator.TryCreate("WR-0000", new string('n', 30), new string('s', 30), "100", "0", out _, out _), "Upper text and age boundaries");
    }

    // Exercises both ends of every classification band and the two invalid ranges.
    private static void TestClassification()
    {
        (int Score, string Status, string Housing)[] cases =
        [
            (0, "Critical", "Intensive Care Unit"), (19, "Critical", "Intensive Care Unit"),
            (20, "Serious", "High-Dependency Ward"), (39, "Serious", "High-Dependency Ward"),
            (40, "Stable", "Recovery Ward"), (59, "Stable", "Recovery Ward"),
            (60, "Recovering", "Outdoor Enclosure"), (79, "Recovering", "Outdoor Enclosure"),
            (80, "Release-Ready", "Pre-Release Camp"), (100, "Release-Ready", "Pre-Release Camp")
        ];
        foreach (var c in cases)
        {
            AnimalClassifier.Classify(c.Score, out string status, out string housingUnit);
            Check(status == c.Status && housingUnit == c.Housing, "Classify " + c.Score);
        }
        Throws<ArgumentOutOfRangeException>(() => AnimalClassifier.Classify(-1, out _, out _), "Reject negative classification");
        Throws<ArgumentOutOfRangeException>(() => AnimalClassifier.Classify(101, out _, out _), "Reject classification above 100");
    }

    // Verifies exact output, restart-safe uniqueness, append preservation, and I/O failures.
    private static void TestFiles(string directory)
    {
        string path = Path.Combine(directory, "animals.txt");
        var files = new FileManager(path);
        var animal = MakeAnimal("wr-0007");
        Check(files.TryAppend(animal), "Create missing file and append");
        string expected = "WR-0007|Naledi|Serval|3|62|Recovering|Outdoor Enclosure" + Environment.NewLine;
        Check(File.ReadAllText(path) == expected, "Seven-field record order and exact spelling");
        byte[] before = File.ReadAllBytes(path);
        Check(!new FileManager(path).TryAppend(animal), "Duplicate checked after restart");
        Check(before.SequenceEqual(File.ReadAllBytes(path)), "Duplicate leaves file unchanged");
        File.WriteAllText(path, "wr-0007|malformed", new UTF8Encoding(false));
        Check(!files.TryAppend(animal), "Reserve lowercase ID in malformed line");
        foreach (string ending in new[] { "", "\n", "\r\n", "\r" })
        {
            string original = "WR-0001|Thabo|Penguin|4|85|Release-Ready|Pre-Release Camp" + ending;
            File.WriteAllText(path, original, new UTF8Encoding(false));
            before = File.ReadAllBytes(path);
            Check(files.TryAppend(animal), "Append with line ending " + ending.Length);
            Check(File.ReadAllBytes(path).Take(before.Length).SequenceEqual(before), "Preserve every existing byte");
            Check(File.ReadAllLines(path).Length == 2, "Append separate record without blank line");
        }
        File.WriteAllBytes(path, Encoding.UTF8.GetPreamble());
        Check(files.TryAppend(animal), "Append to BOM-only UTF-8 file");
        Check(File.ReadAllLines(path).Length == 1, "No empty record after BOM");
        File.WriteAllText(path, "", new UTF8Encoding(false));
        using (var locked = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            Throws<IOException>(() => files.TryAppend(animal), "File in use");
        File.SetAttributes(path, FileAttributes.ReadOnly);
        try { Throws<UnauthorizedAccessException>(() => files.TryAppend(animal), "Read-only file"); }
        finally { File.SetAttributes(path, FileAttributes.Normal); }
        Throws<DirectoryNotFoundException>(() => new FileManager(Path.Combine(directory, "missing", "animals.txt")).TryAppend(animal), "Missing directory");
        var previousCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
            Check(FileManager.Serialize(animal) + Environment.NewLine == expected, "Culture-independent serialization");
            Check(files.TryAppend(MakeAnimal("WR-0008", "Nálédi")), "Append Unicode name");
            Check(File.ReadAllText(path, Encoding.UTF8).Contains("Nálédi"), "UTF-8 round trip");
        }
        finally { CultureInfo.CurrentCulture = previousCulture; }
    }

    // Runs actual button handlers to check focus, messages, clearing, retry, and the refresh event.
    private static void TestForm(string directory, string? screenshotPath)
    {
        string path = Path.Combine(directory, "ui-animals.txt");
        using var form = new MainForm(new FileManager(path));
        form.Show();
        Application.DoEvents();
        TextBox id = (TextBox)form.Controls.Find("txtAnimalId", true).Single();
        TextBox name = (TextBox)form.Controls.Find("txtName", true).Single();
        TextBox species = (TextBox)form.Controls.Find("txtSpecies", true).Single();
        TextBox age = (TextBox)form.Controls.Find("txtAge", true).Single();
        TextBox score = (TextBox)form.Controls.Find("txtRecoveryScore", true).Single();
        Button add = (Button)form.Controls.Find("btnAdd", true).Single();
        Button clear = (Button)form.Controls.Find("btnClear", true).Single();
        Label feedback = (Label)form.Controls.Find("lblFeedback", true).Single();
        int refreshed = 0;
        form.AnimalAdded += (_, _) => refreshed++;
        add.PerformClick();
        Check(id.Focused && feedback.Text.Contains("Animal ID"), "Empty ID message and focus");
        Check(File.Exists(path) && File.ReadAllText(path) == "" && refreshed == 0, "Startup creates empty file; invalid input does not write or refresh");
        id.Text = " wr-0007 "; name.Text = " Naledi "; species.Text = " Serval "; age.Text = "3"; score.Text = "62.5";
        add.PerformClick();
        Check(score.Focused && feedback.Text.Contains("Recovery Score"), "Score error and focus");
        score.Text = "62";
        add.PerformClick();
        Check(feedback.Text.Contains("Successfully added WR-0007"), "Success message");
        Check(new[] { id, name, species, age, score }.All(x => x.Text == "") && id.Focused, "Clear all inputs and focus ID on success");
        Check(refreshed == 1 && File.ReadAllLines(path).Length == 1, "Save once and refresh once");
        Check(form.Controls.Find("lblStatusValue", true).Single().Text == "Recovering", "Display status");
        Check(form.Controls.Find("lblHousingValue", true).Single().Text == "Outdoor Enclosure", "Display housing unit");
        if (!string.IsNullOrWhiteSpace(screenshotPath))
        {
            using var bitmap = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bitmap, new Rectangle(0, 0, form.Width, form.Height));
            bitmap.Save(screenshotPath, System.Drawing.Imaging.ImageFormat.Png);
        }
        id.Text = "wr-0007"; name.Text = "Second"; species.Text = "Serval"; age.Text = "3"; score.Text = "80";
        add.PerformClick();
        Check(id.Focused && feedback.Text.Contains("already exists") && refreshed == 1, "Duplicate ID shows error without refresh");
        id.Text = "WR-0008";
        using (var locked = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
        {
            add.PerformClick();
            Check(feedback.Text.Contains("Cannot save") && id.Text == "WR-0008", "Locked file retains inputs and shows friendly error");
        }
        File.SetAttributes(path, FileAttributes.ReadOnly);
        try
        {
            add.PerformClick();
            Check(feedback.Text.Contains("read-only") && id.Text == "WR-0008", "Read-only file handled by form");
        }
        finally { File.SetAttributes(path, FileAttributes.Normal); }
        add.PerformClick();
        Check(refreshed == 2 && File.ReadAllLines(path).Length == 2, "Retry succeeds after file error");
        name.Text = "Discard";
        clear.PerformClick();
        Check(name.Text == "" && form.Controls.Find("lblStatusValue", true).Single().Text == "-", "Clear resets form");
        form.Close();
    }

    // Creates a known valid animal through the production validator for test fixtures.
    private static Animal MakeAnimal(string id, string name = "Naledi")
    {
        if (!AnimalValidator.TryCreate(id, name, "Serval", "3", "62", out var animal, out _))
            throw new InvalidOperationException("Invalid test data.");
        return animal!;
    }

    // Counts an assertion and reports its purpose if it fails.
    private static void Check(bool condition, string description)
    {
        assertions++;
        if (!condition) throw new InvalidOperationException("FAIL: " + description);
    }

    // Requires a particular failure without allowing unrelated exceptions to pass a test.
    private static void Throws<T>(Action action, string description) where T : Exception
    {
        try { action(); }
        catch (T) { Check(true, description); return; }
        Check(false, description);
    }
}
