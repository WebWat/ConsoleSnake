using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ConsoleSnake
{
    class Program
    {
        private static ConsoleKey Key = default;
        private static ConsoleKey LastKey = default;
        private static bool GameOver = false;
        private static (int Left, int Top) Position = (1, 1);
        private static Queue<(int, int)> Coords = new();
        private static Random Random = new Random();
        private static (int Left, int Top) AppleCoords = new();
        private static int SnakeLength = 1;
        private static bool IsOppositeKey = false;
        private static int Score = 0;

        private static Thread Game = new Thread(new ThreadStart(Move));

        static void Main(string[] args)
        {
            Console.SetWindowPosition(0, 0);
            Console.SetWindowSize(GameSettings.MapSize, GameSettings.MapSize);
            Console.SetBufferSize(GameSettings.MapSize, GameSettings.MapSize);
            Console.CursorVisible = false;

            RenderWall();

            RenderPosition();

            RenderApple();

            while (!GameOver)
            {
                var keyInfo = Console.ReadKey();

                if (!IsOppositeKey)
                    LastKey = Key;

                Key = keyInfo.Key;

                if (!Game.IsAlive && Game.ThreadState != ThreadState.Stopped)
                {
                    Game.Start();
                }
            }

            Coords.Clear();

            Console.Clear();     
            Console.SetCursorPosition(0, 0);

            Writer.PrintLine("game", ConsoleColor.Red);
            Writer.PrintLine("over!", ConsoleColor.Red);
            Writer.Print("score:" + Score, ConsoleColor.Cyan);


            Console.ReadKey(true);
        }


        private static void ClearPixel((int Left, int Top) currentPosition)
        {
            for (int i = 0; i < GameSettings.PixelSize; i++)
            {
                Console.SetCursorPosition(currentPosition.Left * GameSettings.PixelSize,
                                          currentPosition.Top * GameSettings.PixelSize + i);

                Console.Write(new string(' ', GameSettings.PixelSize));
            }
        }


        private static void RenderPosition()
        {
            if (Coords.Count == SnakeLength)
            {
                ClearPixel(Coords.Dequeue());
            }

            var worm = new Pixel(Position.Left, Position.Top, '#');

            worm.Draw(ConsoleColor.Blue);
        }


        private static void Move()
        {
            while (true)
            {
                Coords.Enqueue(Position);

                switch (Key)
                {
                    case ConsoleKey.UpArrow:
                        if (LastKey == ConsoleKey.DownArrow)
                        {
                            ++Position.Top;
                            IsOppositeKey = true;
                        }
                        else
                        {
                            --Position.Top;
                            IsOppositeKey = false;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (LastKey == ConsoleKey.UpArrow)
                        {
                            --Position.Top;
                            IsOppositeKey = true;
                        }
                        else
                        {
                            ++Position.Top;
                            IsOppositeKey = false;
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                        if (LastKey == ConsoleKey.RightArrow)
                        {
                            ++Position.Left;
                            IsOppositeKey = true;
                        }
                        else
                        {
                            --Position.Left;
                            IsOppositeKey = false;
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        if (LastKey == ConsoleKey.LeftArrow)
                        {
                            --Position.Left;
                            IsOppositeKey = true;
                        }
                        else
                        {
                            ++Position.Left;
                            IsOppositeKey = false;
                        }
                        break;
                }

                if (!IsValidPosition() || IsSnakeTale())
                    break;

                if (IsApplePosition())
                {
                    AppleCoords = default;

                    Coords.Enqueue(Position);

                    SnakeLength++;
                    Score += 10;

                    RenderApple();
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
            return AppleCoords.Left == Position.Left && AppleCoords.Top == Position.Top;
        }


        private static bool IsSnakeTale()
        {
            return Coords.Any(i => i.Item1 == Position.Left && i.Item2 == Position.Top);
        }


        private static (int, int) GenerateRandomAppleCoords()
        {
            do
            {
                int left = Random.Next(3, GameSettings.MapSizeInPixels - 1);
                int top = Random.Next(3, GameSettings.MapSizeInPixels - 1);

                AppleCoords = (left, top);
            }
            while (Coords.Any(i => i.Item1 == AppleCoords.Left && i.Item2 == AppleCoords.Top));

            return AppleCoords;
        }


        private static void RenderApple()
        {
            char wallSymbol = '@';

            var coords = GenerateRandomAppleCoords();

            var pixel = new Pixel(coords.Item1, coords.Item2, wallSymbol);

            pixel.Draw(ConsoleColor.Green);
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

                Thread.Sleep(1);
            }

            for (int i = 1; i < GameSettings.MapSizeInPixels; i++)
            {
                var pixel_1 = new Pixel(i, 0, wallSymbol);
                var pixel_2 = new Pixel(i, GameSettings.MapSizeInPixels, wallSymbol);

                pixel_1.Draw();
                pixel_2.Draw();

                Thread.Sleep(1);
            }
        }
    }
}
