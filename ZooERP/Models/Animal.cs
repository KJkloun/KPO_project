using ZooERP.Interfaces;

namespace ZooERP.Models
{
    public abstract class Animal : IAlive, IInventory
    {
        public int Food { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }

        public Animal(string name, int food)
        {
            Name = name;
            Food = food;
        }
    }
}