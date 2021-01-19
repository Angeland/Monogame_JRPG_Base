using Microsoft.Xna.Framework;

namespace DreamsEnd.States.World
{
    public static class WorldInformation
    {
        internal static Color[] worldMapCheck;
        internal static int mapWidth;
        internal static int mapHeight;
        internal static Vector2 MapSize => new Vector2(mapWidth, mapHeight);
    }
}
