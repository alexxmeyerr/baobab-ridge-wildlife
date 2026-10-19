using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BaobabRidge.Services
{
    // Adapted from Member 3's service: preserve file order and use the shared Animal model.
    public class UpdateDeleteService
    {
        private string filePath;

        public UpdateDeleteService() : this(new FileManager().AnimalsPath) { }

        public UpdateDeleteService(string animalsPath)
        {
            filePath = animalsPath;
        }

        private List<string> ReadLines()
        {
            List<string> lines = new List<string>();
            if (!File.Exists(filePath)) return lines;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null) lines.Add(line);
            }
            return lines;
        }

        // Refuse ambiguous IDs instead of changing the first of several matching rows.
        private int FindIndex(List<string> lines, string animalId)
        {
            int found = -1;
            for (int i = 0; i < lines.Count; i++)
            {
                string[] parts = lines[i].Split('|');
                if (parts[0].Trim().Equals(animalId.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    if (found != -1)
                        throw new InvalidDataException("This Animal ID occurs more than once. Correct the duplicate records before editing.");
                    found = i;
                }
            }
            return found;
        }

        private bool TryReadAnimal(string line, out Animal animal)
        {
            animal = null;
            string[] parts = line.Split('|');
            if (parts.Length != 7) return false;
            ValidationError error;
            return AnimalValidator.TryCreate(parts[0], parts[1], parts[2], parts[3], parts[4],
                out animal, out error);
        }

        public Animal FindAnimal(string animalId)
        {
            List<string> lines = ReadLines();
            int index = FindIndex(lines, animalId);
            if (index == -1) return null;
            Animal animal;
            if (!TryReadAnimal(lines[index], out animal))
                throw new InvalidDataException("This animal's saved record is malformed. Correct the record before editing.");
            return animal;
        }

        public List<Animal> GetAllAnimals()
        {
            List<Animal> animals = new List<Animal>();
            foreach (string line in ReadLines())
            {
                Animal animal;
                if (TryReadAnimal(line, out animal)) animals.Add(animal);
            }
            return animals;
        }

        public bool AnimalIdExists(string animalId)
        {
            return FindIndex(ReadLines(), animalId) != -1;
        }

        public bool UpdateAnimal(Animal updatedAnimal)
        {
            List<string> lines = ReadLines();
            int index = FindIndex(lines, updatedAnimal.AnimalId);
            if (index == -1) return false;
            Animal existing;
            if (!TryReadAnimal(lines[index], out existing))
                throw new InvalidDataException("This animal's saved record is malformed. Correct the record before editing.");
            lines[index] = FileManager.Serialize(updatedAnimal);
            WriteLines(lines);
            return true;
        }

        public bool DeleteAnimal(string animalId)
        {
            List<string> lines = ReadLines();
            int index = FindIndex(lines, animalId);
            if (index == -1) return false;
            Animal existing;
            if (!TryReadAnimal(lines[index], out existing))
                throw new InvalidDataException("This animal's saved record is malformed. Correct the record before deleting.");
            lines.RemoveAt(index);
            WriteLines(lines);
            return true;
        }

        private void WriteLines(List<string> lines)
        {
            // Finish writing a temporary file before replacing the original.
            // A failed write leaves the original records intact.
            string temporaryPath = filePath + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                using (FileStream fs = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write))
                using (StreamWriter sw = new StreamWriter(fs, new UTF8Encoding(false)))
                {
                    foreach (string line in lines) sw.WriteLine(line);
                }
                File.Replace(temporaryPath, filePath, null);
            }
            finally
            {
                if (File.Exists(temporaryPath)) File.Delete(temporaryPath);
            }
        }
    }
}
