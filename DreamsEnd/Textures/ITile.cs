using System;

namespace RPG.Textures
{
    public interface ITile : IDisposable
    {
        string TexturePath { get; }
    }
}
