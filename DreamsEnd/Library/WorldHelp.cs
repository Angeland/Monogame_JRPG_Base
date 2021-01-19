using Microsoft.Xna.Framework;
using DreamsEnd.States.World;

namespace DreamsEnd.Library
{
    public static class WorldHelp
    {
        public static Vector2 CorrectWorldOverflow(Vector2 inputVector)
        {
            if (inputVector.X < 0) inputVector.X += WorldInformation.mapWidth;
            else if (inputVector.X >= WorldInformation.mapWidth) inputVector.X -= WorldInformation.mapWidth;
            if (inputVector.Y < 0) inputVector.Y += WorldInformation.mapHeight;
            else if (inputVector.Y >= WorldInformation.mapHeight) inputVector.Y -= WorldInformation.mapHeight;
            return inputVector;
        }
    }
}
