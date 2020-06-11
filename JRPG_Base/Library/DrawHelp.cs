using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG_Base.Library
{
    public static class DrawHelp
    {
        public static Rectangle getShape(Vector2 cameraPos, Vector2 objectPosition, Vector2 objectSize)
        {
            return new Rectangle((int)(objectPosition.X - cameraPos.X), (int)(objectPosition.Y - cameraPos.Y), (int)objectSize.X, (int)objectSize.Y);
        }
    }
}
