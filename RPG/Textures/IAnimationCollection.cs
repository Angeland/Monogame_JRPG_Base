using Microsoft.Xna.Framework.Graphics;
namespace RPG.Textures
{
    public interface IAnimationCollection
    {
        Texture2D GetTexture(int keyFrame);
    }
}
