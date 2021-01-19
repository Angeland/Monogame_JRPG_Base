using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DreamsEnd.Library
{
    public class Texture2DColors
    {
        public Color[] Colors { get; private set; }
        public Texture2D Texture { get; private set; }
        public int Height => Texture.Height;
        public int Width => Texture.Width;

        public Texture2DColors(Texture2D texture)
        {
            Texture = texture;
            Colors = Texture2DToColorArray();
        }

        private Color[] Texture2DToColorArray()
        {
            Color[] r = new Color[Height * Width];
            Texture.GetData(r);
            return r;
        }

        public Color GetColor(int x, int y)
        {
            if (y < 0)
            {
                y = -y;
            }
            if (x < 0)
            {
                x = -x;
            }
            int indx = (y * Width) + x;
            return Colors[indx];

        }

        public Color GetColor(int indx)
        {
            return Colors[indx];
        }
    }
}
