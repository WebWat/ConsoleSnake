namespace ConsoleSnake
{
    public class GameSettings
    {
        public const int MapSize = 25 * PixelSize;
        public const int Speed = 70;
        public const int PixelSize = 3;
        public const int MapSizeInPixels = (MapSize / PixelSize) - PixelSize;
    }
}
