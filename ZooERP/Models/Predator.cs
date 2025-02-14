namespace ZooERP.Models
{
    public abstract class Predator : Animal
    {
        public Predator(string name, int food)
            : base(name, food)
        {
        }
    }
}