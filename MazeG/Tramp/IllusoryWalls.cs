using Spectre.Console;

namespace MazeG;

public class IllusoryWalls : ITramp
{
  public string Type => "ParedFalsa";

  public void Print()
  {
    AnsiConsole.Markup("[rgb(101,67,33)]💀[/]"); //Calavera amarilla
    
  }

  public void Interact(Player player)
  {
    player.X = 0; //Posicion inicial x
    player.Y = 0; //Posicion inicial Y
    AnsiConsole.Markup($"[yellow]¡{player.Name} cayó en una pared falsa! (Reinicio)[/]");
  }
}