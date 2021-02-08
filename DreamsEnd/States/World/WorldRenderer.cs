using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DreamsEnd.States.Animation;
using DreamsEnd.States.AutoElements;
using DreamsEnd.States.Characters;
using DreamsEnd.States.Characters.Uniqe;
using DreamsEnd.States.Configuration;
using DreamsEnd.States.Scene;
using DreamsEnd.States.World;
using DreamsEnd.Textures;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System;
using Microsoft.Xna.Framework.Input;
using DreamsEnd.States.DebugHelp;

namespace DreamsEnd.States
{
    public class WorldRenderer
    {
        #region variables
        private readonly Rectangle[] _minimapLightAnimationSize = new Rectangle[3]{
            new Rectangle(0,0,32,32),
            new Rectangle(33,0,32,32),
            new Rectangle(65,0,32,32)};

        /// <summary>
        /// List of tiles to use
        /// </summary>
        private ITilesCollection cloudsTileCollection { get; set; }
        private ITilesCollection MapTileCollection { get; set; }
        public Minimap Minimap { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        private object[] _tileUsedBy;

        public IWorldCharacter worldCharacter;

        /// <summary>
        /// Bitmap index of world-map
        /// </summary>
        private ITilesCollection _worldMapAnimationSet;
        public int minimapXoff = 0;
        public int minimapYoff = 10;

        //Index of colors from the worldMap bitmap
        private Color[][] _worldMapSpriteSet;
        private readonly List<MapCloud> _cloudsInPlay = new List<MapCloud>();
        /// <summary>
        /// Map Index to show
        /// </summary>

        private Color _sunColor = Color.White;
        private int
            _minimapW = 0,
            _minimapH = 0;
        Point minimapBeacon;

        private bool battleTransition = false;
        private bool _minimapActive = false;
        private readonly Stopwatch sw = new Stopwatch();

        private int
            _halfWidthPlus,
            _halfHeightPlus;

        private WorldAutoElements _autoElements;
        private AnimationRotator<Rectangle> _minimapSpriteAnimator;
        private AnimationRotator<Color[]> _worldmapAnimator;
        #endregion

        #region Init code
        /// <summary>
        /// Initialize the tile engine
        /// </summary>
        /// <param name="tiles">List of tiles to use</param>
        /// <param name="worldMap">Bitmap index of world-map</param>
        /// <param name="ScreenTexturecolors">Index of colors from the worldMap bitmap</param>
        /// <param name="Outofbound">Fallback texture to use on tiles not render-able</param>
        public void LoadWorld()
        {
            MapTileCollection = GSS.TextureLoader.WorldMapTiles();
            _autoElements = GSS.TextureLoader.WorldAutoElements();
            _autoElements.SetMapTileCollection(MapTileCollection);
            worldCharacter = GSS.TextureLoader.WorldCharacter();
            worldCharacter.SetMapTileCollection(MapTileCollection);
            cloudsTileCollection = GSS.TextureLoader.Clouds();
            Minimap = GSS.TextureLoader.Minimap();
            _worldMapAnimationSet = GSS.TextureLoader.WorldMaps();
            _minimapSpriteAnimator = new AnimationRotator<Rectangle>(_minimapLightAnimationSize, AnimationFunction.FORWARD_BACKWARD, playTimeSeconds: 2f);

            WorldInformation.mapWidth = (int)_worldMapAnimationSet.Size.X;
            WorldInformation.mapHeight = (int)_worldMapAnimationSet.Size.Y;
            _minimapW = (int)(DisplayOutputSettings.ScreenWidth * 0.2);
            _minimapH = (int)(DisplayOutputSettings.ScreenHeight * 0.2);


            int mapCount = _worldMapAnimationSet.Length;
            _worldMapSpriteSet = new Color[mapCount][];
            for (int a = 0; a < mapCount; a++)
            {
                _worldMapSpriteSet[a] = new Color[(int)(_worldMapAnimationSet.Size.X * _worldMapAnimationSet.Size.Y)];
                _worldMapAnimationSet.GetTexture(a).GetData(_worldMapSpriteSet[a]);
            }
            _worldmapAnimator = new AnimationRotator<Color[]>(_worldMapSpriteSet, AnimationFunction.FORWARD_BACKWARD, playTimeSeconds: 3f);

            WorldInformation.worldMapCheck = _worldMapSpriteSet[0];

            _tileUsedBy = new object[WorldInformation.mapHeight * WorldInformation.mapWidth];
            //+1 is added to buffed draw the next coulomb to the right of the frame
            //1600 is screen width
            _halfHeightPlus = (int)(DisplayOutputSettings.ScreenWidth / (float)EngineSettings.TileSize) + 1;
            //900 is screen height
            //+2 is added to buffed draw the next two rows bellow the frame (one row was not enough)
            _halfWidthPlus = (int)(DisplayOutputSettings.ScreenHeight / (float)EngineSettings.TileSize) + 2;

            minimapXoff = DisplayOutputSettings.ScreenWidth - _minimapW - 10;
            RunInitWork(MapTileCollection);
            sw.Start();
        }
        private void RunInitWork(ITilesCollection mapTileCollection)
        {
            _autoElements.BuildPreRequisites(mapTileCollection);
        }
        #endregion

        #region Support code
        public void StartBattle()
        {
            battleTransition = true;
        }
        public void AddElementToMap(int x, int y, object obj)
        {
            _tileUsedBy[(y * WorldInformation.mapWidth) + x] = obj;
        }
        public bool MapLocationContainsElement(int x, int y)
        {
            return _tileUsedBy[(y * WorldInformation.mapWidth) + x] != default;
        }
        public object GetObjectFromLocation(int x, int y)
        {
            return _tileUsedBy[(y * WorldInformation.mapWidth) + x];
        }

        #endregion

        #region Update code
        public void Update(GameTime gameTime)
        {
            if (!battleTransition)
            {
                worldCharacter.Update(gameTime);
            }
            WorldMovement();

            //WorldMapAnimation
            WorldMapAnimation(gameTime);
            _autoElements.Update(gameTime);

            _sunColor = GSS.GetSunPositionColor();

            SceneWorker.AttemptSceneChange(worldCharacter.GetCenterTilePosition());
        }

        private void WorldMovement()
        {
            if (GSS.Controlls.AltConfirm())
            {
                _minimapActive = !_minimapActive;
            }
            if (GSS.Controlls.Menu())
            {
                GSS.IngameMenu.OpenMenu();
            }
        }

        private void WorldMapAnimation(GameTime gameTime)
        {
            _minimapSpriteAnimator.Update(gameTime);
            _worldmapAnimator.Update(gameTime);
            WorldMapClouds(gameTime);

            Vector2 pc = worldCharacter.GetCenterTilePosition();
            pc /= WorldInformation.MapSize;
            pc *= new Vector2(_minimapW, _minimapH);
            minimapBeacon = new Point((int)pc.X, (int)pc.Y);
        }

        private void WorldMapClouds(GameTime gameTime)
        {
            _cloudsInPlay.ForEach(a => a.Update());
            _cloudsInPlay.RemoveAll(a => !a.IsAlive);

            if (_cloudsInPlay.Count < 500)
            {
                MapCloud newCloud = new MapCloud(cloudsTileCollection.GetTexture().Width, cloudsTileCollection.GetTexture().Height);
                _cloudsInPlay.Add(newCloud);
            }
        }
        #endregion

        #region Draw code
        public void Draw()
        {
            GSS.SpriteBatch.Begin();
            //World-map
            DrawWorldMap();
            //Player on Screen
            worldCharacter.Draw();
            //Draw WorldElements
            _autoElements.Draw();
            //Clouds
            DrawClouds();
            if (GSS.LoggingEnabeled)
            {
                ActivateTileUnderMouse();
            }
            //_minimapActive
            if (_minimapActive)
            {
                DrawMinimap();
            }

            //GSS.ExtraOutput = "Time " + GSS.getTime().ToString("yyyy-MM-dd HH:mm:ss");
            GSS.SpriteBatch.End();
            if (battleTransition)
            {
            }
        }

        private void DrawWorldMap()
        {
            Vector2 cameraPostition = worldCharacter.CameraPosition;
            Point positionOffset = worldCharacter.PositionOffset + new Point(EngineSettings.TileSize, EngineSettings.TileSize);

            int worldmapLength = _worldMapSpriteSet[0].Length;
            for (int yOffset = ((int)cameraPostition.Y * WorldInformation.mapWidth) - 1, TileY = -positionOffset.Y; yOffset < (int)(cameraPostition.Y + _halfWidthPlus) * WorldInformation.mapWidth; yOffset += WorldInformation.mapWidth, TileY += EngineSettings.TileSize)
            {
                for (int x = (int)cameraPostition.X + yOffset, TileX = -positionOffset.X; x < (int)cameraPostition.X + _halfHeightPlus + yOffset + 1; x++, TileX += EngineSettings.TileSize)
                {
                    int ColorIndex = x;

                    while (ColorIndex < yOffset)
                        ColorIndex += WorldInformation.mapWidth;
                    while (ColorIndex > yOffset + WorldInformation.mapWidth)
                        ColorIndex -= WorldInformation.mapWidth;

                    while (ColorIndex < 0)
                        ColorIndex += worldmapLength;
                    while (ColorIndex >= worldmapLength)
                        ColorIndex -= worldmapLength;

                    Color TileColor = _worldmapAnimator.Get()[ColorIndex];
                    Point location = new Point(TileX, TileY);
                    if (MapTileCollection.ContainsTile(TileColor))
                    {

                        if (MapTileCollection.HasSubTexture(TileColor))
                        {
                            GSS.SpriteBatch.Draw(MapTileCollection.GetSubTexture(TileColor),
                                new Rectangle(location, TileSize), _sunColor);
                        }

                        if (GSS.DebugMapX0Y0)
                        {
                            Point t = GetCoordinate(ColorIndex);
                            GSS.SpriteBatch.Draw(MapTileCollection.GetTexture(TileColor),
                                new Rectangle(location, TileSize),
                                t.X == 0 || t.Y == 0 ? new Color(0, 0, 0) : _sunColor);
                        }
                        else
                        {
                            GSS.SpriteBatch.Draw(MapTileCollection.GetTexture(TileColor),
                                new Rectangle(location, TileSize), _sunColor);
                        }
                    }
                    else
                    {
                        GSS.SpriteBatch.Draw(MapTileCollection.GetTexture(), new Rectangle(location, TileSize), Color.Black);
                    }
                }
            }
        }


        private void ActivateTileUnderMouse()
        {
            MouseState mm = Mouse.GetState();

            Vector2 cameraPostition = worldCharacter.CameraPosition;
            Point positionOffset = worldCharacter.PositionOffset;
            Vector2 mouseWorldPosition = new Vector2(cameraPostition.X + mm.X, cameraPostition.Y + mm.Y);
            DebugConsole.WriteLine($"Mouse global tile X:{mouseWorldPosition.X} Y:{mouseWorldPosition.Y}");

            DebugConsole.WriteLine($"Mouse onscreen position X:{mm.X} Y:{mm.Y}");

            var x = mm.X/ EngineSettings.TileSize;
            var y = mm.Y/ EngineSettings.TileSize;
            int finalX = (x * EngineSettings.TileSize)- positionOffset.X+;
            int finalY = (y * EngineSettings.TileSize)- positionOffset.Y;
            DebugConsole.WriteLine($"positionOffset X:{positionOffset.X} Y:{positionOffset.Y}");
            DebugConsole.WriteLine($"Mouse onscreen tile X:{finalX} Y:{finalY}");

            GSS.SpriteBatch.Draw(MapTileCollection.GetTexture(),
                new Rectangle(new Point(finalX, finalY), TileSize),
                Color.Black);
        }

        private static readonly Point TileSize = new Point(EngineSettings.TileSize, EngineSettings.TileSize);
        private Point GetCoordinate(int ColorIndex)
        {
            int y = ColorIndex / WorldInformation.mapWidth;
            int x = ColorIndex - (y * WorldInformation.mapWidth);
            return new Point(x, y);
        }
        private void DrawClouds()
        {
            int inViewCount = 0;
            Vector2 pos = worldCharacter.GetPosition();
            foreach (MapCloud a in _cloudsInPlay.Where(b => b.WithinCamera(pos)))
            {
                inViewCount++;
                GSS.SpriteBatch.Draw(cloudsTileCollection.GetTexture(), a.GetShape(pos), a.GetColor(_sunColor));
            }
        }
        private void DrawMinimap()
        {
            //_minimapActive
            GSS.SpriteBatch.Draw(Minimap.Map(),
                new Rectangle(minimapXoff, minimapYoff, _minimapW, _minimapH),
                Color.White);
            //_minimapLightAnimationSize
            GSS.SpriteBatch.Draw(Minimap.Cursor(),
                new Rectangle(minimapXoff + minimapBeacon.X - 8, minimapYoff + minimapBeacon.Y - 10, 20, 20),
                _minimapSpriteAnimator.Get(),
                new Color(255, 255, 0));
        }
        #endregion
    }
}
