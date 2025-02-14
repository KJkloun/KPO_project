using System;
using Xunit;
using ZooERP.Interfaces;
using ZooERP.Managers;
using ZooERP.Models;
using System.Linq;
using System.Collections.Generic;

namespace ZooERP.Tests
{
    public class ZooAdvancedTests
    {
        // Заглушка для ветклиники
        private class FakeVetClinic : IVeterinaryClinic
        {
            private readonly bool _isHealthy;
            public FakeVetClinic(bool isHealthy) => _isHealthy = isHealthy;

            public bool CheckHealth(Animal animal)
            {
                return _isHealthy;
            }
        }

        [Fact]
        public void GetInteractiveAnimals_NoAnimals_ReturnsEmptyList()
        {
            // Arrange
            var vetClinic = new FakeVetClinic(true);
            var zoo = new Zoo(vetClinic);

            // Act
            var result = zoo.GetInteractiveAnimals();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetInteractiveAnimals_OnlyPredators_ReturnsEmptyList()
        {
            // Arrange
            var vetClinic = new FakeVetClinic(true);
            var zoo = new Zoo(vetClinic);

            // Добавляем только хищников
            zoo.AddAnimal(new Tiger("TigerOne", 5));
            zoo.AddAnimal(new Wolf("WolfOne", 4));

            // Act
            var result = zoo.GetInteractiveAnimals();

            // Assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(5, false)]
        [InlineData(6, true)]
        [InlineData(0, false)]
        [InlineData(10, true)]
        public void Herbo_IsInteractive_BasedOnKindness(int kindness, bool expected)
        {
            // Arrange
            var rabbit = new Rabbit("TestRabbit", 1, kindness);

            // Act
            bool actual = rabbit.IsInteractive;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddAnimal_MultipleAnimals_InventoryNumbersAreSequential()
        {
            // Arrange
            var vetClinic = new FakeVetClinic(true);
            var zoo = new Zoo(vetClinic);

            var monkey = new Monkey("MonkeyOne", 3, 6);
            var rabbit = new Rabbit("RabbitOne", 2, 7);
            var tiger = new Tiger("TigerOne", 5);

            // Act
            zoo.AddAnimal(monkey);  // инв. номер 1
            zoo.AddAnimal(rabbit);  // инв. номер 2
            zoo.AddAnimal(tiger);   // инв. номер 3

            // Assert
            Assert.Equal(3, zoo.Animals.Count);
            Assert.Equal(1, zoo.Animals[0].Number);
            Assert.Equal(2, zoo.Animals[1].Number);
            Assert.Equal(3, zoo.Animals[2].Number);
        }

        [Fact]
        public void AddThing_MultipleThings_InventoryNumbersContinueAfterAnimals()
        {
            // Arrange
            var vetClinic = new FakeVetClinic(true);
            var zoo = new Zoo(vetClinic);

            // Сначала добавим одно животное
            zoo.AddAnimal(new Rabbit("RabbitOne", 2, 7)); // Получит инв. номер 1

            // Теперь добавим несколько вещей
            var table = new Table("TableOne");      // Должен получить инв. номер 2
            var computer = new Computer("CompOne"); // Должен получить инв. номер 3

            // Act
            zoo.AddThing(table);
            zoo.AddThing(computer);

            // Assert
            Assert.Single(zoo.Animals);
            Assert.Equal(1, zoo.Animals[0].Number);

            Assert.Equal(2, table.Number);
            Assert.Equal(3, computer.Number);
        }

        [Fact]
        public void TotalFoodConsumption_NoAnimals_ReturnsZero()
        {
            // Arrange
            var vetClinic = new FakeVetClinic(true);
            var zoo = new Zoo(vetClinic);

            // Act
            int totalFood = zoo.TotalFoodConsumption();

            // Assert
            Assert.Equal(0, totalFood);
        }

        [Fact]
        public void TotalFoodConsumption_NegativeFood_StillAddsUp()
        {
            // Arrange
            var vetClinic = new FakeVetClinic(true);
            var zoo = new Zoo(vetClinic);

            // Допустим, по ошибке ввели отрицательное значение
            zoo.AddAnimal(new Rabbit("StrangeRabbit", -2, 8));
            zoo.AddAnimal(new Tiger("TigerOne", 5));

            // Act
            int totalFood = zoo.TotalFoodConsumption();

            // Assert
            // Логика класса не запрещает отрицательное значение,
            // поэтому суммарно будет 3 ( -2 + 5 = 3 )
            Assert.Equal(3, totalFood);
        }

        [Fact]
        public void AddAnimal_SameNameDifferentAnimals_AllAdded()
        {
            // Arrange
            var vetClinic = new FakeVetClinic(true);
            var zoo = new Zoo(vetClinic);

            // Два разных животных, но с одинаковым именем
            var rabbit = new Rabbit("Fluffy", 2, 8);
            var tiger = new Tiger("Fluffy", 5);

            // Act
            zoo.AddAnimal(rabbit);
            zoo.AddAnimal(tiger);

            // Assert
            Assert.Equal(2, zoo.Animals.Count);
            Assert.Equal("Fluffy", zoo.Animals[0].Name);
            Assert.Equal("Fluffy", zoo.Animals[1].Name);
        }
    }
}
