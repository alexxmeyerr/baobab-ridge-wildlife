using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BaobabRidge.Services
{
    public class UpdateDeleteService
    {
        private readonly string filePath;

        public UpdateDeleteService()
        {
            string dataFolder = Path.Combine(AppContext.BaseDirectory, "Data");

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            filePath = Path.Combine(dataFolder, "animals.txt");
        }

       
        public AnimalRecord? FindAnimal(string animalId)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split('|');

                
                if (parts.Length != 7)
                    continue;

                if (parts[0].Trim().Equals(animalId.Trim(),
                    StringComparison.OrdinalIgnoreCase))
                {
                    return new AnimalRecord(
                        parts[0].Trim(),
                        parts[1].Trim(),
                        parts[2].Trim(),
                        parts[3].Trim(),
                        parts[4].Trim(),
                        parts[5].Trim(),
                        parts[6].Trim()
                    );
                }
            }

            return null;
        }

      
        public List<AnimalRecord> GetAllAnimals()
        {
            List<AnimalRecord> animals = new List<AnimalRecord>();

            if (!File.Exists(filePath))
            {
                return animals;
            }

            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split('|');

                if (parts.Length != 7)
                    continue;

                animals.Add(new AnimalRecord(
                    parts[0].Trim(),
                    parts[1].Trim(),
                    parts[2].Trim(),
                    parts[3].Trim(),
                    parts[4].Trim(),
                    parts[5].Trim(),
                    parts[6].Trim()
                ));
            }

            return animals;
        }

      
        public bool UpdateAnimal(AnimalRecord updatedAnimal)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }

            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                string[] parts = lines[i].Split('|');

                if (parts.Length != 7)
                    continue;

                if (parts[0].Trim().Equals(
                    updatedAnimal.AnimalId,
                    StringComparison.OrdinalIgnoreCase))
                {
                    lines[i] = updatedAnimal.ToFileLine();

                    File.WriteAllLines(filePath, lines);

                    return true;
                }
            }

            return false;
        }

        
        public bool DeleteAnimal(string animalId)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }

            List<string> lines = File.ReadAllLines(filePath).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                string[] parts = lines[i].Split('|');

                if (parts.Length != 7)
                    continue;

                if (parts[0].Trim().Equals(
                    animalId.Trim(),
                    StringComparison.OrdinalIgnoreCase))
                {
                    lines.RemoveAt(i);

                    File.WriteAllLines(filePath, lines);

                    return true;
                }
            }

            return false;
        }

       
        public bool AnimalIdExists(string animalId)
        {
            return FindAnimal(animalId) != null;
        }
    }

   
    public class AnimalRecord
    {
        public string AnimalId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Age { get; set; }
        public string RecoveryScore { get; set; }
        public string Status { get; set; }
        public string HousingUnit { get; set; }

        public AnimalRecord(
            string animalId,
            string name,
            string species,
            string age,
            string recoveryScore,
            string status,
            string housingUnit)
        {
            AnimalId = animalId;
            Name = name;
            Species = species;
            Age = age;
            RecoveryScore = recoveryScore;
            Status = status;
            HousingUnit = housingUnit;
        }

        public string ToFileLine()
        {
            return $"{AnimalId}|{Name}|{Species}|{Age}|{RecoveryScore}|{Status}|{HousingUnit}";
        }
    }
}