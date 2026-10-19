using System;

namespace BaobabRidge.Services
{

    public static class AnimalClassifier
    {
        // Calculates both outputs in one method so Add and Update use the same rules.
        public static void Classify(int recoveryScore, out string status, out string housingUnit)
        {
            if (recoveryScore < 0 || recoveryScore > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(recoveryScore));
            }

            if (recoveryScore <= 19)
            {
                status = "Critical";
                housingUnit = "Intensive Care Unit";
            }
            else if (recoveryScore <= 39)
            {
                status = "Serious";
                housingUnit = "High-Dependency Ward";
            }
            else if (recoveryScore <= 59)
            {
                status = "Stable";
                housingUnit = "Recovery Ward";
            }
            else if (recoveryScore <= 79)
            {
                status = "Recovering";
                housingUnit = "Outdoor Enclosure";
            }
            else
            {
                status = "Release-Ready";
                housingUnit = "Pre-Release Camp";
            }
        }
    }
}
