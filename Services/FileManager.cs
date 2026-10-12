using System.Globalization;
using System.Text;

namespace BaobabRidge.Services
{

    public class FileManager
    {
        public string AnimalsPath { get; private set; }

        // Uses the executable folder normally, or a separate path for testing.
        public FileManager(string animalsPath = null)
        {
            if (animalsPath == null)
            {
                AnimalsPath = Path.Combine(AppContext.BaseDirectory, "animals.txt");
            }
            else
            {
                AnimalsPath = animalsPath;
            }
        }

        // Reads the current file, rejects a duplicate ID, and appends one new record.
        // The form catches file exceptions and shows a friendly message.
        public bool TryAppend(Animal animal)
        {
            string existingText = "";
            if (File.Exists(AnimalsPath))
            {
                existingText = File.ReadAllText(AnimalsPath, Encoding.UTF8);
            }

            // StringReader reads the saved text one line at a time, including different line endings.
            using (StringReader reader = new StringReader(existingText))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] fields = line.Split('|');
                    string existingId = fields[0].Trim().ToUpperInvariant();
                    // Reserve the ID even if the rest of an existing record is malformed.
                    if (existingId == animal.AnimalId)
                    {
                        return false;
                    }
                }
            }

            // true means append: StreamWriter leaves the existing file contents untouched.
            using (StreamWriter writer = new StreamWriter(AnimalsPath, true, new UTF8Encoding(false)))
            {
                // Start a separate record if the old final line has no line ending.
                if (existingText.Length > 0 && !existingText.EndsWith("\n") && !existingText.EndsWith("\r"))
                {
                    writer.WriteLine();
                }
                writer.WriteLine(Serialize(animal));
            }
            return true;
        }

        // Joins the seven fields in the exact order required by animals.txt.
        public static string Serialize(Animal animal)
        {
            return animal.AnimalId + "|" + animal.Name + "|" + animal.Species + "|"
                + animal.Age.ToString(CultureInfo.InvariantCulture) + "|"
                + animal.RecoveryScore.ToString(CultureInfo.InvariantCulture) + "|"
                + animal.Status + "|" + animal.HousingUnit;
        }
    }
}
