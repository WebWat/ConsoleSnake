using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleSnake
{
    class Program
    {
        private const int ScreenWidth = 100; 
        private const int ScreenHeight = 100 + 2;
        private const int Speed = 10;

        private static ConsoleKey Key = default;
        private static bool GameOver = false;
        private static (int X, int Y) Position = (0 + 1, 0 + 1);
        private static char[] Map = new char[ScreenWidth * ScreenHeight];

        private static Thread Game = new Thread(new ThreadStart(Move));
        private static Thread Launcher = new Thread(new ThreadStart(Launch));

        static void Main(string[] args)
        {
            Console.SetWindowPosition(0, 0);
            Console.SetWindowSize(ScreenWidth, ScreenHeight);
            Console.SetBufferSize(ScreenWidth, ScreenHeight);
            Console.CursorVisible = false;

            Render();

            Launcher.Start();

            while (!GameOver)
            {
                Thread.Sleep(5);
            }

            Launcher.Interrupt();

            Thread.Sleep(500);

            Console.Clear();
            
            Console.WriteLine("Game over!");

            Console.ReadKey(true);
        }


        private static void ClearMap() => Array.Fill(Map, ' ');


        private static void Launch()
        {
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

        private static void Render()
        {
            Console.SetCursorPosition(0, 0);

            ClearMap();

            SetWall();

            SetPosition();

            Console.WriteLine(Map);
        }


        private static void SetPosition()
        {
            int positionIndex = Map.Length - ScreenWidth * (Position.Y + 1) + Position.X;

            Map[positionIndex] = '#';
        }


        private static void Move()
        {
            while (true)
            {
                switch (Key)
                {
                    case ConsoleKey.UpArrow:
                        Position.Y++;
                        break;

                    case ConsoleKey.DownArrow:
                        Position.Y--;
                        break;

                    case ConsoleKey.LeftArrow:
                        Position.X--;
                        break;

                    case ConsoleKey.RightArrow:
                        Position.X++;
                        break;
                }

                if (!IsValidPosition())
                    break;

                Render();

                Thread.Sleep(Speed);

            }

            GameOver = true;
        }


        private static bool IsValidPosition()
        {
            return (Position.Y < ScreenHeight - 3 && Position.Y >= 1) &&
                   (Position.X < ScreenWidth - 1 && Position.X >= 1);
        }


        private static void SetWall()
        {
            char c = '+';

            Array.Fill(Map, c, ScreenWidth * 2, ScreenWidth);
            Array.Fill(Map, c, Map.Length - ScreenWidth, ScreenWidth);

            for (int i = ScreenWidth * 2; i < ScreenWidth * ScreenHeight; i += ScreenWidth)
            {
                Map[i - 1] = c;
                Map[i - ScreenWidth] = c;
            }
        }
    }
}
