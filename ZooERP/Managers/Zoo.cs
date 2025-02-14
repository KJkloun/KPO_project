using System;
using System.Collections.Generic;
using ZooERP.Interfaces;
using ZooERP.Models;

namespace ZooERP.Managers
{
    public class Zoo
    {
        private readonly IVeterinaryClinic _vetClinic;
        public List<Animal> Animals { get; private set; }
        public List<Thing> Things { get; private set; }
        private int _inventoryCounter = 1;

        public Zoo(IVeterinaryClinic vetClinic)
        {
            _vetClinic = vetClinic;
            Animals = new List<Animal>();
            Things = new List<Thing>();
        }

        // Добавление животного с проверкой здоровья
        public void AddAnimal(Animal animal)
        {
            if (_vetClinic.CheckHealth(animal))
            {
                animal.Number = _inventoryCounter++;
                Animals.Add(animal);
                Console.WriteLine($"\nЖивотное {animal.Name} успешно добавлено в зоопарк.");
            }
            else
            {
                Console.WriteLine($"\nЖивотное {animal.Name} не прошло проверку здоровья и не было добавлено.");
            }
        }

        // Добавление вещей
        public void AddThing(Thing thing)
        {
            thing.Number = _inventoryCounter++;
            Things.Add(thing);
        }

        // Подсчет суммарного количества еды
        public int TotalFoodConsumption()
        {
            int total = 0;
            foreach (var animal in Animals)
            {
                total += animal.Food;
            }
            return total;
        }

        // Список животных для контактного зоопарка (травоядные с добротой > 5)
        public List<Herbo> GetInteractiveAnimals()
        {
            List<Herbo> result = new List<Herbo>();
            foreach (var animal in Animals)
            {
                if (animal is Herbo herb && herb.IsInteractive)
                {
                    result.Add(herb);
                }
            }
            return result;
        }

        // Вывод наименований и инвентарных номеров всех объектов
        public void ShowInventory()
        {
            Console.WriteLine("\nСписок всех объектов на балансе:");
            foreach (var animal in Animals)
            {
                Console.WriteLine($"[Животное] {animal.Name}, Инв. номер: {animal.Number}");
            }
            foreach (var thing in Things)
            {
                Console.WriteLine($"[Вещь] {thing.Name}, Инв. номер: {thing.Number}");
            }
        }
    }
}
