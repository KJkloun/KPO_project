namespace ZooERP.Models
{
    public abstract class Herbo : Animal
    {
        public int Kindness { get; set; } // Уровень доброты (0-10)
        public bool IsInteractive => Kindness > 5;

        public Herbo(string name, int food, int kindness)
            : base(name, food)
        {
            Kindness = kindness;
        }
    }
}