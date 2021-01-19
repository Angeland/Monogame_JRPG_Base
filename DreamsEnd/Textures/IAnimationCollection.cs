using Microsoft.Xna.Framework.Graphics;
namespace DreamsEnd.Textures
{
    public interface IAnimationCollection
    {
        Texture2D GetTexture(int keyFrame);
    }
}
