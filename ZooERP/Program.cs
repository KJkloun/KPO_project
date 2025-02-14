using System;
using Microsoft.Extensions.DependencyInjection;
using ZooERP.Managers;
using ZooERP.Models;
using ZooERP.Services;

namespace ZooERP
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Настройка DI-контейнера
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var zoo = serviceProvider.GetService<Zoo>();

            // Добавление предварительных вещей (например, стол и компьютер)
            zoo.AddThing(new Table("Столовая"));
            zoo.AddThing(new Computer("Компьютерный класс"));

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n--- Меню ---");
                Console.WriteLine("1. Добавить новое животное");
                Console.WriteLine("2. Вывести отчет по животным");
                Console.WriteLine("3. Вывести список животных для контактного зоопарка");
                Console.WriteLine("4. Вывести список всех объектов");
                Console.WriteLine("5. Выход");
                Console.Write("Выберите опцию: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddAnimalMenu(zoo);
                        break;
                    case "2":
                        ReportAnimals(zoo);
                        break;
                    case "3":
                        ListInteractiveAnimals(zoo);
                        break;
                    case "4":
                        zoo.ShowInventory();
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        // Регистрация зависимостей
        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<ZooERP.Interfaces.IVeterinaryClinic, VeterinaryClinic>();
            services.AddSingleton<Zoo>();
        }

        // Меню для добавления нового животного
        private static void AddAnimalMenu(Managers.Zoo zoo)
        {
            Console.WriteLine("\nВыберите тип животного:");
            Console.WriteLine("1. Обезьяна (травоядное)");
            Console.WriteLine("2. Кролик (травоядное)");
            Console.WriteLine("3. Тигр (хищник)");
            Console.WriteLine("4. Волк (хищник)");
            Console.Write("Введите номер типа: ");
            string typeChoice = Console.ReadLine();

            Console.Write("Введите имя животного: ");
            string name = Console.ReadLine();

            int food = ReadIntFromConsole("Введите количество потребляемой еды в кг в сутки: ");

            Animal animal = null;
            switch (typeChoice)
            {
                case "1":
                    int kindnessMonkey = ReadIntFromConsole("Введите уровень доброты (0-10): ");
                    animal = new Monkey(name, food, kindnessMonkey);
                    break;
                case "2":
                    int kindnessRabbit = ReadIntFromConsole("Введите уровень доброты (0-10): ");
                    animal = new Rabbit(name, food, kindnessRabbit);
                    break;
                case "3":
                    animal = new Tiger(name, food);
                    break;
                case "4":
                    animal = new Wolf(name, food);
                    break;
                default:
                    Console.WriteLine("Неверный выбор типа животного.");
                    return;
            }

            zoo.AddAnimal(animal);
        }

        // Вспомогательный метод для безопасного ввода числа
        private static int ReadIntFromConsole(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (int.TryParse(input, out value))
                    break;
                Console.WriteLine("Неверное значение. Попробуйте снова.");
            }
            return value;
        }

        // Вывод отчёта по животным
        private static void ReportAnimals(Managers.Zoo zoo)
        {
            Console.WriteLine("\nОтчет по животным:");
            Console.WriteLine($"Количество животных: {zoo.Animals.Count}");
            Console.WriteLine($"Суммарное количество потребляемой еды: {zoo.TotalFoodConsumption()} кг в сутки.");
        }

        // Вывод списка животных для контактного зоопарка
        private static void ListInteractiveAnimals(Managers.Zoo zoo)
        {
            Console.WriteLine("\nЖивотные для контактного зоопарка (травоядные с уровнем доброты > 5):");
            var interactiveAnimals = zoo.GetInteractiveAnimals();
            if (interactiveAnimals.Count == 0)
            {
                Console.WriteLine("Нет подходящих животных для контактного зоопарка.");
            }
            else
            {
                foreach (var animal in interactiveAnimals)
                {
                    Console.WriteLine($"{animal.Name} (Инв. номер: {animal.Number}, Уровень доброты: {animal.Kindness})");
                }
            }
        }
    }


}
