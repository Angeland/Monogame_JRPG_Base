using System;

namespace DreamsEnd.Textures
{
    public interface ITile : IDisposable
    {
        string TexturePath { get; }
    }
}
