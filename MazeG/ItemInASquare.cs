namespace MazeG;

public interface ItemInASquare
{
    // Método para dibujar el elemento en consola
    void Print();

    // Método para interactuar con un jugador que entra en esta casilla
    void Interact(Player player);
}