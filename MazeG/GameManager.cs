using System;
using System.Collections.Generic;
using Spectre.Console;

namespace MazeG
{
    public class GameManager
    {
        private Board board;
        private List<Player> players;
        private int currentTurn = 0;
        private bool gameWon = false;
        private Random random = new Random();

        public void Run()
        {
            SetupGame();

            while (!gameWon)
            {
                // Procesa los efectos de estado del jugador al inicio del turno.
                Player currentPlayer = players[currentTurn];
                currentPlayer.ProcessEffects();

                RenderGameState();
                Console.WriteLine($"\nTurno de {currentPlayer.Name} (Velocidad: {currentPlayer.Speed}).");

                int stepsTaken = 0;
                bool turnEnded = false;

                while (stepsTaken < currentPlayer.Speed && !turnEnded)
                {
                    MostrarInstruccionesMovimiento(currentPlayer);
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    // Permite a Mara usar su habilidad de curación.
                    if (currentPlayer is Mara && keyInfo.Key == ConsoleKey.H)
                    {
                        currentPlayer.UseSkill();
                        Console.WriteLine($"{currentPlayer.Name} ha activado su habilidad de curación.");
                        Console.WriteLine($"[Detalle] {currentPlayer.Name} ahora tiene {currentPlayer.LifePoints} HP y {currentPlayer.Speed} de velocidad.");
                        Console.WriteLine("Presione cualquier tecla para continuar...");
                        Console.ReadKey(true);
                        turnEnded = true;
                        break;
                    }

                    // Obtener la dirección del movimiento.
                    if (!ObtenerDireccion(keyInfo, out int dx, out int dy))
                    {
                        Console.WriteLine("Tecla inválida. Usa W, A, S o D (o H para curar si eres Mara).");
                        continue;
                    }

                    int newX = currentPlayer.X + dx;
                    int newY = currentPlayer.Y + dy;

                    // Validación: el destino debe estar en rango y no ser una pared.
                    if (!board.IsValid(newX, newY) || board.Squares[newX, newY].IsWall)
                    {
                        Console.WriteLine("Movimiento inválido: la celda está bloqueada, fuera de rango o es una pared. No puedes deformar el laberinto.");
                        continue;
                    }

                    // Mover al jugador sin modificar la estructura del laberinto.
                    MoverJugador(currentPlayer, newX, newY);
                    stepsTaken++;
                    RenderGameState();

                    // Comprobar si se alcanzó la meta.
                    if (newX == board.ExitX && newY == board.ExitY)
                    {
                        Console.WriteLine($"\n¡{currentPlayer.Name} ha alcanzado la meta y gana la partida!");
                        gameWon = true;
                        turnEnded = true;
                        break;
                    }

                    // Si hay una trampa en la nueva celda, maneja la interacción.
                    if (board.Squares[newX, newY].Item != null)
                    {
                        ManejarInteraccionTrampa(currentPlayer, board.Squares[newX, newY].Item, newX, newY);
                        turnEnded = true;
                        break;
                    }
                } // Fin del bucle de movimientos del turno.

                if (!gameWon)
                {
                    // Pasa al siguiente jugador.
                    currentTurn = (currentTurn + 1) % players.Count;
                }
            } // Fin del bucle principal.

            Console.WriteLine("¡Fin del juego!");
        }

        // Mueve al jugador de la celda origen a la celda destino.
        private void MoverJugador(Player player, int newX, int newY)
        {
            board.Squares[player.X, player.Y].Players.Remove(player);
            player.X = newX;
            player.Y = newY;
            board.Squares[newX, newY].Players.Add(player);
        }

        // Muestra instrucciones de movimiento según el tipo de jugador.
        private void MostrarInstruccionesMovimiento(Player player)
        {
            if (player is Mara)
                Console.WriteLine("Presione W/A/S/D para moverse un paso o H para curar.");
            else
                Console.WriteLine("Presione W/A/S/D para moverse un paso:");
        }

