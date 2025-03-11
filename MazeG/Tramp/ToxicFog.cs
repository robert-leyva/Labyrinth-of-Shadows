using Spectre.Console;

namespace MazeG;

public class ToxicFog : ITramp
{
    public string Type => "Niebla";

    public void Print()
    {
        AnsiConsole.Markup("[purple]💀[/]"); // Representación visual
    }

    public void Interact(Player player)
    {
        // Aplica un efecto de estado
        player.Effects.Add(new StatusEffect(
            duration: 3,
            onTurn: (player) =>
            {
                player.LifePoints -= 5;
                player.Speed -= 1;
                AnsiConsole.MarkupLine($"[purple]La niebla venenosa afecta a {player.Name} (-5 HP, -1 velocidad).[/]");
            },
            onFinish: (player) =>
            {
                player.Speed += 1;
                AnsiConsole.MarkupLine($"[green]{player.Name} se liberó de la niebla venenosa.[/]");
            }
        ));
    }
}