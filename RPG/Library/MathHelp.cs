using Microsoft.Xna.Framework;
using System;

namespace RPG.Library
{
    public static class MathHelp
    {
        public static double Distance(Vector2 From, Vector2 To)
        {
            return Math.Sqrt(Math.Pow(From.X - To.X, 2) + Math.Pow(From.Y - To.Y, 2));
        }
    }
}
