using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG_Base.Library
{
    public static class WorldHelp
    {
        public static Vector2 correctWorldOverflow(Vector2 inputVector)
        {
            if (inputVector.X < 0) inputVector.X += GSS.world.mapWidth;
            else if (inputVector.X >= GSS.world.mapWidth) inputVector.X -= GSS.world.mapWidth;
            if (inputVector.Y < 0) inputVector.Y += GSS.world.mapHeight;
            else if (inputVector.Y >= GSS.world.mapHeight) inputVector.Y -= GSS.world.mapHeight;
            return inputVector;
        }
    }
}
