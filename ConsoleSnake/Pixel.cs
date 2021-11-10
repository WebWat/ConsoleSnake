using System;

namespace ConsoleSnake
{
    public struct Pixel
    {
        public int Left { get; private set; }
        public int Top { get; private set; }
        public char Symbol { get; private set; }

        public Pixel(int left, int top, char symbol)
        {
            Left = left;
            Top = top;
            Symbol = symbol;
        }


        public void Draw(ConsoleColor color = ConsoleColor.White)
        {
            for (int i = 0; i < GameSettings.PixelSize; i++)
            {
                Console.SetCursorPosition(Left * GameSettings.PixelSize, Top * GameSettings.PixelSize + i);

                char[] output = new char[GameSettings.PixelSize];

                Array.Fill(output, Symbol);

                Console.ForegroundColor = color;
                Console.Write(output);
                Console.ForegroundColor = default;
            }
        }
    }
}
