using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DreamsEnd.Textures
{
    public class AnimationCollection: TextureCache, IAnimationCollection
    {
        private readonly string[] texturePaths;
        public AnimationCollection(ContentManager content, string[] texturePaths):base(content)
        {
            this.texturePaths = texturePaths;
        }
        public Texture2D GetTexture(int keyFrame)
        {
            if (keyFrame < texturePaths.Length)
            {
                return GetTexture(texturePaths[keyFrame]);
            }
            return GetTexture(texturePaths[0]);
        }
    }
}
