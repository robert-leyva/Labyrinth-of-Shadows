namespace MazeG;

public interface ITramp : ItemInASquare
{
    string Type { get; } // identificador de trampa
    void Interact(Player player);
}