using System;

namespace BaobabRidge.Services
{

    public enum AnimalField { AnimalId, Name, Species, Age, RecoveryScore }

    public class ValidationError
    {
        private AnimalField field;
        private string message;

        // Stores which input failed and the explanation to show on the form.
        public ValidationError(AnimalField field, string message)
        {
            this.field = field;
            this.message = message;
        }

        public AnimalField Field { get => field; }
        public string Message { get => message; }
    }

    public static class AnimalValidator
    {
        // Checks each input in order and creates an animal only when every check passes.
        // Update can reuse this method with the original ID. Add checks uniqueness in FileManager.
        public static bool TryCreate(string animalId, string name, string species,
            string age, string recoveryScore, out Animal animal, out ValidationError error)
        {
            animal = null;
            error = null;
            string id = CleanText(animalId).ToUpperInvariant();
            string cleanName = CleanText(name);
            string cleanSpecies = CleanText(species);
            int parsedAge;
            int parsedScore;

            if (!IsValidId(id))
            {
                error = new ValidationError(AnimalField.AnimalId,
                    "Animal ID is required and must be WR- followed by exactly four digits (for example, WR-0007).");
                return false;
            }
            if (!IsValidText(cleanName))
            {
                error = new ValidationError(AnimalField.Name,
                    "Name is required, must be 1 to 30 characters, and cannot contain | or line breaks.");
                return false;
            }
            if (!IsValidText(cleanSpecies))
            {
                error = new ValidationError(AnimalField.Species,
                    "Species is required, must be 1 to 30 characters, and cannot contain | or line breaks.");
                return false;
            }
            if (!TryParseWholeNumber(age, out parsedAge))
            {
                error = new ValidationError(AnimalField.Age, "Age must be a whole number between 0 and 100.");
                return false;
            }
            if (!TryParseWholeNumber(recoveryScore, out parsedScore))
            {
                error = new ValidationError(AnimalField.RecoveryScore,
                    "Recovery Score must be a whole number between 0 and 100.");
                return false;
            }

            animal = new Animal(id, cleanName, cleanSpecies, parsedAge, parsedScore);
            return true;
        }

        // Treats null as empty text and removes spaces before and after the input.
        private static string CleanText(string text)
        {
            if (text == null)
            {
                return "";
            }
            return text.Trim();
        }

        // Requires the WR- prefix and exactly four ordinary digits.
        private static bool IsValidId(string id)
        {
            if (id.Length != 7 || !id.StartsWith("WR-"))
            {
                return false;
            }
            return ContainsOnlyDigits(id.Substring(3));
        }

        // Checks the length and rejects characters that would split a saved record.
        private static bool IsValidText(string text)
        {
            if (text.Length < 1 || text.Length > 30)
            {
                return false;
            }
            if (text.Contains("|") || text.Contains("\r") || text.Contains("\n"))
            {
                return false;
            }
            return true;
        }

        // Uses a loop to reject empty text, letters, signs, and decimal separators.
        private static bool ContainsOnlyDigits(string text)
        {
            if (text.Length == 0)
            {
                return false;
            }
            foreach (char character in text)
            {
                if (character < '0' || character > '9')
                {
                    return false;
                }
            }
            return true;
        }

        // Converts whole-number input safely and checks that it is between 0 and 100.
        private static bool TryParseWholeNumber(string text, out int value)
        {
            value = 0;
            text = CleanText(text);
            if (!ContainsOnlyDigits(text))
            {
                return false;
            }
            if (!int.TryParse(text, out value))
            {
                return false;
            }
            return value >= 0 && value <= 100;
        }
    }
}
