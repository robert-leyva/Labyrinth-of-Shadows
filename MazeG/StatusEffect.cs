namespace MazeG;

public class StatusEffect
{
    public int Duration { get; set; } // Duración del efecto en turnos
    public Action<Player> OnTurn { get; } // Acción que se ejecuta en cada turno
    public Action<Player> OnFinish { get; } // Acción que se ejecuta al finalizar el efecto

    // Constructor que inicializa la duración y las acciones
    public StatusEffect(int duration, Action<Player> onTurn, Action<Player> onFinish)
    {
        Duration = duration;
        OnTurn = onTurn;
        OnFinish = onFinish;
    }

    // Método para aplicar el efecto en el turno actual
    public void ApplyEffect(Player player)
    {
        OnTurn?.Invoke(player); // Ejecuta la acción del turno si existe
        Duration--; // Reduce la duración restante del efecto
        if (Duration <= 0)
        {
            OnFinish?.Invoke(player); // Ejecuta la acción final si la duración se agotó
        }
    }
}