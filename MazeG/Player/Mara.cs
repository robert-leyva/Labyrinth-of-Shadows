namespace MazeG
{
    public class Mara : Player
    {
        public int HealingUses { get; set; } = 2; // Limitar la cantidad de curaciones.
    
        public Mara() : base("Mara", "❤️", 100, 3, 0, 0, "rgb[(212,175,55)]")
        {
        }

        public override void UseSkill()
        {
            if (HealingUses > 0)
            {
                int healingAmount = 30;
                LifePoints = (LifePoints + healingAmount > 100) ? 100 : LifePoints + healingAmount;
                HealingUses--;
            }
            else
            {
                Console.WriteLine("No te quedan usos de curación.");
            }
        }
    }
}