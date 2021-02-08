
using Microsoft.Xna.Framework;
namespace DreamsEnd.Textures
{
    public class TileProperties
    {
        public Color TileColor { get; set; }
        public bool Walkable { get; set; } = false;
        public bool Sailable { get; set; } = false;
        public string SubTexturePath { get; internal set; }
    }
}
