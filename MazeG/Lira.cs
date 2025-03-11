namespace MazeG
{
    public class Lira : Player
    {
        public int RemainingUses { get; set; } = 3; // Puede usar la Bola de Fuego 3 veces.

        public Lira() : base("Lira", "🔥", 90, 3, 0, 0, "rgb[(250,128,114)]")
        {
        }

        public override void UseSkill()
        {
            // Se invoca cuando el jugador decide usar la habilidad de Bola de Fuego.
        }
    }
}