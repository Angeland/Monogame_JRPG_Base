using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace DreamsEnd.Textures
{
    public interface ITilesCollection: IDisposable
    {
        Texture2D GetTexture(Color tileColor);
        Texture2D GetTexture(int index = 0);
        Texture2D GetSubTexture(Color tileColor);
        bool HasSubTexture(Color tileColor);
        int Length { get; }
        Vector2 Size { get; }

        bool ContainsTile(Color tileColor);
        bool IsWalkable(int tileX, int tileY);
        bool IsTileSailable(int tileX, int tileY, Vector2 size);
    }
}
