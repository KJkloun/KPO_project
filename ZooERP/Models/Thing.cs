using ZooERP.Interfaces;

namespace ZooERP.Models
{
    public abstract class Thing : IInventory
    {
        public int Number { get; set; }
        public string Name { get; set; }

        public Thing(string name)
        {
            Name = name;
        }
    }
}