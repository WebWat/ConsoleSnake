namespace ConsoleSnake;

public class GameSettings
{
    public const int MapSize = 25 * PixelSize;
    public const int Speed = 100;
    public const int PixelSize = 2;
    public const int MapSizeInPixels = (MapSize / PixelSize) - PixelSize;
}
