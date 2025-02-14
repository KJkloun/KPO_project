using System;
using ZooERP.Interfaces;
using ZooERP.Models;

namespace ZooERP.Services
{
    public class VeterinaryClinic : IVeterinaryClinic
    {
        public bool CheckHealth(Animal animal)
        {
            Console.WriteLine($"\nПроводится осмотр животного: {animal.Name}.");
            Console.Write("Введите 'Y', если животное здорово, или любую другую клавишу для отказа: ");
            string input = Console.ReadLine();
            return input.Trim().ToUpper() == "Y";
        }
    }
}