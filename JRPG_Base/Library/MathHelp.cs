using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG_Base.Library
{
    public static class MathHelp
    {
        public static double distance(Vector2 From, Vector2 To)
        {
            return Math.Sqrt(Math.Pow(From.X - To.X, 2) + Math.Pow(From.Y - To.Y, 2));
        }
    }
}
