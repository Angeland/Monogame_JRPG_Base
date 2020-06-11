using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RPG.States.Configuration;

namespace RPG.Textures
{
    public class Button : TextureCache, IButton
    {
        private readonly string activePath;
        private readonly string inactivePath;
        public Rectangle Rect { get; }

        public int Height => Rect != null ? Rect.Height : Active().Height;
        public int Width => Rect != null ? Rect.Width : Active().Width;

        public Button(ContentManager content, string inactivePath, string activePath, float pstXoffset = 0, float pstYoffset = 0) :
            this(content, activePath, inactivePath, DefaultRect(content, activePath, pstXoffset, pstYoffset))
        {
        }

        public Button(ContentManager content, string inactivePath, string activePath, Rectangle size)
            : base(content)
        {
            this.activePath = activePath;
            this.inactivePath = inactivePath;
            Rect = size;
        }

        public Texture2D Active()
        {
            return GetTexture(activePath);
        }

        public Texture2D Inactive()
        {
            return GetTexture(inactivePath);
        }

        public Texture2D GetTexture(bool isActive)
        {
            if (isActive)
            {
                return Active();
            }
            return Inactive();
        }

        private static Rectangle DefaultRect(ContentManager content, string baseTexturePath, float pstXoffset = 0, float pstYoffset = 0)
        {
            var tex = content.Load<Texture2D>(baseTexturePath);
            return new Rectangle(
                        (int)((DisplayOutputSettings.ScreenWidth / 2f) - (int)(tex.Width / 2f) + (tex.Width * pstXoffset)),
                        (int)((DisplayOutputSettings.ScreenHeight / 2f) - (int)((tex.Height + 10) * 2 / 2f) + (tex.Height * pstYoffset)),
                        tex.Width, tex.Height);
        }
    }
}
