using Microsoft.Xna.Framework;

namespace RPG.Library
{
    public static class DrawHelp
    {
        public static Rectangle GetBoundary(Vector2 cameraPos, Vector2 objectPosition, Vector2 objectSize)
        {
            return new Rectangle(
                (int)(objectPosition.X - cameraPos.X),
                (int)(objectPosition.Y - cameraPos.Y),
                (int)objectSize.X,
                (int)objectSize.Y
                );
        }
    }
}
