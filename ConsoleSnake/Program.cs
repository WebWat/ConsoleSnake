using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleSnake
{
    class Program
    {
        private static ConsoleKey Key = default;
        private static bool GameOver = false;
        private static (int Left, int Top) Position = (1, 1);
        private static Queue<(int, int)> Coords = new();
        private static Random Random = new Random();
        private static List<(int, int)> ApplesCoords = new();
        private static int SnakeLength = 1;

        private static Thread Game = new Thread(new ThreadStart(Move));
        private static Thread Launcher = new Thread(new ThreadStart(Launch));

        static void Main(string[] args)
        {
            Console.SetWindowPosition(0, 0);
            Console.SetWindowSize(GameSettings.MapSize, GameSettings.MapSize);
            Console.SetBufferSize(GameSettings.MapSize, GameSettings.MapSize);
            Console.CursorVisible = false;

            Launcher.Start();

            while (!GameOver)
            {
                Thread.Sleep(5);
            }

            Launcher.Interrupt();

            Thread.Sleep(500);

            Console.Clear();

            Console.ReadKey(true);
        }


        private static void Launch()
        {
            RenderWall();

            RenderPosition();

            GenerateRandomAppleCoords(20);

            RenderApples();

            while (true)
            {
                var keyInfo = Console.ReadKey();

                Key = keyInfo.Key;

                if (!Game.IsAlive && Game.ThreadState != ThreadState.Stopped)
                {
                    Game.Start();
                }
            }
        }


        private static void ClearPixel((int Left, int Top) currentPosition)
        {
            for (int i = 0; i < GameSettings.PixelSize; i++)
            {
                Console.SetCursorPosition(currentPosition.Left * GameSettings.PixelSize, 
                                          currentPosition.Top * GameSettings.PixelSize + i);

                char[] output = new char[GameSettings.PixelSize];

                Array.Fill(output, ' ');

                Console.Write(output);
            }
        }


        private static void RenderPosition()
        {
            if (Coords.Count == SnakeLength)
            {
                if (Coords.TryDequeue(out (int, int) result))
                    ClearPixel(result);
            }

            var worm = new Pixel(Position.Left, Position.Top, '#');

            worm.Draw();
        }


        private static void Move()
        {
            while (true)
            {
                Coords.Enqueue(Position);

                switch (Key)
                {
                    case ConsoleKey.UpArrow:
                        Position.Top--;
                        break;

                    case ConsoleKey.DownArrow:
                        Position.Top++;
                        break;

                    case ConsoleKey.LeftArrow:
                        Position.Left--;
                        break;

                    case ConsoleKey.RightArrow:
                        Position.Left++;
                        break;
                }

                if (!IsValidPosition())
                    break;

                if (IsApplePosition())
                {
                    Coords.Enqueue(Position);
                    SnakeLength++;
                }

                RenderPosition();

                Thread.Sleep(GameSettings.Speed);
            }

            GameOver = true;
        }


        private static bool IsValidPosition()
        {
            return (Position.Top < GameSettings.MapSizeInPixels && Position.Top > 0) &&
                   (Position.Left < GameSettings.MapSizeInPixels && Position.Left > 0);
        }


        private static bool IsApplePosition()
        {
            var result = ApplesCoords.Any(i => i.Item1 == Position.Left && i.Item2 == Position.Top);

            ApplesCoords.Remove(Position);

            return result;
        }


        private static void GenerateRandomAppleCoords(int count)
        {
            for (int i = 0; i < count; i++)
            {
                int top = Random.Next(2, GameSettings.MapSizeInPixels);
                int left = Random.Next(2, GameSettings.MapSizeInPixels);

                ApplesCoords.Add((left, top));
            }
        }


        private static void RenderApples()
        {
            char wallSymbol = '@';

            foreach (var item in ApplesCoords)
            {
                var pixel = new Pixel(item.Item1, item.Item2, wallSymbol);

                pixel.Draw();
            }
        }


        private static void RenderWall()
        {
            char wallSymbol = '+';

            for (int i = 0; i < GameSettings.MapSizeInPixels + 1; i++)
            {
                var pixel_1 = new Pixel(0, i, wallSymbol);
                var pixel_2 = new Pixel(GameSettings.MapSizeInPixels, i, wallSymbol);

                pixel_1.Draw();
                pixel_2.Draw();
            }

            for (int i = 1; i < GameSettings.MapSizeInPixels; i++)
            {
                var pixel_1 = new Pixel(i, 0, wallSymbol);
                var pixel_2 = new Pixel(i, GameSettings.MapSizeInPixels, wallSymbol);

                pixel_1.Draw();
                pixel_2.Draw();
            }
        }
    }
}
