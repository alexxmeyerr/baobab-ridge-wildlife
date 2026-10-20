using BaobabRidge.Services;
using System.Globalization;

namespace BaobabRidge.Tests;

internal static partial class Program
{
    // Checks every status, exact formatting, empty data, locale, overwrite, and file failures.
    private static void TestSummaryReport(string directory)
    {
        Animal[] animals = {
            MakeAnimalWithScore("WR-0001", 10, 2), MakeAnimalWithScore("WR-0002", 30, 4),
            MakeAnimalWithScore("WR-0003", 50, 7), MakeAnimalWithScore("WR-0004", 70, 8),
            MakeAnimalWithScore("WR-0005", 90, 10)
        };
        SummaryReport report = SummaryReportService.Create(animals);
        Check(report.TotalAnimals == 5 && report.AverageAge == 6.2 && report.AverageRecoveryScore == 50,
            "Summary totals and fractional averages");
        Check(report.Critical == 1 && report.Serious == 1 && report.Stable == 1
            && report.Recovering == 1 && report.ReleaseReady == 1, "All five status counts remain separate");
        string expected = "BAOBAB RIDGE WILDLIFE REHABILITATION CENTRE\nANIMAL SUMMARY REPORT\nGenerated: "
            + report.GeneratedAt.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)
            + "\n----------------------------------------\nTotal animals:            5"
            + "\nAverage age (years):      6.20\nAverage recovery score:   50.00"
            + "\n----------------------------------------\nAnimals per status:"
            + "\n  Critical:               1\n  Serious:                1\n  Stable:                 1"
            + "\n  Recovering:             1\n  Release-Ready:          1";
        expected = expected.Replace("\n", Environment.NewLine);
        Check(report.ToText() == expected, "Exact required summary layout and generation timestamp");
        CultureInfo previous = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
            Check(report.ToText() == expected, "Summary decimal points do not depend on computer locale");
        }
        finally { CultureInfo.CurrentCulture = previous; }
        string path = Path.Combine(directory, "summary.txt");
        File.WriteAllText(path, "Old report that must be overwritten");
        SummaryReportService.Save(report, path);
        Check(File.ReadAllText(path) == expected + Environment.NewLine, "Summary report saves as UTF-8 text");
        SummaryReport empty = SummaryReportService.Create(Array.Empty<Animal>());
        Check(empty.ToText().Contains("Average age (years):      N/A")
            && empty.ToText().Contains("Average recovery score:   N/A") && empty.TotalAnimals == 0
            && empty.Critical + empty.Serious + empty.Stable + empty.Recovering + empty.ReleaseReady == 0,
            "Empty report uses N/A and zero counts without division by zero");
        using (var locked = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            Throws<IOException>(() => SummaryReportService.Save(report, path), "Locked report cannot be overwritten");
        File.SetAttributes(path, FileAttributes.ReadOnly);
        try { Throws<UnauthorizedAccessException>(() => SummaryReportService.Save(report, path), "Read-only summary reports error"); }
        finally { File.SetAttributes(path, FileAttributes.Normal); }
        Throws<DirectoryNotFoundException>(() => SummaryReportService.Save(report,
            Path.Combine(directory, "absent", "summary.txt")), "Missing summary folder reports error");
    }

    // Creates validated fixtures through the same entry point used by the form.
    private static Animal MakeAnimalWithScore(string id, int score, int age)
    {
        if (!AnimalValidator.TryCreate(id, "Naledi", "Serval", age.ToString(), score.ToString(),
            out Animal animal, out _))
        {
            throw new InvalidOperationException("Invalid summary test data.");
        }

        return animal;
    }
}
