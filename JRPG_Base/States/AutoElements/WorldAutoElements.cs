using JRPG_Base.States.Area.Cities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG_Base.States.AutoElements
{
    public class WorldAutoElements
    {
        List<Ship> Ships = new List<Ship>();
        public void Init(Texture2D smallBoat)
        {
            City Newton = new City(new Vector2(170, 195));
            City Shackville = new City(new Vector2(540, 155));
            City Island = new City(new Vector2(140, 1135));

            //Ship 1
            Cargo Newton_Shackville_Cargo = new Cargo(50);
            Ship Newton_Shackville = new Ship(new TimeSpan(1, 30, 0), smallBoat, Newton_Shackville_Cargo, Shackville.getHarbor(), Newton.getHarbor(), 6);
            Ships.Add(Newton_Shackville);

            //Ship 2
            Cargo Shackville_Cargo = new Cargo(30);
            Ship Newton_Shackville2 = new Ship(new TimeSpan(1, 30, 0), smallBoat, Shackville_Cargo, Newton.getHarbor(), Island.getHarbor(), 6);
            Ships.Add(Newton_Shackville2);
        }
        public void BuildPreRequisites()
        {
            Ships.ForEach(a => a.BuildPreRequisites());
        }
        public void Update(GameTime gametime)
        {
            Ships.ForEach(a => a.Update(gametime));

            
            //(Debug)Follow Ship (Bug found when crossing edge)
            GSS.ExtraOutput = String.Format("Ship X:{0} Y:{1} Camera X:{2} Y:{3}", Ships[1].getPos().X, Ships[1].getPos().Y, GSS.world.worldCharacter.CameraPostition.X, GSS.world.worldCharacter.CameraPostition.Y);
            GSS.world.worldCharacter.CameraPostition = (Ships[1].getPos()/GSS.TileSize)- (new Vector2(GSS.world.mapWidth/2,GSS.world.mapHeight/2)/GSS.TileSize);
            
        }
        public void Draw()
        {
            if (GSS.DebugMode)
            {
                GSS.spriteBatch.Draw(GSS.world.minimapTex,
                     new Rectangle(0, 0, GSS.ScreenWidth, GSS.ScreenHeight), Color.White);
            }
            Ships.ForEach(a => a.Draw());
        }
    }
}
