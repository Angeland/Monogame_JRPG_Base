using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Textures
{
    public interface IWorldCharacter
    {
        Point WindowPosition { get; }
        Vector2 CameraPosition { get; set; }
        Vector2 GetPosition();
        Point PositionOffset { get; }
        Vector2 GetCenterTilePosition();
        void Update(GameTime gameTime);
        void Draw();
    }
}
