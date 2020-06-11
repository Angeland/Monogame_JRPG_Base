using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace RPG.Textures
{
    public class TextureCache : IDisposable
    {
        private Dictionary<string, Texture2D> cachedTexture = new Dictionary<string, Texture2D>();
        protected readonly ContentManager Content;
        public TextureCache(ContentManager content)
        {
            Content = content;
        }

        protected Texture2D GetTexture(string texturePath)
        {
            if (!cachedTexture.ContainsKey(texturePath))
            {
                cachedTexture.Add(texturePath, Content.Load<Texture2D>(texturePath));
            }
            return cachedTexture[texturePath];
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (cachedTexture != null)
                {
                    cachedTexture = null;
                }
            }
        }
    }
}
