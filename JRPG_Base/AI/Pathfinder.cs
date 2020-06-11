using JRPG_Base.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG_Base.AI
{
    public class Pathfinder
    {
        private float _sqrt2 = (float)(Math.Sqrt(2));
        public bool InDraw = false;
        public bool InChange = false;
        public bool PathFound = false;
        public Node PathNode = null;
        public Vector2 FromOff;
        private Vector2 _elementSize;
        public Vector2 _goal, CorrectedGoal;
        public List<Node> OpenList = new List<Node>();
        public List<Vector2> ClosedList = new List<Vector2>();
        public bool[,] Visited;


        public List<Vector2> getPath(Vector2 Start, Vector2 Goal, Vector2 ElementSize)
        {
            Visited = new bool[GSS.world.mapWidth, GSS.world.mapHeight];

            initVisitMap();
            _elementSize = ElementSize / GSS.TileSize;
            _goal = Goal;
            FromOff = new Vector2((GSS.world.mapWidth / 2) - Start.X, (GSS.world.mapHeight / 2) - Start.Y);
            CorrectedGoal = WorldHelp.correctWorldOverflow(_goal + FromOff);
            return FindPath(Start);
        }
        void initVisitMap()
        {
            for (int x = 0; x < GSS.world.mapWidth; x++)
            {
                for (int y = 0; y < GSS.world.mapHeight; y++)
                {
                    Visited[x, y] = false;
                }
            }
        }
        Node InitPF(Vector2 From)
        {
            ClosedList = new List<Vector2>();
            OpenList = new List<Node>();
            Node startNode = new Node();
            startNode.position = From;
            startNode.G = 0;
            PathNode = startNode;
            OpenList.Add(startNode);
            return startNode;
        }
        List<Vector2> FindPath(Vector2 From)
        {
            Node startNode = InitPF(From);
            while (OpenList.Count != 0)
            {
                Node current = FindLowestF(OpenList);
                if (current.position == _goal)
                {
                    //Return goal
                    PathFound = true;
                    PathNode = current;
                    return ExtractPath();
                }
                
                OpenList.Remove(current);
                ClosedList.Add(current.position);
                Visited[(int)current.position.X, (int)current.position.Y] = true;

                for (int X = -1; X <= 1; X++)
                {
                    for (int Y = -1; Y <= 1; Y++)
                    {
                        if (X != 0 || Y != 0)
                        {
                            Vector2 newPos = new Vector2(current.position.X + X, current.position.Y + Y);
                            Vector2 correctedNewPos = WorldHelp.correctWorldOverflow(newPos);

                            if (Visited[(int)correctedNewPos.X, (int)correctedNewPos.Y])
                            {
                                continue;
                            }
                            else if (OpenList.Any(a => a.position == correctedNewPos))
                            {
                                continue;
                            }
                            else if (GSS.world.IsTileSailable((int)correctedNewPos.X, (int)correctedNewPos.Y, _elementSize))
                            {
                                Node newNode = new Node()
                                {
                                    parent = current,
                                    position = correctedNewPos,
                                    G = current.G + ((X == 0 || Y == 0) ? 1 : _sqrt2)
                                };
                                OpenList.Add(newNode);
                            }
                        }
                    }
                }
            }
            return new List<Vector2>();
        }
        List<Vector2> ExtractPath()
        {
            List<Vector2> path = new List<Vector2>();
            Node end = PathNode;
            while (end.parent != null)
            {
                path.Add(end.position);
                end = end.parent;
            }
            return path;
        }
        Node FindLowestF(List<Node> list)
        {
            Node check = null;
            float F = float.MaxValue;
            foreach (Node node in list)
            {
                float tmpF = node.G + getChebyshevH(node);
                if (tmpF < F)
                {
                    F = tmpF * (1001 / 1000);
                    check = node;
                }
            }
            return check;
        }
        float getManhattanH(Node node)
        {
            Vector2 altPos = WorldHelp.correctWorldOverflow(FromOff + node.position);
            float Hx = Math.Abs(CorrectedGoal.X - altPos.X);
            float Hy = Math.Abs(CorrectedGoal.Y - altPos.Y);
            return (Hx + Hy) * 1;
        }
        float getEuclideanH(Node node)
        {
            Vector2 altPos = WorldHelp.correctWorldOverflow(FromOff + node.position);
            float Hx = CorrectedGoal.X - altPos.X;
            float Hy = CorrectedGoal.Y - altPos.Y;

            return (float)(Math.Sqrt(Math.Pow(Hx, 2) + Math.Pow(Hy, 2)) * ((Hx == 0 || Hy == 0) ? 1 : _sqrt2));
        }
        float getChebyshevH(Node node)
        {
            Vector2 altPos = WorldHelp.correctWorldOverflow(FromOff + node.position);
            float Hx = Math.Abs(CorrectedGoal.X - altPos.X);
            float Hy = Math.Abs(CorrectedGoal.Y - altPos.Y);
            return (float)(Math.Max(Hx, Hy) * ((Hx == 0 || Hy == 0) ? 1 : _sqrt2));
        }
        public void Draw(Texture2D Texture)
        {
            InDraw = true;
            if (!InChange)
            {
                var tmpOpenList = new List<Vector2>();
                var tmpClosedList = new List<Vector2>();
                try
                {
                    tmpOpenList = OpenList.Select(b => b.position).ToList();
                    tmpClosedList = ClosedList.ToList();
                }
                catch { }

                if (tmpOpenList.Count != 0)
                {
                    foreach (Vector2 a in tmpOpenList)
                    {
                        GSS.spriteBatch.Draw(Texture,
                             new Rectangle((int)a.X, (int)a.Y, 1, 1), Color.Blue);
                    }
                    foreach (Vector2 a in tmpClosedList)
                    {
                        GSS.spriteBatch.Draw(Texture,
                             new Rectangle((int)a.X, (int)a.Y, 1, 1), Color.Red);
                    }
                    GSS.spriteBatch.Draw(Texture,
                         new Rectangle((int)_goal.X - 1, (int)_goal.Y - 1, 2, 2), Color.Green);
                    InDraw = false;
                }
            }
        }

        public class Node
        {
            public float G = 0;
            public Node parent = null;
            public Vector2 position;
        }
    }
}
