using System;

namespace DreamsEnd.Textures
{
    public class Tile : ITile
    {
        public string TexturePath { get; }
        public TileProperties TileProperties { get; }

        public Tile(string texturePath)
        {
            TexturePath = texturePath;
        }
        public Tile(string texturePath, TileProperties tileProperties) : this(texturePath)
        {
            TileProperties = tileProperties;
        }
    }
}
