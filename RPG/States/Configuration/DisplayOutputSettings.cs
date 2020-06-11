using Microsoft.Xna.Framework.Graphics;

namespace RPG.States.Configuration
{
    public static class DisplayOutputSettings
    {
        internal static bool isFullScreen = false   ;


        private static DisplayMode displayMode;
        internal static DisplayMode DisplayMode
        {
            get { return displayMode; }
            set
            {
                displayMode = value;
                ScreenHeight = value.Height / 2;
                ScreenWidth = value.Width / 2;
            }
        }

        public static int CenterScreenTileOffsetX, CenterScreenTileOffsetY;

        private static int _screenHeight = 640;
        public static int ScreenHeight
        {
            get
            {
                return _screenHeight;
            }
            private set
            {
                _screenHeight = value;
                CenterScreenTileOffsetY = (int)((value / 2560f) * (2 * EngineSettings.TileSize));
            }
        }

        private static int _screenWidth = 480;
        public static int ScreenWidth
        {
            get
            {
                return _screenWidth;
            }
            private set
            {
                _screenWidth = value;
                CenterScreenTileOffsetX = (int)((value / 1440f) * (2 * EngineSettings.TileSize));
            }
        }
    }
}
