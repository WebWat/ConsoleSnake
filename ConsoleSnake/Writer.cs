using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleSnake;
public static class Writer
{
    public static Dictionary<char, int[,]> Characters = new Dictionary<char, int[,]>()
    {
        ['g'] = new int[,] { { 1, 1, 1 }, { 1, 0, 0 }, { 1, 0, 0 }, { 1, 0, 1 }, { 1, 1, 1 } },
        ['a'] = new int[,] { { 0, 1, 0 }, { 1, 0, 1 }, { 1, 1, 1 }, { 1, 0, 1 }, { 1, 0, 1 } },
        ['m'] = new int[,] { { 1, 0, 1 }, { 1, 1, 1 }, { 1, 0, 1 }, { 1, 0, 1 }, { 1, 0, 1 } },
        ['e'] = new int[,] { { 1, 1, 1 }, { 1, 0, 0 }, { 1, 1, 1 }, { 1, 0, 0 }, { 1, 1, 1 } },
        ['o'] = new int[,] { { 1, 1, 1 }, { 1, 0, 1 }, { 1, 0, 1 }, { 1, 0, 1 }, { 1, 1, 1 } },
        ['v'] = new int[,] { { 1, 0, 1 }, { 1, 0, 1 }, { 1, 0, 1 }, { 1, 0, 1 }, { 0, 1, 0 } },
        ['r'] = new int[,] { { 1, 1, 1 }, { 1, 0, 1 }, { 1, 1, 0 }, { 1, 0, 1 }, { 1, 0, 1 } },
        ['s'] = new int[,] { { 1, 1, 1 }, { 1, 0, 0 }, { 1, 1, 1 }, { 0, 0, 1 }, { 1, 1, 1 } },
        ['c'] = new int[,] { { 1, 1, 1 }, { 1, 0, 0 }, { 1, 0, 0 }, { 1, 0, 0 }, { 1, 1, 1 } },
        [':'] = new int[,] { { 0, 1, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 1, 0 } },
        ['!'] = new int[,] { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 }, { 0, 0, 0 }, { 0, 1, 0 } },
        [' '] = new int[,] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } },
        ['0'] = new int[,] { { 0, 1, 0 }, { 1, 0, 1 }, { 1, 0, 1 }, { 1, 0, 1 }, { 0, 1, 0 } },
        ['1'] = new int[,] { { 0, 0, 1 }, { 0, 1, 1 }, { 1, 0, 1 }, { 0, 0, 1 }, { 0, 0, 1 } },
        ['2'] = new int[,] { { 1, 1, 1 }, { 0, 0, 1 }, { 1, 1, 1 }, { 1, 0, 0 }, { 1, 1, 1 } },
        ['3'] = new int[,] { { 1, 1, 1 }, { 0, 0, 1 }, { 1, 1, 1 }, { 0, 0, 1 }, { 1, 1, 1 } },
        ['4'] = new int[,] { { 1, 0, 1 }, { 1, 0, 1 }, { 1, 1, 1 }, { 0, 0, 1 }, { 0, 0, 1 } },
        ['5'] = new int[,] { { 1, 1, 1 }, { 1, 0, 0 }, { 1, 1, 1 }, { 0, 0, 1 }, { 1, 1, 1 } },
        ['6'] = new int[,] { { 1, 1, 1 }, { 1, 0, 0 }, { 1, 1, 1 }, { 1, 0, 1 }, { 1, 1, 1 } },
        ['7'] = new int[,] { { 1, 1, 1 }, { 0, 0, 1 }, { 0, 0, 1 }, { 0, 0, 1 }, { 0, 0, 1 } },
        ['8'] = new int[,] { { 1, 1, 1 }, { 1, 0, 1 }, { 1, 1, 1 }, { 1, 0, 1 }, { 1, 1, 1 } },
        ['9'] = new int[,] { { 1, 1, 1 }, { 1, 0, 1 }, { 1, 1, 1 }, { 0, 0, 1 }, { 1, 1, 1 } },
    };


    private static void WriteSymbols(string text, ref int left, ref int top, ConsoleColor color)
    {
        text = text.ToLower();

        foreach (char c in text)
        {
            if (Characters.ContainsKey(c))
            {
                int counter = 0;
                foreach (var item in Characters[c])
                {
                    if (item != 0)
                    {
                        var pixel = new Pixel(left, top, '*');
                        pixel.Draw(color);
                        Thread.Sleep(1);
                    }

                    left++;
                    counter++;

                    if (counter == 3)
                    {
                        left -= 3;
                        counter -= 3;
                        top++;
                    }
                }

                left += 4;
                top -= 5;

                if ((left + 3) * GameSettings.PixelSize >= GameSettings.MapSize)
                {
                    left = 0;
                    top += 6;
                }
            }
        }
    }


    public static void Print(string text, ConsoleColor color = ConsoleColor.White)
    {
        (int left, int top) = Console.GetCursorPosition();

        WriteSymbols(text, ref left, ref top, color);
    }


    public static void PrintLine(string text, ConsoleColor color = ConsoleColor.White)
    {
        (int left, int top) = Console.GetCursorPosition();

        WriteSymbols(text, ref left, ref top, color);

        Console.SetCursorPosition(0, top + 6);
    }
}
