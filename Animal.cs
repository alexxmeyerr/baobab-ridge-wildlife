using BaobabRidge.Services;

namespace BaobabRidge
{

    public class Animal
    {
        private string animalId, name, species, status, housingUnit;
        private int age, recoveryScore;

        // Creates a validated animal and calculates its two derived fields together.
        internal Animal(string animalId, string name, string species, int age, int recoveryScore)
        {
            this.animalId = animalId;
            this.name = name;
            this.species = species;
            this.age = age;
            this.recoveryScore = recoveryScore;
            string status;
            string housingUnit;
            AnimalClassifier.Classify(recoveryScore, out status, out housingUnit);
            this.status = status;
            this.housingUnit = housingUnit;
        }

        // Properties expose the fields. Update creates a new validated animal.
        public string AnimalId { get => animalId; }
        public string Name { get => name; }
        public string Species { get => species; }
        public int Age { get => age; }
        public int RecoveryScore { get => recoveryScore; }
        public string Status { get => status; }
        public string HousingUnit { get => housingUnit; }
    }
}
