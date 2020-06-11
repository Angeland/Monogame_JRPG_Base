using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RPG.Textures
{
    public interface ITilesCollection: IDisposable
    {
        Texture2D GetTile(int index = 0);
        int Length { get; }
        Vector2 Size { get; }
    }
}
