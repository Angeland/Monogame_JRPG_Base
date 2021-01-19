using Microsoft.Xna.Framework;

namespace DreamsEnd.States.Configuration
{
    public static class GraphicsSettings
    {
        public static GraphicsDeviceManager GraphicsDeviceManager { private get; set; }

        public static void SetFullscreen(bool isFullscreen)
        {
            GraphicsDeviceManager.IsFullScreen = isFullscreen;
        }

        public static void ApplyChanges()
        {
            GraphicsDeviceManager.ApplyChanges();
        }

        internal static void SetPreferredBackBuffer(int width, int height)
        {
            GraphicsDeviceManager.PreferredBackBufferWidth = width;
            GraphicsDeviceManager.PreferredBackBufferHeight = height;
        }
    }
}
