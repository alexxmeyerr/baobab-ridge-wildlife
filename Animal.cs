using BaobabRidge.Services;

namespace BaobabRidge
{

    public class Animal
    {
        public string AnimalId { get; }
        public string Name { get; }
        public string Species { get; }
        public int Age { get; }
        public int RecoveryScore { get; }
        public string Status { get; }
        public string HousingUnit { get; }

        // Creates a validated animal and calculates its two derived fields together.
        internal Animal(string animalId, string name, string species, int age, int recoveryScore)
        {
            AnimalId = animalId;
            Name = name;
            Species = species;
            Age = age;
            RecoveryScore = recoveryScore;
            string status;
            string housingUnit;
            AnimalClassifier.Classify(recoveryScore, out status, out housingUnit);
            Status = status;
            HousingUnit = housingUnit;
        }
    }
}
