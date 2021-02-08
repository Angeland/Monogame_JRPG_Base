using Microsoft.Xna.Framework.Graphics;

namespace DreamsEnd.States.Configuration
{
    public static class DisplayOutputSettings
    {
        const int BaseHeigth = 1200;
        const int BaseWidth = 1920;
        internal static bool isFullScreen = false;

        private static DisplayMode displayMode;
        internal static DisplayMode DisplayMode
        {
            get { return displayMode; }
            set
            {
                displayMode = value;
                ScreenHeight = isFullScreen ? value.Height : BaseHeigth;
                ScreenWidth = isFullScreen ? value.Width : BaseWidth;
            }
        }

        public static int CenterScreenTileOffsetX, CenterScreenTileOffsetY;

        private static int _screenHeight = DisplayMode?.Height ?? BaseHeigth;
        public static int ScreenHeight
        {
            get
            {
                return _screenHeight;
            }
            private set
            {
                _screenHeight = value;
                CenterScreenTileOffsetY = (int)((value / (double)DisplayMode.Width) * (2 * EngineSettings.TileSize));
            }
        }

        private static int _screenWidth = DisplayMode?.Width ?? BaseWidth;
        public static int ScreenWidth
        {
            get
            {
                return _screenWidth;
            }
            private set
            {
                _screenWidth = value;
                CenterScreenTileOffsetX = (int)((value / (double)DisplayMode.Height) * (2 * EngineSettings.TileSize));
            }
        }
    }
}
