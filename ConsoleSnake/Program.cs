﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// ... only supported on: 'windows'
#pragma warning disable CA1416
namespace ConsoleSnake;

class Program
{
    private static ConsoleKey Key = default;
    private static (int Left, int Top) Position = (1, 1);
    private static Queue<(int, int)> Coords = new();
    private static Random Random = new Random();
    private static (int Left, int Top) AppleCoords = new();
    private static int SnakeLength = 1;
    private static int Score = 0;

    static void Main(string[] args)
    {
        Console.SetWindowPosition(0, 0);
        Console.SetWindowSize(GameSettings.MapSize, GameSettings.MapSize);
        Console.SetBufferSize(GameSettings.MapSize, GameSettings.MapSize);
        Console.CursorVisible = false;

        RenderWall();

        RenderPosition();

        RenderApple();

        Key = Console.ReadKey(true).Key;

        StartGame();

        Console.Clear();
        Console.SetCursorPosition(0, 0);

        Writer.PrintLine("game", ConsoleColor.Red);
        Writer.PrintLine("over!", ConsoleColor.Red);
        Writer.Print("score:" + Score, ConsoleColor.Cyan);

        Console.ReadLine();
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


    private static void StartGame()
    {
        while (true)
        {
            Coords.Enqueue(Position);

            Key = GetDirectionKey();

            switch (Key)
            {
                case ConsoleKey.UpArrow:
                    --Position.Top;
                    break;

                case ConsoleKey.DownArrow:      
                    ++Position.Top;                 
                    break;

                case ConsoleKey.LeftArrow:            
                    --Position.Left;                    
                    break;

                case ConsoleKey.RightArrow:
                    ++Position.Left;                    
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

                Task.Run(() => Console.Beep(500, 200));

                RenderApple();
            }

            RenderPosition();

            Thread.Sleep(GameSettings.Speed);
        }

        Coords.Clear();

        Task.Run(() => Console.Beep(170, 800));
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

    private static ConsoleKey GetDirectionKey()
    {
        if (!Console.KeyAvailable)
        {
            return Key;
        }
        
        var key = Console.ReadKey(true).Key;

        return key switch
        {
            ConsoleKey.UpArrow when Key != ConsoleKey.DownArrow => ConsoleKey.UpArrow,
            ConsoleKey.DownArrow when Key != ConsoleKey.UpArrow => ConsoleKey.DownArrow,
            ConsoleKey.LeftArrow when Key != ConsoleKey.RightArrow => ConsoleKey.LeftArrow,
            ConsoleKey.RightArrow when Key != ConsoleKey.LeftArrow => ConsoleKey.RightArrow,
            _ => Key
        };
    }
}
#pragma warning restore CA1416