namespace MazeG
{
    public class Square
    {
        public int X { get; }
        public int Y { get; }
        public bool IsWall { get; set; }
        public ITramp? Item { get; set; }  // Suponiendo que ya lo has cambiado a ITramp
        // Ahora guardamos una lista de jugadores
        public List<Player> Players { get; } = new List<Player>();

        public Square(int x, int y)
        {
            X = x;
            Y = y;
            IsWall = true;
        }
    }
}