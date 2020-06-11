using JRPG_Base.Library;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG_Base.States.Characters.Uniqe
{
    class MapCloud
    {
        private const float speed = 5 / GSS.ClockSpeed;
        private const float halfSpeed = speed / 2f;
        private int TTL;
        private double maxTTL;
        public bool isAlive { get { return TTL > 0 ? true : false; } }
        private Vector2 Position;
        private Vector2 Size;
        private Vector2 Velocity = Vector2.Zero;

        public MapCloud(int ttl, Vector2 pos, Vector2 size)
        {
            TTL = ttl;
            maxTTL = ttl;
            Position = pos;
            while (Velocity.X == 0 && Velocity.Y == 0)
                Velocity = new Vector2((float)((GSS.globalRand.NextDouble() * speed) - halfSpeed), (float)(GSS.globalRand.NextDouble() * speed) - halfSpeed);
            Size = size;
        }
        public MapCloud(int maxWidth, int maxHeight, int maxPosW, int maxPosH)
        {
            TTL = GSS.globalRand.Next(2000);
            maxTTL = TTL;
            Position = new Vector2((float)GSS.globalRand.Next(0, maxPosW * GSS.TileSize), (float)GSS.globalRand.Next(0, maxPosH * GSS.TileSize));
            while (Velocity.X == 0 && Velocity.Y == 0)
                Velocity = new Vector2((float)((GSS.globalRand.NextDouble() * speed) - halfSpeed), (float)(GSS.globalRand.NextDouble() * speed) - halfSpeed);
            Size = new Vector2((float)GSS.globalRand.Next((int)(maxWidth * 0.2d), maxWidth), (float)GSS.globalRand.Next((int)(maxHeight * 0.2d), maxHeight));
        }

        public Rectangle getShape(Vector2 cameraPos)
        {
            return DrawHelp.getShape(cameraPos, Position, Size);
        }

        public Color getColor(Color baseColor)
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
