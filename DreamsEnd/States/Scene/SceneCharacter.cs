using Microsoft.Xna.Framework;

namespace RPG.States.Characters.Individuals
{
    public class SceneCharacter : Character
    {
        private Vector2 tilePosition = Vector2.Zero;

        public SceneCharacter(string name)
        {
        }

        internal Vector2 GetTilePosition()
        {
            return tilePosition;
        }
    }
}
