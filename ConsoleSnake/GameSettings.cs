namespace ConsoleSnake
{
    public class GameSettings
    {
        public const int MapSize = 20 * PixelSize;
        public const int Speed = 100;
        public const int PixelSize = 5;
        public const int MapSizeInPixels = (MapSize / PixelSize) - PixelSize;
    }
}
