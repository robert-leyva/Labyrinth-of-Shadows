using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;

namespace MazeG
{
    public class Board
    {
        public Square[,] Squares { get; }   // Matriz de celdas del tablero.
        public int Size { get; }            // Tamaño del tablero (por ejemplo, 15 para un 15x15).
        public int ExitX { get; set; }      // Coordenada X de la celda meta.
        public int ExitY { get; set; }      // Coordenada Y de la celda meta.

        private Random rand = new Random();

        public Board(int size)
        {
            Size = size;
            Squares = new Square[size, size];
            InitializeSquares();
            GenerateMaze(1, 1);
            ExitX = Size - 2;
            ExitY = Size - 2;
            if (Squares[ExitX, ExitY].IsWall)
                Squares[ExitX, ExitY].IsWall = false;
        }

        private void InitializeSquares()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    Squares[x, y] = new Square(x, y);
                }
            }
        }

        // Generación de laberinto mediante un DFS recursivo que trabaja en pasos de 2 celdas.
        private void GenerateMaze(int x, int y)
        {
            Squares[x, y].IsWall = false; // Marca la celda actual como camino.
            int[][] directions = new int[4][]
            {
                new int[] { 0, -2 }, // Norte.
                new int[] { 0, 2 },  // Sur.
                new int[] { 2, 0 },  // Este.
                new int[] { -2, 0 }  // Oeste.
            };

            Shuffle(directions, rand);

            foreach (var dir in directions)
            {
                int newX = x + dir[0];
                int newY = y + dir[1];
                if (IsValid(newX, newY) && Squares[newX, newY].IsWall)
                {
                    int deltaX = dir[0] / 2;
                    int deltaY = dir[1] / 2;
                    int wallX = x + deltaX;
                    int wallY = y + deltaY;
                    Squares[wallX, wallY].IsWall = false; // Rompe la pared intermedia.
                    GenerateMaze(newX, newY);
                }
            }
        }

        public bool IsValid(int x, int y)
        {
            return x >= 0 && x < Size && y >= 0 && y < Size;
        }

        private void Shuffle<T>(T[] array, Random rand)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }
    
        // Coloca a los jugadores en celdas de inicio vacías (se selecciona celdas sin jugadores aún).
        public void PlacePlayers(Player[] players)
        {
            int startX = 1;
            int startY = 1;
            int placedPlayers = 0;
            int blockSize = (int)Math.Ceiling(Math.Sqrt(players.Length));

            for (int i = 0; i < blockSize && placedPlayers < players.Length; i++)
            {
                for (int j = 0; j < blockSize && placedPlayers < players.Length; j++)
                {
                    int posX = startX + i;
                    int posY = startY + j;
                    if (IsValid(posX, posY) && !Squares[posX, posY].IsWall && Squares[posX, posY].Players.Count == 0)
                    {
                        players[placedPlayers].X = posX;
                        players[placedPlayers].Y = posY;
                        Squares[posX, posY].Players.Add(players[placedPlayers]);
                        placedPlayers++;
                    }
                }
            }
        }

        // Coloca trampas en celdas libres (no paredes, sin jugadores y sin trampas) evitando la celda meta.
        public void PlaceTraps()
        {
            double trapProbability = 0.15; // Ajusta la probabilidad si es necesario.
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (!Squares[x, y].IsWall && Squares[x, y].Players.Count == 0 && Squares[x, y].Item == null)
                    {
                        if (x == ExitX && y == ExitY)
                            continue;
                        if (rand.NextDouble() < trapProbability)
                        {
                            int trapType = rand.Next(3);
                            ITramp trap;
                            switch (trapType)
                            {
                                case 0:
                                    trap = new EmergingSpears();
                                    break;
                                case 1:
                                    trap = new ToxicFog();
                                    break;
                                case 2:
                                    trap = new IllusoryWalls();
                                    break;
                                default:
                                    trap = new EmergingSpears();
                                    break;
                            }
                            Squares[x, y].Item = trap;
                            Console.WriteLine($"Trampa generada en ({x}, {y}) del tipo {trap.Type}.");
                        }
                    }
                }
            }
        }

        // Imprime el tablero en consola.
        // Si hay jugadores en una celda, se muestra únicamente el emoji del jugador cuyo turno es (si está allí),
        // de lo contrario, se muestra el emoji del primer jugador en la lista.
        // En la celda meta se muestra el icono especial ⛳ en verde si está vacía.
        public void PrintBoard(Player currentPlayer)
        {
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    // Celda meta
                    if (x == ExitX && y == ExitY)
                    {
                        if (Squares[x, y].Players.Any())
                        {
                            if (Squares[x, y].Players.Contains(currentPlayer))
                                Console.Write(currentPlayer.Emoji);
                            else
                                Console.Write(Squares[x, y].Players[0].Emoji);
                        }
                        else if (Squares[x, y].Item != null)
                        {
                            Squares[x, y].Item.Print();
                        }
                        else
                        {
                            AnsiConsole.Markup("[green]⛳[/]");
                        }
                    }
                    else if (Squares[x, y].Players.Any())
                    {
                        if (Squares[x, y].Players.Contains(currentPlayer))
                            Console.Write(currentPlayer.Emoji);
                        else
                            Console.Write(Squares[x, y].Players[0].Emoji);
                    }
                    else if (Squares[x, y].Item != null)
                    {
                        Squares[x, y].Item.Print();
                    }
                    else if (Squares[x, y].IsWall)
                    {
                        AnsiConsole.Markup("[rgb(165,42,42)]██[/]");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
