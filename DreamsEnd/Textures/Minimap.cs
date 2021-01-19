using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamsEnd.Textures
{
    public class Minimap : TextureCache
    {
        private readonly string cursor;
        private readonly string map;

        public Minimap(ContentManager content, string map, string cursor) : base(content)
        {
            this.cursor = cursor;
            this.map = map;
        }

        public Texture2D Map()
        {
            return GetTexture(map);
        }
        public Texture2D Cursor()
        {
            return GetTexture(cursor);
        }
    }
}
