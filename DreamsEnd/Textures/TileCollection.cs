using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DreamsEnd.Textures
{
    public class TileCollection : TextureCache, ITilesCollection
    {
        public ITile[] Tiles { get; private set; }

        public int Length => Tiles.Length;

        public Vector2 Size { get; }

        public TileCollection(ContentManager content, ITile[] tiles, Vector2 size) : base(content)
        {
            Tiles = tiles;
            Size = size;
        }

        public TileCollection(ContentManager content, ITile[] tiles) :
            this(content, tiles, GetSize(content, tiles[0]))
        {
        }

        private static Vector2 GetSize(ContentManager content, ITile tile)
        {
            var tex = content.Load<Texture2D>(tile.TexturePath);
            return new Vector2(tex.Width, tex.Height);
        }

        public Texture2D GetTile(int index = 0)
        {
            return GetTexture(Tiles[index].TexturePath);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach(ITile tile in Tiles)
                {
                    tile.Dispose();
                }
                Tiles = null;
            }
            base.Dispose(disposing);
        }
    }
}
