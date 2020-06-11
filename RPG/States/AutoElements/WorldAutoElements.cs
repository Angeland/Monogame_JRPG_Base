using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RPG.States.Area.Cities;
using RPG.States.Configuration;
using RPG.States.DebugEnums;
using RPG.States.DebugHelp;
using RPG.States.World;
using System;
using System.Collections.Generic;

namespace RPG.States.AutoElements
{
    public class WorldAutoElements
    {
        readonly List<Ship> Ships = new List<Ship>();
        public void Init(Texture2D smallBoat)
        {
            City Newton = new City(new Vector2(170, 195));
            City Shackville = new City(new Vector2(540, 155));
            City Island = new City(new Vector2(140, 1135));

            //Ship 1
            Cargo Newton_Shackville_Cargo = new Cargo(50);
            Ship Newton_Shackville = new Ship(new TimeSpan(1, 30, 0), smallBoat, Newton_Shackville_Cargo, Shackville.GetHarbor(), Newton.GetHarbor(), 6);
            Ships.Add(Newton_Shackville);

            //Ship 2
            Cargo Shackville_Cargo = new Cargo(30);
            Ship Newton_Shackville2 = new Ship(new TimeSpan(1, 30, 0), smallBoat, Shackville_Cargo, Newton.GetHarbor(), Island.GetHarbor(), 6);
            Ships.Add(Newton_Shackville2);
        }
        public void BuildPreRequisites()
        {
            Ships.ForEach(a => a.BuildPreRequisites());
        }
        public void Update(GameTime gametime)
        {
            Ships.ForEach(a => a.Update(gametime));


            if (GSS.FollowOverride == CameraOverride.FOLLOW_SHIP)
            {
                //TODO: Follow Ship (Bug found when crossing edge)
                DebugConsole.WriteLine($@"Ship X:{Ships[1].GetPos().X} Y:{Ships[1].GetPos().Y} 
Camera X:{GSS.World.worldCharacter.CameraPostition.X} Y:{GSS.World.worldCharacter.CameraPostition.Y}");
                GSS.World.worldCharacter.CameraPostition = (Ships[1].GetPos() / EngineSettings.TileSize) - (new Vector2(WorldInformation.mapWidth / 2, WorldInformation.mapHeight / 2) / EngineSettings.TileSize);
            }
        }
        public void Draw()
        {
            if (GSS.DebugMode)
            {
                GSS.SpriteBatch.Draw(GSS.World.Minimap.Map(),
                     new Rectangle(0, 0, DisplayOutputSettings.ScreenWidth, DisplayOutputSettings.ScreenHeight), Color.White);
            }
            Ships.ForEach(a => a.Draw());
        }
    }
}
