using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace BaobabRidge.Services
{
    public sealed class SummaryReport
    {
        public int TotalAnimals { get; private set; }
        public double AverageAge { get; private set; }
        public double AverageRecoveryScore { get; private set; }
        public int ReadyForRelease { get; private set; }
        public int NeedsMonitoring { get; private set; }
        public int CriticalCare { get; private set; }

        internal SummaryReport(IList<Animal> animals)
        {
            TotalAnimals = animals.Count;
            if (TotalAnimals == 0)
            {
                return;
            }

            AverageAge = animals.Average(animal => animal.Age);
            AverageRecoveryScore = animals.Average(animal => animal.RecoveryScore);

            foreach (Animal animal in animals)
            {
                if (animal.Status == "Release-Ready")
                {
                    ReadyForRelease++;
                }
                else if (animal.Status == "Critical")
                {
                    CriticalCare++;
                }
                else
                {
                    NeedsMonitoring++;
                }
            }
        }

        public string ToText()
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine("===== Animal Rehabilitation Summary =====");
            report.AppendLine();
            report.AppendLine("Total Animals: " + TotalAnimals.ToString(CultureInfo.InvariantCulture));
            report.AppendLine("Average Age: " + AverageAge.ToString("0.0", CultureInfo.InvariantCulture) + " Years");
            report.AppendLine("Average Recovery Score: " + AverageRecoveryScore.ToString("0.0", CultureInfo.InvariantCulture) + "%");
            report.AppendLine("Ready for Release: " + ReadyForRelease.ToString(CultureInfo.InvariantCulture));
            report.AppendLine("Needs Monitoring: " + NeedsMonitoring.ToString(CultureInfo.InvariantCulture));
            report.AppendLine("Critical Care: " + CriticalCare.ToString(CultureInfo.InvariantCulture));
            return report.ToString().TrimEnd();
        }
    }

    public static class SummaryReportService
    {
        public static SummaryReport Create(IEnumerable<Animal> animals)
        {
            if (animals == null)
            {
                throw new ArgumentNullException(nameof(animals));
            }

            return new SummaryReport(animals.ToList());
        }

        public static void Save(SummaryReport report, string path)
        {
            if (report == null)
            {
                throw new ArgumentNullException(nameof(report));
            }

            File.WriteAllText(path, report.ToText() + Environment.NewLine, new UTF8Encoding(false));
        }
    }
}
