namespace MazeG
{
    public class Orion : Player
    {
        public Orion() : base("Orion", "⚡", 70, 5, 0, 0, "[rgb(173,216,230)]")
        {
        }

        public override void UseSkill()
        {
            // Su habilidad de evasión se activa automáticamente.
        }
    }
}