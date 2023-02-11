using System.Numerics;
using System;
using System.Runtime.Intrinsics;

using System.Numerics;
using System;
using System.Runtime.Intrinsics;

namespace HomeWorkFunction4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Random random = new Random();

            bool isPlaying = true;
            bool isAlive = true;

            int packmanPositionX;
            int packmanPositionY;
            int packmanDestinationX = 0;
            int packmanDestionationY = 1;

            int ghostPositionX;
            int ghostPositionY;
            int ghostDestinationX = 0;
            int ghostDestionationY = -1;

            char wall = '#';
            char ghost = '$';
            char player = '@';
            char emptySpace = ' ';
            char dot = '.';

            int allDots = 0;
            int collectedDots = 0;

            char[,] map = ReadMap("map1", dot, player, ghost, emptySpace, out packmanPositionX, out packmanPositionY, out ghostPositionX, out ghostPositionY, ref allDots);

            DrawMap(map);

            while (isPlaying)
            {
                Console.SetCursorPosition(0, 20);
                Console.WriteLine($"Собрано: {collectedDots}/{allDots}");

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    ChangeDirection(key, ref packmanDestinationX, ref packmanDestionationY);
                }

                if (map[packmanPositionX + packmanDestinationX, packmanPositionY + packmanDestionationY] != wall)
                {
                    CollectDots(map, dot, emptySpace, packmanPositionX, packmanPositionY, ref collectedDots);

                    Move(map, player, ref packmanPositionX, ref packmanPositionY, packmanDestinationX, packmanDestionationY);
                }

                if (map[ghostPositionX + ghostDestinationX, ghostPositionY + ghostDestionationY] != wall)
                {
                    Move(map, ghost, ref ghostPositionX, ref ghostPositionY, ghostDestinationX, ghostDestionationY);
                }
                else
                {
                    ChangeDirection(random, ref ghostDestinationX, ref ghostDestionationY);
                }

                System.Threading.Thread.Sleep(250);

                if (ghostPositionX == packmanPositionX && ghostPositionY == packmanPositionY)
                {
                    isAlive = false;
                }

                if (collectedDots == allDots || isAlive == false)
                {
                    isPlaying = false;
                }
            }

            Console.SetCursorPosition(0, 25);

            if (collectedDots == allDots)
            {
                Console.WriteLine("Вы победили");
            }
            else if (isAlive == false)
            {
                Console.WriteLine("Вы лох");
            }
        }

        static void CollectDots(char[,] map, char dot, char emptySpace, int packmanPositionX, int packmanPositionY, ref int collectedDots)
        {
            if (map[packmanPositionX, packmanPositionY] == dot)
            {
                collectedDots++;
                map[packmanPositionX, packmanPositionY] = emptySpace;
            }
        }

        static void Move(char[,] map, char symbol, ref int vectorX, ref int vectorY, int destinationX, int destinationY)
        {
            RenderElementGame(map, vectorX, vectorY);

            vectorX += destinationX;
            vectorY += destinationY;

            Console.SetCursorPosition(vectorY, vectorX);
            Console.Write(symbol);
        }

        static void ChangeDirection(ConsoleKeyInfo key, ref int destinationX, ref int destinationY)
        {
            const ConsoleKey CommandUpArrow = ConsoleKey.UpArrow;
            const ConsoleKey CommandDownArrow = ConsoleKey.DownArrow;
            const ConsoleKey CommandLeftArrow = ConsoleKey.LeftArrow;
            const ConsoleKey CommandRightArrow = ConsoleKey.RightArrow;

            destinationX = 0;
            destinationY = 0;

            switch (key.Key)
            {
                case CommandUpArrow:
                    destinationX = -1;
                    break;

                case CommandDownArrow:
                    destinationX = 1;
                    break;

                case CommandLeftArrow:
                    destinationY = -1;
                    break;

                case CommandRightArrow:
                    destinationY = 1;
                    break;
            }
        }

        static void ChangeDirection(Random random, ref int destinationX, ref int destinationY)
        {
            int ghostDirection = random.Next(1, 5);

            switch (ghostDirection)
            {
                case 1:
                    destinationX = -1;
                    destinationY = 0;
                    break;

                case 2:
                    destinationX = 1;
                    destinationY = 0;
                    break;

                case 3:
                    destinationX = 0;
                    destinationY = -1;
                    break;

                case 4:
                    destinationX = 0;
                    destinationY = 1;
                    break;
            }
        }

        static void DrawMap(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }

                Console.WriteLine();
            }
        }

        static char[,] ReadMap(string mapName, char dot, char player, char ghost, char emptySpace, out int packmanPositionX, out int packmanPositionY, out int ghostPositionX, out int ghostPositionY, ref int allDots)
        {
            packmanPositionX = 0;
            packmanPositionY = 0;
            ghostPositionX = 0;
            ghostPositionY = 0;

            string[] newFile = File.ReadAllLines($"Maps/{mapName}.txt");
            char[,] map = new char[newFile.Length, newFile[0].Length];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];

                    if (map[i, j] == player)
                    {
                        packmanPositionX = i;
                        packmanPositionY = j;
                        map[i, j] = dot;
                    }
                    else if (map[i, j] == ghost)
                    {
                        ghostPositionX = i;
                        ghostPositionY = j;
                        map[i, j] = dot;
                    }
                    else if (map[i, j] == emptySpace)
                    {
                        map[i, j] = dot;
                        allDots++;
                    }
                }
            }

            return map;
        }

        static void RenderElementGame(char[,] map, int vectorX, int vectorY)
        {
            Console.SetCursorPosition(vectorY, vectorX);
            Console.Write(map[vectorX, vectorY]);
        }
    }
}
