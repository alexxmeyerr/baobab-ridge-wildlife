using System.Reflection;
using BaobabRidge.Services;

namespace BaobabRidge.Tests;

internal static partial class Program
{
    // Exercises the shared form against changing files, including all three refresh paths.
    private static void TestIntegratedWorkflow(string directory, string? screenshotPath)
    {
        string path = Path.Combine(directory, "integrated.txt");
        File.WriteAllLines(path, new[] {
            FileManager.Serialize(MakeAnimalWithScore("WR-0002", 30, 4)),
            "broken|record", "WR-0998|Name|Species|x|30|Serious|High-Dependency Ward",
            FileManager.Serialize(MakeAnimalWithScore("WR-0001", 90, 2))
        });
        using var form = new EditTestForm(path);
        form.Show();
        Application.DoEvents();
        T Get<T>(string name) where T : Control => (T)form.Controls.Find(name, true).Single();
        var grid = Get<DataGridView>("dgvAnimals");
        var id = Get<TextBox>("txtAnimalId");
        var name = Get<TextBox>("txtName");
        var species = Get<TextBox>("txtSpecies");
        var age = Get<TextBox>("txtAge");
        var score = Get<TextBox>("txtRecoveryScore");
        var report = Get<TextBox>("txtSummaryReport");
        var feedback = Get<Label>("lblFeedback");
        Check(grid.Rows.Count == 2 && (string)grid.Rows[0].Cells[0].Value == "WR-0002"
            && (string)grid.Rows[1].Cells[0].Value == "WR-0001", "Grid skips malformed rows and keeps file order");
        Check(Get<Label>("lblLoadInfo").Text.StartsWith("2 malformed"), "One load message counts skipped records");
        Check(grid.ReadOnly && !grid.AllowUserToAddRows && !grid.AllowUserToDeleteRows && !grid.MultiSelect
            && grid.SelectionMode == DataGridViewSelectionMode.FullRowSelect
            && grid.Columns.Cast<DataGridViewColumn>().All(c => c.SortMode == DataGridViewColumnSortMode.NotSortable),
            "Grid uses read-only whole rows and cannot sort away file order");
        Get<Button>("btnDelete").PerformClick();
        Check(feedback.Text.Contains("Select an animal") && form.Confirmations == 0,
            "Delete without a selection gives feedback without confirmation");
        id.Text = "WR-9999"; name.Text = "Unsaved"; species.Text = "Otter"; age.Text = "7"; score.Text = "62";
        Get<Button>("btnSearch").PerformClick();
        Check(id.Text == "WR-9999" && name.Text == "Unsaved" && species.Text == "Otter" && age.Text == "7"
            && score.Text == "62" && feedback.Text.Contains("not found"), "Failed search leaves every input unchanged");
        Get<Button>("btnAdd").PerformClick();
        Check(grid.Rows.Count == 3 && (string)grid.Rows[2].Cells[0].Value == "WR-9999", "Add immediately refreshes the grid");
        // Raise the grid's real CellClick event without relying on desktop coordinates.
        grid.CurrentCell = grid.Rows[0].Cells[0];
        typeof(DataGridView).GetMethod("OnCellClick", BindingFlags.Instance | BindingFlags.NonPublic)!
            .Invoke(grid, new object[] { new DataGridViewCellEventArgs(0, 0) });
        Check(id.ReadOnly && id.Text == "WR-0002" && score.Text == "30" && !Get<Button>("btnAdd").Enabled,
            "Grid click enters Edit Mode using the selected record");
        name.Text = "Changed"; score.Text = "85";
        Get<Button>("btnSaveChanges").PerformClick();
        Check((string)grid.Rows[0].Cells[1].Value == "Changed"
            && (string)grid.Rows[0].Cells[5].Value == "Release-Ready" && id.Text == "",
            "Update immediately refreshes name and classification and clears input");
        id.Text = "WR-9999"; Get<Button>("btnSearch").PerformClick();
        form.Answer = true; Get<Button>("btnDelete").PerformClick();
        Check(grid.Rows.Count == 2 && id.Text == "" && !File.ReadAllText(path).Contains("WR-9999"),
            "Delete immediately removes the row and clears input");
        var files = new FileManager(path);
        files.TryAppend(MakeAnimalWithScore("WR-0003", 10, 3));
        Get<Button>("btnSummaryReport").PerformClick();
        Check(report.ReadOnly && report.Text.Contains("Total animals:            3")
            && report.Text.Contains("  Critical:               1")
            && File.ReadAllText(files.SummaryPath) == report.Text + Environment.NewLine,
            "Persistent summary rereads the file rather than a stale grid and matches saved text");
        File.SetAttributes(files.SummaryPath, FileAttributes.ReadOnly);
        try
        {
            Get<Button>("btnSummaryReport").PerformClick();
            Check(feedback.Text.Contains("permissions"), "Form reports summary write failure without crashing");
        }
        finally { File.SetAttributes(files.SummaryPath, FileAttributes.Normal); }
        if (!string.IsNullOrWhiteSpace(screenshotPath))
        {
            Get<Button>("btnSummaryReport").PerformClick();
            using var bitmap = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bitmap, new Rectangle(0, 0, form.Width, form.Height));
            bitmap.Save(Path.ChangeExtension(screenshotPath, ".integrated.png"));
        }
        form.Close();
        File.WriteAllText(path, "");
        using var emptyForm = new MainForm(new FileManager(path));
        emptyForm.Show();
        ((Button)emptyForm.Controls.Find("btnSummaryReport", true).Single()).PerformClick();
        Check(emptyForm.Controls.Find("txtSummaryReport", true).Single().Text.Contains("Average age (years):      N/A"),
            "Empty file generates a visible report without a modal dialog or crash");
        emptyForm.Close();
        string starter = Path.Combine(AppContext.BaseDirectory, "animals.txt");
        List<Animal> starterAnimals = new FileManager(starter).TryLoadAll(out int starterSkipped);
        Check(starterSkipped == 0 && starterAnimals.Count >= 8
            && starterAnimals.Select(a => a.AnimalId).Distinct().Count() == starterAnimals.Count
            && starterAnimals.Select(a => a.Status).Distinct().Count() >= 4,
            "Build copies valid starter data beside the executable: eight unique records and four or more statuses");
        using var missingFolderForm = new MainForm(new FileManager(Path.Combine(directory, "absent", "animals.txt")));
        Check(missingFolderForm.Controls.Find("lblFeedback", true).Single().Text.Contains("Cannot load"),
            "Startup missing folder gives friendly feedback");
    }
}
