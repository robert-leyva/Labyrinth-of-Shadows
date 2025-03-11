namespace MazeG
{
    public class Garrick : Player
    {
        public int RemainingUses { get; set; } = 2; // Puede romper muros ilusorios 2 veces.

        public Garrick() : base("Garrick", "🤜", 120, 2, 0, 0, "[(139,0,0)]")
        {
        }

        public override void UseSkill()
        {
            // Se activa automáticamente al pisar un muro ilusorio en el GameManager.
        }
    }
}