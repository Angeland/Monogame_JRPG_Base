using System;

namespace RPG.Textures
{
    public class Tile : ITile
    {
        public string TexturePath { get; private set; }

        public Tile(string texturePath)
        {
            TexturePath = texturePath;
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
                TexturePath = null;
            }
        }
    }
}
