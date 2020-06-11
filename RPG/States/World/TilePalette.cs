using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace RPG.States.World
{
    public static class TilePalette
    {
        public static readonly Dictionary<Color, int> TileColor = new Dictionary<Color, int>();
        private static readonly Dictionary<Color, bool> WalkableColor = new Dictionary<Color, bool>();
        private static readonly Dictionary<Color, bool> SailableColor = new Dictionary<Color, bool>();


        public static bool IsAble(int tileX, int tileY, Dictionary<Color, bool> able)
        {
            if (tileX >= WorldInformation.mapWidth) tileX -= WorldInformation.mapWidth;
            else if (tileX < 0) tileX += WorldInformation.mapWidth;
            if (tileY >= WorldInformation.mapHeight) tileY -= WorldInformation.mapHeight;
            else if (tileY < 0) tileY += WorldInformation.mapHeight;
            return able.ContainsKey(WorldInformation.worldMapCheck[tileX + (tileY * WorldInformation.mapWidth)]);
        }
        public static bool IsWalkable(int tileX, int tileY)
        {
            return IsAble(tileX, tileY, WalkableColor) &&
                IsAble(tileX + 1, tileY, WalkableColor) &&
                IsAble(tileX, tileY + 1, WalkableColor) &&
                IsAble(tileX + 1, tileY + 1, WalkableColor);
        }
        public static bool IsTileSailable(int tileX, int tileY, Vector2 size)
        {
            return IsAble(tileX, tileY, SailableColor) &&
                IsAble(tileX + (int)size.X, tileY, SailableColor) &&
                IsAble(tileX, tileY + (int)size.Y, SailableColor) &&
                IsAble(tileX + (int)size.X, tileY + (int)size.Y, SailableColor);
        }

        static TilePalette()
        {
            TileColor.Add(new Color(34, 177, 76), 0);//Grass
            WalkableColor.Add(new Color(34, 177, 76), true);//Grass

            TileColor.Add(new Color(239, 228, 176), 1);//Sand/Desert
            WalkableColor.Add(new Color(239, 228, 176), true);//Sand/Desert

            TileColor.Add(new Color(153, 217, 234), 2);//Beach Sea
            WalkableColor.Add(new Color(153, 217, 234), true);//Beach Sea
            //_sailAble.Add(new Color(153, 217, 234), true);//Beach Sea

            TileColor.Add(new Color(0, 162, 232), 3);//Shallow Sea
            SailableColor.Add(new Color(0, 162, 232), true);//Shallow Sea

            TileColor.Add(new Color(63, 72, 204), 4);//Deep Sea
            SailableColor.Add(new Color(63, 72, 204), true);//Deep Sea

            TileColor.Add(new Color(195, 195, 195), 5);//City ground
            WalkableColor.Add(new Color(195, 195, 195), true);//City ground

            TileColor.Add(new Color(127, 127, 127), 6);//City Wall

            TileColor.Add(new Color(185, 122, 87), 7);//City Port
            WalkableColor.Add(new Color(185, 122, 87), true);//City Port

            TileColor.Add(new Color(201, 151, 124), 8);//House Roof Up

            TileColor.Add(new Color(206, 175, 119), 9);//House Roof Down

            TileColor.Add(new Color(216, 179, 158), 10);//House Roof Right

            TileColor.Add(new Color(218, 188, 156), 11);//House Roof Left   

            TileColor.Add(new Color(170, 105, 70), 12);//House Roof Angle           

            TileColor.Add(new Color(174, 0, 27), 13);//Ridge Roof Vertical

            TileColor.Add(new Color(136, 0, 21), 14);//Ridge Roof Horizontal

            TileColor.Add(new Color(91, 0, 14), 15);//Ridge Roof Angle

        }
    }
}