        // Convierte la tecla presionada en un desplazamiento (dx, dy).
        private bool ObtenerDireccion(ConsoleKeyInfo keyInfo, out int dx, out int dy)
        {
            dx = 0;
            dy = 0;
            switch (keyInfo.Key)
            {
                case ConsoleKey.W:
                    dy = -1;
                    return true;
                case ConsoleKey.S:
                    dy = 1;
                    return true;
                case ConsoleKey.A:
                    dx = -1;
                    return true;
                case ConsoleKey.D:
                    dx = 1;
                    return true;
                default:
                    return false;
            }
        }

        // Actualiza la posición del jugador en el tablero luego de la interacción con una trampa.
        // Después de ejecutar la trampa se muestra un mensaje con las estadísticas actualizadas y se espera a que el usuario presione una tecla.
        private void TrapInteractWithUpdate(ITramp trap, Player player, int oldX, int oldY)
        {
            trap.Interact(player);
            Console.WriteLine($"[Detalle] {player.Name} ahora tiene {player.LifePoints} HP y {player.Speed} de velocidad.");
            // Se espera para que el usuario pueda ver el mensaje.
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey(true);
            
            if (player.X != oldX || player.Y != oldY)
            {
                if (board.Squares[oldX, oldY].Players.Contains(player))
                    board.Squares[oldX, oldY].Players.Remove(player);
                if (!board.Squares[player.X, player.Y].Players.Contains(player))
                    board.Squares[player.X, player.Y].Players.Add(player);
            }
        }

