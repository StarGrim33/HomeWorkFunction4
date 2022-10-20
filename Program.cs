namespace HomeWorkFunction4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Random random = new Random();

            bool isPlaying = true;

            int packmanX;
            int packmanY;
            int packmanDX = 0, packmanDY = 1;
            int allDots = 0;
            bool isAlive = true;
            int ghostX, ghostY; ;
            int ghostDX = 0, ghostDY = -1;
            int collectedDots = 0;

            char[,] map = ReadMap("map1", out packmanX, out packmanY, out ghostX, out ghostY, ref allDots);

            DrawMap(map);

            while (isPlaying)
            {
                Console.SetCursorPosition(0, 20);
                Console.WriteLine($"Собрано: {collectedDots}/{allDots}");
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    ChangeDirection(key, ref packmanDX, ref packmanDY);
                }

                if (map[packmanX + packmanDX, packmanY + packmanDY] != '#')
                {
                    CollectDots(map, packmanX, packmanY, ref collectedDots);

                    Move(map, '@', ref packmanX, ref packmanY, packmanDX, packmanDY);

                }

                if (map[ghostX + ghostDX, ghostY + ghostDY] != '#')
                {
                    Move(map, '$', ref ghostX, ref ghostY, ghostDX, ghostDY);
                }
                else
                {
                    ChangeDirection(random, ref ghostDX, ref ghostDY);
                }

                System.Threading.Thread.Sleep(250);

                if (ghostX == packmanX && ghostY == packmanY)
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

        static void CollectDots(char[,] map, int packmanX, int packmanY, ref int collectedDots)
        {
            if (map[packmanX, packmanY] == '.')
            {
                collectedDots++;
                map[packmanX, packmanY] = ' ';
            }
        }

        static void Move(char[,] map, char symbol, ref int X, ref int Y, int DX, int DY)
        {
            Console.SetCursorPosition(Y, X);
            Console.Write(map[X, Y]);

            X += DX;
            Y += DY;

            Console.SetCursorPosition(Y, X);
            Console.Write(symbol);
        }

        static void ChangeDirection(ConsoleKeyInfo key, ref int DX, ref int DY)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    DX = -1; DY = 0;
                    break;
                case ConsoleKey.DownArrow:
                    DX = 1; DY = 0;
                    break;
                case ConsoleKey.LeftArrow:
                    DX = 0; DY = -1;
                    break;
                case ConsoleKey.RightArrow:
                    DX = 0; DY = 1;
                    break;
            }
        }

        static void ChangeDirection(Random random, ref int DX, ref int DY)
        {
            int ghostDir = random.Next(1, 5);

            switch (ghostDir)
            {
                case 1:
                    DX = -1; DY = 0;
                    break;
                case 2:
                    DX = 1; DY = 0;
                    break;
                case 3:
                    DX = 0; DY = -1;
                    break;
                case 4:
                    DX = 0; DY = 1;
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

        static char[,] ReadMap(string mapName, out int packmanX, out int packmanY, out int ghostX, out int ghostY, ref int allDots)
        {
            packmanX = 0;
            packmanY = 0;
            ghostX = 0;
            ghostY = 0;

            string[] newFile = File.ReadAllLines($"Maps/{mapName}.txt");
            char[,] map = new char[newFile.Length, newFile[0].Length];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];

                    if (map[i, j] == '@')
                    {
                        packmanX = i;
                        packmanY = j;
                        map[i, j] = '.';
                    }
                    else if (map[i, j] == '$')
                    {
                        ghostX = i;
                        ghostY = j;
                        map[i, j] = '.';
                    }
                    else if (map[i, j] == ' ')
                    {
                        map[i, j] = '.';
                        allDots++;
                    }
                }
            }
            return map;

        }
    }
}