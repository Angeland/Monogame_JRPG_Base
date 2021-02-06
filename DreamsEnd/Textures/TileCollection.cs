using DreamsEnd.States.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace DreamsEnd.Textures
{
    public class TileCollection : TextureCache, ITilesCollection
    {
        public ITile[] Tiles { get; private set; }
        public Dictionary<Color, TileProperties> TileProperties { get; private set; }
        public Dictionary<Color, ITile> TilesMap { get; private set; }

        public int Length => Tiles.Length;

        public Vector2 Size { get; }

        public TileCollection(ContentManager content, ITile[] tiles, Vector2 size) : base(content)
        {
            Tiles = tiles;
            Size = size;
            TileProperties = new Dictionary<Color, TileProperties>();
            foreach (ITile tile in Tiles)
            {
                if (tile.TileProperties != null)
                {
                    TileProperties.Add(tile.TileProperties.tileColor, tile.TileProperties);
                }
            }
            TilesMap = new Dictionary<Color, ITile>();
            foreach (ITile tile in Tiles)
            {
                if (tile.TileProperties != null)
                {
                    TilesMap.Add(tile.TileProperties.tileColor, tile);
                }
            }
        }

        public TileCollection(ContentManager content, ITile[] tiles) :
            this(content, tiles, GetSize(content, tiles[0]))
        {
        }

        public TileProperties IsAble(int tileX, int tileY)
        {
            if (tileX >= WorldInformation.mapWidth) tileX -= WorldInformation.mapWidth;
            else if (tileX < 0) tileX += WorldInformation.mapWidth;
            if (tileY >= WorldInformation.mapHeight) tileY -= WorldInformation.mapHeight;
            else if (tileY < 0) tileY += WorldInformation.mapHeight;
            var t = WorldInformation.worldMapCheck[tileX + (tileY * WorldInformation.mapWidth)];
            if (TileProperties.ContainsKey(t))
            {
                return TileProperties[t];
            }
            return new TileProperties();
        }
        public bool IsWalkable(int tileX, int tileY)
        {
            return IsAble(tileX, tileY).Walkable &&
                IsAble(tileX + 1, tileY).Walkable &&
                IsAble(tileX, tileY + 1).Walkable &&
                IsAble(tileX + 1, tileY + 1).Walkable;
        }
        public bool IsTileSailable(int tileX, int tileY, Vector2 size)
        {
            return IsAble(tileX, tileY).Sailable &&
                IsAble(tileX + (int)size.X, tileY).Sailable &&
                IsAble(tileX, tileY + (int)size.Y).Sailable &&
                IsAble(tileX + (int)size.X, tileY + (int)size.Y).Sailable;
        }

        private static Vector2 GetSize(ContentManager content, ITile tile)
        {
            var tex = content.Load<Texture2D>(tile.TexturePath);
            return new Vector2(tex.Width, tex.Height);
        }

        public Texture2D GetTile(int index)
        {
            return GetTexture(Tiles[index].TexturePath);
        }

        public Texture2D GetTile(Color tileColor)
        {
            return GetTexture(TilesMap[tileColor].TexturePath);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Tiles = null;
            }
            base.Dispose(disposing);
        }

        public bool ContainsTile(Color tileColor)
        {
            return TileProperties.ContainsKey(tileColor);
        }
    }
}
