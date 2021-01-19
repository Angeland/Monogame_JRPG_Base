using Microsoft.Xna.Framework;
using RPG.States.Configuration;

namespace RPG.Library
{
    public static class CameraHelp
    {
        public static bool WithinCamera(Vector2 cameraPos, Vector2 objectPosition, Vector2 objectSize)
        {
            //Checks is out of bound
            if (objectPosition.X + objectSize.X > cameraPos.X && objectPosition.Y + objectSize.Y > cameraPos.Y)
                if (objectPosition.X - objectSize.X < cameraPos.X + DisplayOutputSettings.ScreenWidth && objectPosition.Y - objectSize.Y < cameraPos.Y + DisplayOutputSettings.ScreenHeight)
                    return true;
            return false;
        }

    }
}
