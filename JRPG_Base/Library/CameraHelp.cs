using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG_Base.Library
{
    public static class CameraHelp
    {
        public static bool WithinCamera(Vector2 cameraPos, Vector2 objectPosition, Vector2 objectSize)
        {
            //Checks is out of bound
            if (objectPosition.X + objectSize.X > cameraPos.X && objectPosition.Y + objectSize.Y > cameraPos.Y)
                if (objectPosition.X - objectSize.X < cameraPos.X + GSS.ScreenWidth && objectPosition.Y - objectSize.Y < cameraPos.Y + GSS.ScreenHeight)
                    return true;
            return false;
        }

    }
}
