using System;
using Xunit;
using ZooERP.Interfaces;
using ZooERP.Managers;
using ZooERP.Models;

namespace ZooERP.Tests
{
    // Фейковая ветклиника для тестов (исключаем ввод с консоли)
    public class FakeVetClinic : IVeterinaryClinic
    {
        private readonly bool _isHealthy;
        public FakeVetClinic(bool isHealthy) => _isHealthy = isHealthy;

        public bool CheckHealth(Animal animal)
        {
            // Возвращаем заранее заданный результат
            return _isHealthy;
        }
    }

    public class ZooTests
    {
        [Fact]
        public void AddAnimal_HealthyAnimal_AnimalAdded()
        {
            // Arrange
            var vetClinic = new FakeVetClinic(true);
            var zoo = new Zoo(vetClinic);
            var rabbit = new Rabbit("Bunny", 2, 8);

            // Act
            zoo.AddAnimal(rabbit);

            // Assert
            Assert.Single(zoo.Animals);
            Assert.Equal("Bunny", zoo.Animals[0].Name);
            Assert.Equal(1, zoo.Animals[0].Number);
        }

        [Fact]
        public void AddAnimal_UnhealthyAnimal_AnimalNotAdded()
        {
            // Arrange
            var vetClinic = new FakeVetClinic(false);
            var zoo = new Zoo(vetClinic);
            var tiger = new Tiger("Tony", 5);

            // Act
            zoo.AddAnimal(tiger);

            // Assert
            Assert.Empty(zoo.Animals);
        }

        [Fact]
        public void TotalFoodConsumption_MultipleAnimals_ReturnsCorrectSum()
        {
            // Arrange
            var vetClinic = new FakeVetClinic(true);
            var zoo = new Zoo(vetClinic);

            var rabbit = new Rabbit("Bunny", 2, 8);
            var tiger = new Tiger("Tony", 5);

            zoo.AddAnimal(rabbit);
            zoo.AddAnimal(tiger);

            // Act
            int totalFood = zoo.TotalFoodConsumption();

            // Assert
            Assert.Equal(7, totalFood); // 2 + 5 = 7
        }

        [Fact]
        public void GetInteractiveAnimals_MixedAnimals_ReturnsOnlyKindHerbos()
        {
            // Arrange
            var vetClinic = new FakeVetClinic(true);
            var zoo = new Zoo(vetClinic);

            var rabbitKind = new Rabbit("KindRabbit", 1, 8);    // доброта 8
            var rabbitNotKind = new Rabbit("GrumpyRabbit", 1, 3); // доброта 3
            var tiger = new Tiger("Tony", 5); // хищник

            zoo.AddAnimal(rabbitKind);
            zoo.AddAnimal(rabbitNotKind);
            zoo.AddAnimal(tiger);

            // Act
            var interactiveList = zoo.GetInteractiveAnimals();

            // Assert
            Assert.Single(interactiveList); // Только KindRabbit
            Assert.Equal("KindRabbit", interactiveList[0].Name);
        }

        [Fact]
        public void AddThing_ThingAddedWithInventoryNumber()
        {
            // Arrange
            var vetClinic = new FakeVetClinic(true);
            var zoo = new Zoo(vetClinic);
            var table = new Table("Wooden Table");

            // Act
            zoo.AddThing(table);

            // Assert
            Assert.Single(zoo.Things);
            Assert.Equal("Wooden Table", zoo.Things[0].Name);
            Assert.Equal(1, zoo.Things[0].Number);
        }
    }
}
