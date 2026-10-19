using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace BaobabRidge.Services
{
    public class FileManager
    {
        private string path;

        // Keep the data beside the executable when the application is run.
        public FileManager()
        {
            path = Path.Combine(AppContext.BaseDirectory, "animals.txt");
        }

        // A separate path lets the tests use disposable files.
        public FileManager(string animalsPath)
        {
            if (animalsPath == null)
            {
                path = Path.Combine(AppContext.BaseDirectory, "animals.txt");
            }
            else
            {
                path = animalsPath;
            }
        }

        public string AnimalsPath { get => path; }

        // Read lines into a list, as in the lecturer's ReadWrite example.
        // A missing file has no records; the append operation will create it.
        private List<string> Read(out bool needsNewLine)
        {
            List<string> lines = new List<string>();
            needsNewLine = false;

            if (File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    string text;
                    while ((text = sr.ReadLine()) != null)
                    {
                        lines.Add(text);
                    }

                    // A hand-edited file may not end with Enter. Check its last byte
                    // so the next animal is written on a new line. UTF-8 uses 10 for
                    // line feed and 13 for carriage return. Ignore an empty/BOM-only file.
                    if (lines.Count > 0)
                    {
                        fs.Seek(-1, SeekOrigin.End);
                        int lastByte = fs.ReadByte();
                        needsNewLine = lastByte != 10 && lastByte != 13;
                    }
                }
            }
            return lines;
        }

        // Check the saved IDs and append only the new animal.
        // The form catches file errors and keeps the user's input for a retry.
        public bool TryAppend(Animal animal)
        {
            bool needsNewLine;
            List<string> lines = Read(out needsNewLine);

            foreach (string text in lines)
            {
                string[] fields = text.Split('|');
                string existingId = fields[0].Trim().ToUpperInvariant();
                // Reserve an ID even if the other fields in its line are malformed.
                if (existingId == animal.AnimalId)
                {
                    return false;
                }
            }

            // Append preserves old records. Create would overwrite them.
            // using closes both streams even if writing throws an exception.
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs, new UTF8Encoding(false)))
            {
                if (needsNewLine)
                {
                    sw.WriteLine();
                }
                string text = Serialize(animal);
                sw.WriteLine(text);
            }
            return true;
        }

        // Build one record by joining the seven fields with the required delimiter.
        public static string Serialize(Animal animal)
        {
            return animal.AnimalId + "|" + animal.Name + "|" + animal.Species + "|"
                + animal.Age.ToString(CultureInfo.InvariantCulture) + "|"
                + animal.RecoveryScore.ToString(CultureInfo.InvariantCulture) + "|"
                + animal.Status + "|" + animal.HousingUnit;
        }
        
    
public List<Animal> TryLoadAll(out int skipped)
{
    List<Animal> animals = new List<Animal>();
    skipped = 0;

    List<string> lines = Read(out _);

    foreach (string line in lines)
    {
        string[] fields = line.Split('|');

        
        if (fields.Length != 7)
        {
            skipped++;
            continue;
        }

        Animal animal;
        ValidationError error;

        if (!AnimalValidator.TryCreate(
            fields[0],
            fields[1],
            fields[2],
            fields[3],
            fields[4],
            out animal,
            out error))
        {
            skipped++;
            continue;
        }

        animals.Add(animal);
    }

    return animals;
}

    }
}
