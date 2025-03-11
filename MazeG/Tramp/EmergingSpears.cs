using Spectre.Console;

namespace MazeG;

public class EmergingSpears : ITramp
{
    public string Type => "Lanzas";

    public void Print()
    {
        AnsiConsole.Markup("[red]💀[/]"); // Calavera roja
    }

    public void Interact(Player player)
    {
        player.LifePoints -= 20;
        AnsiConsole.MarkupLine($"[red]¡{player.Name} pisó lanzas! (-20 HP)[/]");
    }
}