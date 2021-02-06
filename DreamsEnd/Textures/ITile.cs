using System;

namespace DreamsEnd.Textures
{
    public interface ITile
    {
        string TexturePath { get; }
        TileProperties TileProperties{ get; }
    }
}
