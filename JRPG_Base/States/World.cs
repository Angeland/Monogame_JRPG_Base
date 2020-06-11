using JRPG_Base.States.AutoElements;
using JRPG_Base.States.Characters;
using JRPG_Base.States.Characters.Uniqe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JRPG_Base.States
{
    public class World
    {
        #region variables
        private Rectangle[] _minimapLightAnimationSize = new Rectangle[3]{
            new Rectangle(0,0,32,32),
            new Rectangle(33,0,32,32),
            new Rectangle(65,0,32,32)};

        /// <summary>
        /// List of tiles to use
        /// </summary>
        private Texture2D[] _worldmapTilesTexSet,
            _worldCloudTexSet;
        /// <summary>
        /// 
        /// </summary>
        private object[] _tileUsedBy;

        public Texture2D minimapTex,
            minimapCursorTex;
        public World_Character worldCharacter;

        /// <summary>
        /// Bitmap index of world-map
        /// </summary>
        private Texture2D[] _worldMapAnimationSet;
        public int mapHeight = 0,
            mapWidth = 0,
            minimapXoff = 0;

        //Index of colors from the worldMap bitmap
        private Color[][] _worldMapSpriteSet;
        private Color[] _activeWorldMapSprite;
        public Color[] worldMapCheck;
        private Color _timeOfDayColor;
        private List<MapCloud> _cloudsInPlay = new List<MapCloud>();
        /// <summary>
        /// Map Index to show
        /// </summary>
        private int
            _mapAnimationDirection = 1,
            _activeWorldMapSpriteSetIndex = 1;

        private Color _sunColor = Color.White;
        private int
            _minimapW = 0,
            _minimapH = 0,
            _lightX = 0,
            _lightY = 0;

        private bool battleTransition = false;
        private bool _minimapActive = false;
        private Stopwatch sw = new Stopwatch();

        private int
            _halfWidthPlus,
            _halfHeightPlus;

        private Keys isKeyDown;
        private WorldAutoElements _autoElements;
        #endregion

        #region Init code
        /// <summary>
        /// Initialize the tile engine
        /// </summary>
        /// <param name="tiles">List of tiles to use</param>
        /// <param name="worldMap">Bitmap index of world-map</param>
        /// <param name="ScreenTexturecolors">Index of colors from the worldMap bitmap</param>
        /// <param name="Outofbound">Fallback texture to use on tiles not render-able</param>
        public void InitTiles(Texture2D[] Tiles, Texture2D[] WorldMap,
            Texture2D[] MinimapAssets, Texture2D[] WorldClouds, World_Character WorldCharacter, WorldAutoElements AutoElements)
        {
            this._autoElements = AutoElements;
            this.worldCharacter = WorldCharacter;
            _worldCloudTexSet = WorldClouds;
            minimapTex = MinimapAssets[0];
            minimapCursorTex = MinimapAssets[1];
            _worldMapAnimationSet = WorldMap;

            mapHeight = _worldMapAnimationSet[0].Height;
            mapWidth = _worldMapAnimationSet[0].Width;
            _minimapW = (int)(GSS.ScreenWidth * 0.2);
            _minimapH = (int)(GSS.ScreenHeight * 0.2);

            _worldmapTilesTexSet = Tiles;
            _timeOfDayColor = new Color(200, 200, 200);

            int mapCount = WorldMap.Length;
            _worldMapSpriteSet = new Color[mapCount][];
            for (int a = 0; a < mapCount; a++)
            {
                _worldMapSpriteSet[a] = new Color[WorldMap[a].Width * WorldMap[a].Height];
                WorldMap[a].GetData(_worldMapSpriteSet[a]);
            }
            _activeWorldMapSprite = _worldMapSpriteSet[_activeWorldMapSpriteSetIndex];
            worldMapCheck = _worldMapSpriteSet[0];

            _tileUsedBy = new object[mapHeight * mapWidth];
            //+1 is added to buffed draw the next coulomb to the right of the frame
            //1600 is screen width
            _halfHeightPlus = (int)(GSS.ScreenWidth / (float)GSS.TileSize) + 1;
            //900 is screen height
            //+2 is added to buffed draw the next two rows bellow the frame (one row was not enough)
            _halfWidthPlus = (int)(GSS.ScreenHeight / (float)GSS.TileSize) + 2;

            minimapXoff = GSS.ScreenWidth - _minimapW - 10;
            runInitWork();
            sw.Start();
        }
        private void runInitWork()
        {
            worldCharacter.PlaceCamera();
            _autoElements.BuildPreRequisites();
            initMapColors();

        }
        private Dictionary<Color, int> _colorToTile = new Dictionary<Color, int>();
        private Dictionary<Color, bool> _colorToWalkables = new Dictionary<Color, bool>();
        private Dictionary<Color, bool> _sailAble = new Dictionary<Color, bool>();
        private void initMapColors()
        {

            _colorToTile.Add(new Color(34, 177, 76), 0);//Grass
            _colorToWalkables.Add(new Color(34, 177, 76), true);//Grass

            _colorToTile.Add(new Color(239, 228, 176), 1);//Sand/Desert
            _colorToWalkables.Add(new Color(239, 228, 176), true);//Sand/Desert

            _colorToTile.Add(new Color(153, 217, 234), 2);//Beach Sea
            _colorToWalkables.Add(new Color(153, 217, 234), true);//Beach Sea
            //_sailAble.Add(new Color(153, 217, 234), true);//Beach Sea

            _colorToTile.Add(new Color(0, 162, 232), 3);//Shallow Sea
            _sailAble.Add(new Color(0, 162, 232), true);//Shallow Sea

            _colorToTile.Add(new Color(63, 72, 204), 4);//Deep Sea
            _sailAble.Add(new Color(63, 72, 204), true);//Deep Sea

            _colorToTile.Add(new Color(195, 195, 195), 5);//City ground
            _colorToWalkables.Add(new Color(195, 195, 195), true);//City ground

            _colorToTile.Add(new Color(127, 127, 127), 6);//City Wall

            _colorToTile.Add(new Color(185, 122, 87), 7);//City Port
            _colorToWalkables.Add(new Color(185, 122, 87), true);//City Port

            _colorToTile.Add(new Color(201, 151, 124), 8);//House Roof Up

            _colorToTile.Add(new Color(206, 175, 119), 9);//House Roof Down

            _colorToTile.Add(new Color(216, 179, 158), 10);//House Roof Right

            _colorToTile.Add(new Color(218, 188, 156), 11);//House Roof Left   

            _colorToTile.Add(new Color(170, 105, 70), 12);//House Roof Angle           

            _colorToTile.Add(new Color(174, 0, 27), 13);//Ridge Roof Vertical

            _colorToTile.Add(new Color(136, 0, 21), 14);//Ridge Roof Horizontal

            _colorToTile.Add(new Color(91, 0, 14), 15);//Ridge Roof Angle
        }
        #endregion

        #region Support code
        public void startBattle()
        {
            battleTransition = true;
        }
        public void AddElementToMap(int x, int y, object obj)
        {
            _tileUsedBy[(int)(y * mapWidth) + x] = obj;
        }
        public bool MapLocationContainsElement(int x, int y)
        {
            if (_tileUsedBy[(int)(y * mapWidth) + x] != default(object))
                return true;
            return false;
        }
        public object GetObjectFromLocation(int x, int y)
        {
            return _tileUsedBy[(int)(y * mapWidth) + x];
        }
        public bool IsWalkable(int tileX, int tileY)
        {
            return IsAble(tileX, tileY, _colorToWalkables) &&
                IsAble(tileX + 1, tileY, _colorToWalkables) &&
                IsAble(tileX, tileY + 1, _colorToWalkables) &&
                IsAble(tileX + 1, tileY + 1, _colorToWalkables);
        }
        public bool IsTileSailable(int tileX, int tileY, Vector2 size)
        {
            return IsAble(tileX, tileY, _sailAble) &&
                IsAble(tileX + (int)size.X, tileY, _sailAble) &&
                IsAble(tileX, tileY + (int)size.Y, _sailAble) &&
                IsAble(tileX + (int)size.X, tileY + (int)size.Y, _sailAble);
        }

        public bool IsAble(int tileX, int tileY, Dictionary<Color, bool> able)
        {
            if (tileX >= mapWidth) tileX -= mapWidth;
            else if (tileX < 0) tileX += mapWidth;
            if (tileY >= mapHeight) tileY -= mapHeight;
            else if (tileY < 0) tileY += mapHeight;
            return able.ContainsKey(worldMapCheck[tileX + (tileY * mapWidth)]);
        }
        #endregion
        #region Update code
        public void Update(GameTime gameTime)
        {
            if (!battleTransition)
            {
                worldCharacter.Update(gameTime);
            }
            worldMovement();

            //WorldMapAnimation
            worldMapAnimation(gameTime);
            _autoElements.Update(gameTime);

            _sunColor = GSS.getSunPositionColor();
        }
        private void worldMovement()
        {
            if (GSS.KeyState.IsKeyDown(Keys.X) && isKeyDown == Keys.None)
            {
                isKeyDown = Keys.X;
                _minimapActive = !_minimapActive;
            }
            else if (GSS.KeyState.IsKeyUp(Keys.X))
            {
                isKeyDown = Keys.None;
            }
            if (GSS.PadState.Buttons.Y == ButtonState.Pressed || GSS.KeyState.IsKeyDown(Keys.M))
            {
                GSS.ingameMenu.openMenu();
            }
        }
        private void worldMapAnimation(GameTime gameTime)
        {
            if (gameTime.ElapsedGameTime.Milliseconds > 400 * GSS.ClockSpeed)
            {
                _activeWorldMapSpriteSetIndex += _mapAnimationDirection;

                if (_activeWorldMapSpriteSetIndex + 1 >= _worldMapAnimationSet.Length)
                    _mapAnimationDirection = -1;
                else if (_activeWorldMapSpriteSetIndex == 0)
                    _mapAnimationDirection = 1;

                _activeWorldMapSprite = _worldMapSpriteSet[_activeWorldMapSpriteSetIndex];
                sw.Restart();
            }
            worldMapClouds(gameTime);

            float pcX = worldCharacter.CameraPostition.X / (float)(mapWidth);
            float pcY = worldCharacter.CameraPostition.Y / (float)(mapHeight);

            _lightX = (int)((_minimapW * pcX) + GSS.ScreenWidth - _minimapW - 10);
            _lightY = (int)((_minimapH * pcY) + 10);

        }
        private void worldMapClouds(GameTime gameTime)
        {
            _cloudsInPlay.ForEach(a => a.Update());
            _cloudsInPlay.RemoveAll(a => !a.isAlive);

            if (_cloudsInPlay.Count < 500)
            {
                MapCloud newCloud = new MapCloud(_worldCloudTexSet[0].Width, _worldCloudTexSet[0].Height, mapWidth, mapHeight);
                _cloudsInPlay.Add(newCloud);
            }
        }
        #endregion
        #region Draw code
        public void Draw()
        {
            GSS.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.DepthRead, RasterizerState.CullNone);
            //World-map
            drawWorldMap();
            //Player on Screen
            worldCharacter.Draw();
            //Draw WorldElements
            _autoElements.Draw();
            //Clouds
            drawClouds();
            //_minimapActive
            if (_minimapActive)
                drawMinimap();

            //GSS.ExtraOutput = "Time " + GSS.getTime().ToString("yyyy-MM-dd HH:mm:ss");
            GSS.spriteBatch.End();
            if (battleTransition)
            {
            }
        }

        private void drawWorldMap()
        {
            int x1 = 0, y1 = 0;
            Color FoundColor;
            for (int yOffset = (int)worldCharacter.CameraPostition.Y * mapWidth, TileY = -GSS.TileSize + (int)worldCharacter.CharacterOffset.Y; yOffset < (int)(worldCharacter.CameraPostition.Y + _halfWidthPlus + 1) * mapWidth; yOffset += mapWidth, TileY += GSS.TileSize)
            {
                y1++;
                //-1 is added to buffer draw the next coulomb to the left of the frame
                for (int x = (int)worldCharacter.CameraPostition.X - 1 + yOffset, TileX = -GSS.TileSize + (int)worldCharacter.CharacterOffset.X; x < (int)worldCharacter.CameraPostition.X + _halfHeightPlus + yOffset; x++, TileX += GSS.TileSize)
                {
                    x1++;
                    int ColorIndex = x;

                    while (ColorIndex < yOffset)
                        ColorIndex += mapWidth;
                    while (ColorIndex > yOffset + mapWidth)
                        ColorIndex -= mapWidth;

                    while (ColorIndex < 0)
                        ColorIndex += _activeWorldMapSprite.Length;
                    while (ColorIndex >= _activeWorldMapSprite.Length)
                        ColorIndex -= _activeWorldMapSprite.Length;

                    FoundColor = _activeWorldMapSprite[ColorIndex];

                    int col = _colorToTile[FoundColor];
                    Texture2D tmp = _worldmapTilesTexSet[col];

                    GSS.spriteBatch.Draw(tmp,
                        new Rectangle(TileX, TileY, GSS.TileSize, GSS.TileSize), _sunColor);

                }
            }
        }
        private void drawClouds()
        {
            int inViewCount = 0;
            Vector2 pos = worldCharacter.getPosition();
            foreach (MapCloud a in _cloudsInPlay.Where(b => b.WithinCamera(pos)))
            {
                inViewCount++;
                GSS.spriteBatch.Draw(_worldCloudTexSet[0], a.getShape(pos), a.getColor(_sunColor));
            }
        }
        private void drawMinimap()
        {
            //_minimapActive
            GSS.spriteBatch.Draw(minimapTex,
                new Rectangle(minimapXoff, 10, _minimapW, _minimapH), Color.White);
            //_minimapLightAnimationSize
            GSS.spriteBatch.Draw(minimapCursorTex,
                new Rectangle(_lightX
                    , _lightY
                    , 20, 20), _minimapLightAnimationSize[_activeWorldMapSpriteSetIndex], Color.Yellow);
        }
        #endregion
    }
}
