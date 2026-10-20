using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace BaobabRidge.Services
{
    public sealed class SummaryReport
    {
        public int TotalAnimals { get; private set; }
        public double AverageAge { get; private set; }
        public double AverageRecoveryScore { get; private set; }
        public int Critical { get; private set; }
        public int Serious { get; private set; }
        public int Stable { get; private set; }
        public int Recovering { get; private set; }
        public int ReleaseReady { get; private set; }
        public DateTime GeneratedAt { get; private set; }

        // Adds totals and counts the status already calculated by AnimalClassifier.
        internal SummaryReport(IEnumerable<Animal> animals)
        {
            GeneratedAt = DateTime.Now;
            int totalAge = 0;
            int totalScore = 0;
            foreach (Animal animal in animals)
            {
                TotalAnimals++;
                totalAge += animal.Age;
                totalScore += animal.RecoveryScore;
                if (animal.Status == "Critical") Critical++;
                else if (animal.Status == "Serious") Serious++;
                else if (animal.Status == "Stable") Stable++;
                else if (animal.Status == "Recovering") Recovering++;
                else if (animal.Status == "Release-Ready") ReleaseReady++;
            }
            if (TotalAnimals > 0)
            {
                AverageAge = (double)totalAge / TotalAnimals;
                AverageRecoveryScore = (double)totalScore / TotalAnimals;
            }
        }

        // Uses the required layout, two decimal places, and N/A for empty data.
        public string ToText()
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine("BAOBAB RIDGE WILDLIFE REHABILITATION CENTRE");
            report.AppendLine("ANIMAL SUMMARY REPORT");
            report.AppendLine("Generated: " + GeneratedAt.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));
            report.AppendLine("----------------------------------------");
            report.AppendLine("Total animals:            " + TotalAnimals.ToString(CultureInfo.InvariantCulture));
            report.AppendLine("Average age (years):      " + (TotalAnimals == 0 ? "N/A" : AverageAge.ToString("F2", CultureInfo.InvariantCulture)));
            report.AppendLine("Average recovery score:   " + (TotalAnimals == 0 ? "N/A" : AverageRecoveryScore.ToString("F2", CultureInfo.InvariantCulture)));
            report.AppendLine("----------------------------------------");
            report.AppendLine("Animals per status:");
            report.AppendLine("  Critical:               " + Critical.ToString(CultureInfo.InvariantCulture));
            report.AppendLine("  Serious:                " + Serious.ToString(CultureInfo.InvariantCulture));
            report.AppendLine("  Stable:                 " + Stable.ToString(CultureInfo.InvariantCulture));
            report.AppendLine("  Recovering:             " + Recovering.ToString(CultureInfo.InvariantCulture));
            report.Append("  Release-Ready:          " + ReleaseReady.ToString(CultureInfo.InvariantCulture));
            return report.ToString();
        }
    }

    public static class SummaryReportService
    {
        // Creates a report from the valid records most recently read from the file.
        public static SummaryReport Create(IEnumerable<Animal> animals)
        {
            if (animals == null) throw new ArgumentNullException(nameof(animals));
            return new SummaryReport(animals);
        }

        // Overwrites the previous UTF-8 report; the form handles file errors.
        public static void Save(SummaryReport report, string path)
        {
            if (report == null) throw new ArgumentNullException(nameof(report));
            using (StreamWriter writer = new StreamWriter(path, false, new UTF8Encoding(false)))
            {
                writer.WriteLine(report.ToText());
            }
        }
    }
}
