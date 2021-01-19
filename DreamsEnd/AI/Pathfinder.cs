using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RPG.Library;
using RPG.States.Configuration;
using RPG.States.World;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RPG.AI
{
    public class Pathfinder
    {
        private static readonly float Sqrt2 = (float)Math.Sqrt(2);
        private Node PathNode = null;
        private Vector2 FromOff;
        private Vector2 _elementSize;
        private Vector2 _goal;
        private Vector2 CorrectedGoal;
        private List<Node> OpenList = new List<Node>();
        private List<Vector2> ClosedList = new List<Vector2>();
        private bool[,] Visited;


        public List<Vector2> GetPath(Vector2 Start, Vector2 Goal, Vector2 ElementSize)
        {
            Visited = new bool[WorldInformation.mapWidth, WorldInformation.mapHeight];

            InitVisitMap();
            _elementSize = ElementSize / EngineSettings.TileSize;
            _goal = Goal;
            FromOff = new Vector2((WorldInformation.mapWidth / 2) - Start.X, (WorldInformation.mapHeight / 2) - Start.Y);
            CorrectedGoal = WorldHelp.CorrectWorldOverflow(_goal + FromOff);
            return FindPath(Start);
        }
        void InitVisitMap()
        {
            for (int x = 0; x < WorldInformation.mapWidth; x++)
            {
                for (int y = 0; y < WorldInformation.mapHeight; y++)
                {
                    Visited[x, y] = false;
                }
            }
        }
        Node InitPF(Vector2 From)
        {
            ClosedList = new List<Vector2>();
            OpenList = new List<Node>();
            Node startNode = new Node()
            {
                position = From,
                G = 0
            };
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
                            Vector2 correctedNewPos = WorldHelp.CorrectWorldOverflow(newPos);

                            if (Visited[(int)correctedNewPos.X, (int)correctedNewPos.Y])
                            {
                                continue;
                            }
                            else if (OpenList.Any(a => a.position == correctedNewPos))
                            {
                                continue;
                            }
                            else if (TilePalette.IsTileSailable((int)correctedNewPos.X, (int)correctedNewPos.Y, _elementSize))
                            {
                                Node newNode = new Node()
                                {
                                    parent = current,
                                    position = correctedNewPos,
                                    G = current.G + ((X == 0 || Y == 0) ? 1 : Sqrt2)
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
                float tmpF = node.G + AngleSelection(node);
                if (tmpF < F)
                {
                    F = tmpF * (1001 / 1000);
                    check = node;
                }
            }
            return check;
        }

        private float AngleSelection(Node node)
        {
            int selection = 3;
            if (selection == 1)
            {
                return GetManhattanH(node);
            }
            if (selection == 2)
            {
                return GetEuclideanH(node);
            }
            if (selection == 3)
            {
                return GetChebyshevH(node);
            }
            return 0;
        }

        float GetManhattanH(Node node)
        {
            Vector2 altPos = WorldHelp.CorrectWorldOverflow(FromOff + node.position);
            float Hx = Math.Abs(CorrectedGoal.X - altPos.X);
            float Hy = Math.Abs(CorrectedGoal.Y - altPos.Y);
            return (Hx + Hy) * 1;
        }
        float GetEuclideanH(Node node)
        {
            Vector2 altPos = WorldHelp.CorrectWorldOverflow(FromOff + node.position);
            float Hx = CorrectedGoal.X - altPos.X;
            float Hy = CorrectedGoal.Y - altPos.Y;

            return (float)(Math.Sqrt(Math.Pow(Hx, 2) + Math.Pow(Hy, 2)) * ((Hx == 0 || Hy == 0) ? 1 : Sqrt2));
        }
        float GetChebyshevH(Node node)
        {
            Vector2 altPos = WorldHelp.CorrectWorldOverflow(FromOff + node.position);
            float Hx = Math.Abs(CorrectedGoal.X - altPos.X);
            float Hy = Math.Abs(CorrectedGoal.Y - altPos.Y);
            return (float)(Math.Max(Hx, Hy) * ((Hx == 0 || Hy == 0) ? 1 : Sqrt2));
        }
        public void Draw(Texture2D Texture)
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
                    GSS.SpriteBatch.Draw(Texture,
                         new Rectangle((int)a.X, (int)a.Y, 1, 1), Color.Blue);
                }
                foreach (Vector2 a in tmpClosedList)
                {
                    GSS.SpriteBatch.Draw(Texture,
                         new Rectangle((int)a.X, (int)a.Y, 1, 1), Color.Red);
                }
                GSS.SpriteBatch.Draw(Texture,
                     new Rectangle((int)_goal.X - 1, (int)_goal.Y - 1, 2, 2), Color.Green);
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