        // Maneja la interacción de un jugador al toparse con una trampa.
        private void ManejarInteraccionTrampa(Player currentPlayer, ITramp trap, int cellX, int cellY)
        {
            Console.WriteLine("¡Has encontrado una trampa!");

            if (currentPlayer is Thalia)
            {
                if (random.NextDouble() < 0.5)
                {
                    Console.WriteLine("Thalia evade la trampa automáticamente!");
                    board.Squares[cellX, cellY].Item = null;
                    Console.WriteLine($"[Detalle] {currentPlayer.Name} mantiene {currentPlayer.LifePoints} HP y {currentPlayer.Speed} de velocidad.");
                    Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey(true);
                }
                else
                {
                    Console.WriteLine("Thalia no logró evadir la trampa.");
                    TrapInteractWithUpdate(trap, currentPlayer, cellX, cellY);
                }
            }
            else if (currentPlayer is Garrick g)
            {
                if (trap is IllusoryWalls && g.RemainingUses > 0)
                {
                    Console.WriteLine("Garrick rompe el muro ilusorio automáticamente!");
                    board.Squares[cellX, cellY].Item = null;
                    g.RemainingUses--;
                    Console.WriteLine($"[Detalle] {currentPlayer.Name} mantiene {currentPlayer.LifePoints} HP y {currentPlayer.Speed} de velocidad. (Usos restantes: {g.RemainingUses})");
                    Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey(true);
                }
                else
                {
                    TrapInteractWithUpdate(trap, currentPlayer, cellX, cellY);
                }
            }
            else if (currentPlayer is Orion)
            {
                if (currentPlayer.Speed > 3)
                {
                    Console.WriteLine("Orion evade la trampa automáticamente.");
                    board.Squares[cellX, cellY].Item = null;
                    Console.WriteLine($"[Detalle] {currentPlayer.Name} mantiene {currentPlayer.LifePoints} HP y {currentPlayer.Speed} de velocidad.");
                    Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey(true);
                }
                else
                {
                    TrapInteractWithUpdate(trap, currentPlayer, cellX, cellY);
                }
            }
            else if (currentPlayer is Lira li)
            {
                Console.WriteLine($"Presione B para activar Bola de Fuego (usos restantes: {li.RemainingUses}) o F para enfrentar la trampa.");
                ConsoleKeyInfo choiceKey = Console.ReadKey(true);
                bool handled = false;
                if (choiceKey.Key == ConsoleKey.B)
                {
                    if (li.RemainingUses > 0)
                    {
                        Console.WriteLine("Ingrese la dirección para quemar trampas (W/A/S/D):");
                        ConsoleKeyInfo dirKey = Console.ReadKey(true);
                        if (!ObtenerDireccion(dirKey, out int ddx, out int ddy))
                        {
                            Console.WriteLine("Dirección inválida. Enfrentarás la trampa.");
                            TrapInteractWithUpdate(trap, currentPlayer, cellX, cellY);
                            handled = true;
                        }
                        if (!handled)
                        {
                            int tx = cellX, ty = cellY;
                            while (board.IsValid(tx, ty) && !board.Squares[tx, ty].IsWall)
                            {
                                board.Squares[tx, ty].Item = null;
                                tx += ddx;
                                ty += ddy;
                            }
                            Console.WriteLine("Bola de Fuego activada: trampas quemadas en esa línea.");
                            li.RemainingUses--;
                            Console.WriteLine($"[Detalle] {currentPlayer.Name} ahora tiene {currentPlayer.LifePoints} HP y {currentPlayer.Speed} de velocidad.");
                            Console.WriteLine("Presione cualquier tecla para continuar...");
                            Console.ReadKey(true);
                            handled = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No quedan usos para Bola de Fuego. Enfrentarás la trampa.");
                        TrapInteractWithUpdate(trap, currentPlayer, cellX, cellY);
                        handled = true;
                    }
                }
                if (!handled)
                {
                    if (choiceKey.Key == ConsoleKey.F)
                    {
                        TrapInteractWithUpdate(trap, currentPlayer, cellX, cellY);
                    }
                    else
                    {
                        Console.WriteLine("Entrada inválida. Enfrentarás la trampa.");
                        TrapInteractWithUpdate(trap, currentPlayer, cellX, cellY);
                    }
                }
            }
            else
            {
                TrapInteractWithUpdate(trap, currentPlayer, cellX, cellY);
            }
        }

        // Configura el juego limitando la cantidad de jugadores a 2.
        private void SetupGame()
        {
            Console.Write("Ingrese la cantidad de jugadores (Solo se permiten 2 jugadores): ");
            int numPlayers;
            while (!int.TryParse(Console.ReadLine(), out numPlayers) || numPlayers != 2)
            {
                Console.Write("Cantidad inválida. Debe ser exactamente 2: ");
            }

            players = new List<Player>();
            for (int i = 0; i < numPlayers; i++)
            {
                Console.WriteLine($"\nSeleccione el tipo para el jugador #{i + 1}:");
                Console.WriteLine("1. Thalia");
                Console.WriteLine("2. Garrick");
                Console.WriteLine("3. Lira");
                Console.WriteLine("4. Orion");
                Console.WriteLine("5. Mara");
                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 5)
                {
                    Console.WriteLine("Selección inválida. Ingrese un número entre 1 y 5.");
                }
                switch (choice)
                {
                    case 1:
                        players.Add(new Thalia());
                        break;
                    case 2:
                        players.Add(new Garrick());
                        break;
                    case 3:
                        players.Add(new Lira());
                        break;
                    case 4:
                        players.Add(new Orion());
                        break;
                    case 5:
                        players.Add(new Mara());
                        break;
                }
            }

            int boardSize = 25;
            board = new Board(boardSize);
            board.PlacePlayers(players.ToArray());
            board.PlaceTraps();
        }

        // Limpia la consola y muestra el estado del tablero y el detalle de los jugadores.
        private void RenderGameState()
        {
            Console.Clear();
            board.PrintBoard(players[currentTurn]);
            Console.WriteLine("\nDetalles de jugadores:");
            foreach (var player in players)
            {
                Console.WriteLine($"{player.Name}: Vida: {player.LifePoints}, Velocidad: {player.Speed} (Posición: {player.X}, {player.Y})");
            }
        }
    }
}