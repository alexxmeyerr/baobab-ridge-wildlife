using BaobabRidge.Services;

namespace BaobabRidge.Tests;

internal static partial class Program
{
    // Verifies edits, deletion, record preservation, duplicate IDs, and file failures.
    private static void TestUpdateDeleteFiles(string directory)
    {
        string path = Path.Combine(directory, "edit-records.txt");
        var service = new UpdateDeleteService(path);
        Check(service.FindAnimal("WR-0001") == null && service.GetAllAnimals().Count == 0,
            "Missing file is empty for search and load");
        Check(!service.UpdateAnimal(MakeAnimal("WR-0001")) && !service.DeleteAnimal("WR-0001")
            && !File.Exists(path), "Missing update/delete does not create a file");
        var files = new FileManager(path);
        files.TryAppend(MakeAnimal("WR-0001", "First"));
        files.TryAppend(MakeAnimal("WR-0002", "Second"));
        File.AppendAllText(path, "bad|record\n\n");
        files.TryAppend(MakeAnimal("WR-0003", "Third"));
        string[] before = File.ReadAllLines(path);
        Check(service.FindAnimal(" wr-0002 ")!.Name == "Second", "Case-insensitive trimmed search uses Add's file");
        Check(service.GetAllAnimals().Select(a => a.AnimalId).SequenceEqual(new[] {"WR-0001", "WR-0002", "WR-0003"}),
            "Loading skips malformed lines and preserves record order");
        Check(AnimalValidator.TryCreate("WR-0002", "NÃ¡lÃ©di", "Serval", "4", "85", out var edited, out _), "Prepare valid update");
        Check(service.UpdateAnimal(edited), "Update existing record");
        string[] after = File.ReadAllLines(path);
        Check(after.Length == before.Length && after[0] == before[0] && after[2] == before[2]
            && after[3] == before[3] && after[4] == before[4], "Update preserves other lines and position");
        Check(after[1] == "WR-0002|NÃ¡lÃ©di|Serval|4|85|Release-Ready|Pre-Release Camp", "Update recalculates both derived fields and preserves Unicode");
        Check(service.DeleteAnimal(" wr-0002 "), "Delete existing ID");
        Check(File.ReadAllLines(path).SequenceEqual(before.Where((_, index) => index != 1)), "Delete preserves all other lines in order");
        Check(!service.DeleteAnimal("WR-0002") && !service.UpdateAnimal(edited), "Unknown update/delete returns false");
        byte[] saved = File.ReadAllBytes(path);
        using (var locked = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
        {
            Throws<IOException>(() => service.UpdateAnimal(MakeAnimal("WR-0001")), "Locked update is rejected");
            Throws<IOException>(() => service.DeleteAnimal("WR-0001"), "Locked delete is rejected");
        }
        Check(saved.SequenceEqual(File.ReadAllBytes(path)), "Lock failures preserve original file");
        File.SetAttributes(path, FileAttributes.ReadOnly);
        try
        {
            Throws<UnauthorizedAccessException>(() => service.UpdateAnimal(MakeAnimal("WR-0001")), "Read-only update is rejected");
            Throws<UnauthorizedAccessException>(() => service.DeleteAnimal("WR-0001"), "Read-only delete is rejected");
        }
        finally { File.SetAttributes(path, FileAttributes.Normal); }
        Check(saved.SequenceEqual(File.ReadAllBytes(path)) && Directory.GetFiles(directory, "*.tmp").Length == 0,
            "Failed replacement preserves data and cleans temporary files");
        File.AppendAllText(path, "wr-0001|malformed\n");
        saved = File.ReadAllBytes(path);
        Throws<InvalidDataException>(() => service.FindAnimal("WR-0001"), "Ambiguous search is rejected");
        Throws<InvalidDataException>(() => service.UpdateAnimal(MakeAnimal("WR-0001")), "Ambiguous update is rejected");
        Throws<InvalidDataException>(() => service.DeleteAnimal("WR-0001"), "Ambiguous delete is rejected");
        Check(saved.SequenceEqual(File.ReadAllBytes(path)), "Ambiguous IDs do not change data");
        File.WriteAllText(path, "WR-0001|broken\n");
        Throws<InvalidDataException>(() => service.FindAnimal("WR-0001"), "Malformed target reports error");
        Throws<InvalidDataException>(() => service.DeleteAnimal("WR-0001"), "Malformed target is not silently removed");
        File.WriteAllText(path, FileManager.Serialize(MakeAnimal("WR-0001")));
        Check(service.DeleteAnimal("WR-0001") && File.ReadAllText(path) == "", "Delete final record leaves an empty file");
        Check(service.FindAnimal("WR-0001") == null, "Search after deleting final record");
    }

    private sealed class EditTestForm : MainForm
    {
        public bool Answer;
        public int Confirmations;
        // Uses disposable records instead of the real application file.
        public EditTestForm(string path) : base(new FileManager(path)) { }
        // Supplies a test answer without opening the production confirmation dialog.
        protected override bool ConfirmDelete(string id) { Confirmations++; return Answer; }
    }

    // Runs real form handlers to verify edit mode, retry, cancellation, and events.
    private static void TestUpdateDeleteForm(string directory, string? screenshotPath)
    {
        string path = Path.Combine(directory, "edit-ui.txt");
        using var form = new EditTestForm(path);
        form.Show();
        Application.DoEvents();
        T Control<T>(string name) where T : Control => (T)form.Controls.Find(name, true).Single();
        var id = Control<TextBox>("txtAnimalId");
        var name = Control<TextBox>("txtName");
        var species = Control<TextBox>("txtSpecies");
        var age = Control<TextBox>("txtAge");
        var score = Control<TextBox>("txtRecoveryScore");
        var add = Control<Button>("btnAdd");
        var search = Control<Button>("btnSearch");
        var save = Control<Button>("btnSaveChanges");
        var delete = Control<Button>("btnDelete");
        var clear = Control<Button>("btnClear");
        var feedback = Control<Label>("lblFeedback");
        int changes = 0, additions = 0;
        form.AnimalsChanged += (_, _) => changes++;
        form.AnimalAdded += (_, _) => additions++;
        Check(add.Enabled && !save.Enabled && delete.Enabled && !id.ReadOnly, "Initial add mode");
        search.PerformClick();
        Check(feedback.Text.Contains("Enter an Animal ID") && id.Focused, "Search requires ID");
        id.Text = "WR-0001";
        search.PerformClick();
        Check(feedback.Text.Contains("not found") && changes == 0, "Search missing file has no mutation event");
        name.Text = "First"; species.Text = "Serval"; age.Text = "2"; score.Text = "15";
        add.PerformClick();
        Check(changes == 1 && additions == 1, "Add publishes both compatibility and general refresh events");
        id.Text = "wr-0001";
        search.PerformClick();
        Check(name.Text == "First" && score.Text == "15" && id.Text == "WR-0001", "Search loads shared fields");
        Check(id.ReadOnly && !add.Enabled && save.Enabled && delete.Enabled && !search.Enabled
            && form.AcceptButton == save, "Edit mode locks ID and Add, enables save and delete");
        if (!string.IsNullOrWhiteSpace(screenshotPath))
        {
            using var bitmap = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bitmap, new Rectangle(0, 0, form.Width, form.Height));
            bitmap.Save(Path.ChangeExtension(screenshotPath, ".edit.png"));
        }
        byte[] original = File.ReadAllBytes(path);
        add.PerformClick();
        Check(changes == 1 && original.SequenceEqual(File.ReadAllBytes(path)), "Disabled Add cannot duplicate a record in edit mode");
        score.Text = "101";
        save.PerformClick();
        Check(feedback.Text.Contains("Recovery Score") && score.Focused && changes == 1
            && original.SequenceEqual(File.ReadAllBytes(path)), "Update enforces shared numeric validation");
        score.Text = "85"; name.Text = "Updated";
        using (var locked = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
        {
            save.PerformClick();
            Check(feedback.Text.Contains("Cannot update") && name.Text == "Updated" && id.ReadOnly
                && changes == 1, "Update failure retains edit mode and values for retry");
        }
        save.PerformClick();
        Check(changes == 2 && additions == 1 && feedback.Text.Contains("Successfully updated"), "Update fires general refresh exactly once");
        Check(File.ReadAllText(path).Contains("|85|Release-Ready|Pre-Release Camp"), "UI update persists recalculated classification");
        Check(new[] { id, name, species, age, score }.All(x => x.Text == "") && !id.ReadOnly
            && add.Enabled && !save.Enabled && delete.Enabled, "Update clears form and exits edit mode");
        id.Text = "WR-0001"; search.PerformClick();
        name.Text = "Cancelled edit"; clear.PerformClick();
        Check(add.Enabled && id.Text == "" && changes == 2
            && !File.ReadAllText(path).Contains("Cancelled"), "Clear cancels editing without saving");
        id.Text = "WR-0001"; search.PerformClick();
        original = File.ReadAllBytes(path);
        form.Answer = false; delete.PerformClick();
        Check(form.Confirmations == 1 && changes == 2 && id.ReadOnly
            && original.SequenceEqual(File.ReadAllBytes(path)), "No in delete confirmation preserves data and edit mode");
        form.Answer = true;
        using (var locked = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
        {
            delete.PerformClick();
            Check(feedback.Text.Contains("Cannot delete") && id.ReadOnly && changes == 2,
                "Delete error retains edit mode without a refresh");
        }
        delete.PerformClick();
        Check(form.Confirmations == 3 && changes == 3 && additions == 1 && File.ReadAllText(path) == "",
            "Confirmed delete removes record and refreshes exactly once");
        Check(feedback.Text.Contains("Successfully deleted") && id.Text == "" && add.Enabled
            && !save.Enabled && delete.Enabled && !id.ReadOnly, "Delete clears form and exits edit mode");
        form.Close();
    }
}
