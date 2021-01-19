using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DreamsEnd.Textures
{
    public interface IButton: IDisposable
    {
        Texture2D Active();
        Texture2D Inactive();
        Texture2D GetTexture(bool isActive);
        Rectangle Rect { get; }
        int Height { get; }
        int Width { get; }
    }
}
