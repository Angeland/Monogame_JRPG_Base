using Microsoft.Xna.Framework;
using RPG.Library;
using RPG.States.Configuration;
using RPG.States.World;
using System;

namespace RPG.States.Characters.Uniqe
{
    class MapCloud
    {
        private const float speed = 5 / GSS.ClockSpeed;
        private const float halfSpeed = speed / 2f;
        private int TTL;
        private double maxTTL;
        public bool IsAlive { get { return TTL > 0 ? true : false; } }
        private Vector2 Position;
        private Vector2 Size;
        private Vector2 Velocity = Vector2.Zero;

        public MapCloud(int maxWidth, int maxHeight)
        {
            TTL = GSS.GlobalRand.Next(2000);
            maxTTL = TTL;
            Position = new Vector2(GSS.GlobalRand.Next(0, WorldInformation.mapWidth * EngineSettings.TileSize), GSS.GlobalRand.Next(0, WorldInformation.mapHeight * EngineSettings.TileSize));
            while (Velocity.X == 0 && Velocity.Y == 0)
                Velocity = new Vector2((float)((GSS.GlobalRand.NextDouble() * speed) - halfSpeed), (float)(GSS.GlobalRand.NextDouble() * speed) - halfSpeed);
            Size = new Vector2(GSS.GlobalRand.Next((int)(maxWidth * 0.2d), maxWidth), GSS.GlobalRand.Next((int)(maxHeight * 0.2d), maxHeight));
        }

        public Rectangle GetShape(Vector2 cameraPos)
        {
            return DrawHelp.GetBoundary(cameraPos, Position, Size);
        }

        public Color GetColor(Color baseColor)
        {
            double td = Math.Sin((TTL * Math.PI) / maxTTL);
            baseColor.A = (byte)((int)(td * 255));
            return baseColor;
        }

        public bool WithinCamera(Vector2 cameraPos)
        {
            return CameraHelp.WithinCamera(cameraPos, Position, Size);
        }

        public void Update()
        {
            Position.X += Velocity.X;
            Position.Y += Velocity.Y;
            TTL--;
        }
    }
}
