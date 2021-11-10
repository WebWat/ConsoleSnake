namespace ConsoleSnake
{
    public class GameSettings
    {
        public const int MapSize = 35 * PixelSize;
        public const int Speed = 150;
        public const int PixelSize = 3;
        public const int MapSizeInPixels = (MapSize / PixelSize) - PixelSize;
    }
}
