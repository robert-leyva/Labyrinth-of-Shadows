namespace MazeG
{
    public class Thalia : Player
    {
        public Thalia() : base("Thalia", "🌬", 80, 4, 0, 0, "[rgb(255,0,255)]")
        {
        }

        public override void UseSkill()
        {
            // La evasión de trampas se gestiona automáticamente en GameManager.
        }
    }
}