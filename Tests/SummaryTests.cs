using BaobabRidge.Services;

namespace BaobabRidge.Tests;

internal static partial class Program
{
    private static void TestSummaryReport(string directory)
    {
        Animal release = MakeAnimalWithScore("WR-0001", 80, 2);
        Animal monitoring = MakeAnimalWithScore("WR-0002", 60, 4);
        Animal critical = MakeAnimalWithScore("WR-0003", 10, 7);

        SummaryReport report = SummaryReportService.Create(new[] { release, monitoring, critical });
        string expected = "===== Animal Rehabilitation Summary =====" + Environment.NewLine + Environment.NewLine
            + "Total Animals: 3" + Environment.NewLine
            + "Average Age: 4.3 Years" + Environment.NewLine
            + "Average Recovery Score: 50.0%" + Environment.NewLine
            + "Ready for Release: 1" + Environment.NewLine
            + "Needs Monitoring: 1" + Environment.NewLine
            + "Critical Care: 1";
        Check(report.ToText() == expected, "Summary calculates and formats all statistics");

        string path = Path.Combine(directory, "summary.txt");
        SummaryReportService.Save(report, path);
        Check(File.ReadAllText(path) == expected + Environment.NewLine, "Summary report saves as UTF-8 text");
    }

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
