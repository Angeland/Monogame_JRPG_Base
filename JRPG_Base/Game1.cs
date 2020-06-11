#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using JRPG_Base;
using JRPG_Base.States.Characters;
using JRPG_Base.States.AutoElements;
#endregion

namespace JRPG_Base
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {

        /**********************************
         *           GAME CONTENT         *
         *********************************/
        Random random = new Random();


        SpriteFont spriteFont;
        //Sound Engine
        SoundEffectInstance soundEffectInstance;
        //Sounds
        SoundEffect ButtonClick;
        //Location of Emitter
        Vector2 CameraMove = Vector2.Zero;


        //Movement variables
        int stepCounter = 0;
        float MoveSpeed = 2;
        Vector2 lastPosition = Vector2.Zero;

        //Graphic variables
        Color surfaceColor;
        int kolonne, rad, Zoom = 10, getColorIndex, x = 0;
        Color[][] ScreenTextureColors;

        //Monitor Variables
        bool bFullScreen = true;
        float pst10Xangle, pst10Yangle;

        //Game Variables
        bool buttonpres = true, isIngameMenuTexturesLoaded = false, RefreshmapFlag = false;

        public Game1()
            : base()
        {
            GSS.graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = @"D:\JRPG_Base\JRPG_Base\JRPG_Base\Content";
            Content.RootDirectory = "Content";
            IsFixedTimeStep = true; //Låst fps
        }
        private void InitGraphicsMode()
        {
            DisplayMode dm = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            GSS.ScreenHeight = dm.Height;
            GSS.ScreenWidth = dm.Width;
            GSS.graphics.PreferredBackBufferWidth = GSS.ScreenWidth;
            GSS.graphics.PreferredBackBufferHeight = GSS.ScreenHeight;
            GSS.graphics.IsFullScreen = bFullScreen;
            GSS.graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            pst10Xangle = GSS.ScreenWidth * 0.1f;
            pst10Yangle = GSS.ScreenHeight * 0.1f;
            IsMouseVisible = true;
            InitGraphicsMode();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            GSS.spriteBatch = new SpriteBatch(GraphicsDevice);

            //Font Loading
            GSS.baseFont = Content.Load<SpriteFont>("Fonts/Arial");

            //Load Main Menu Buttons
            List<Texture2D> MainMenuBtn = new List<Texture2D>();
            MainMenuBtn.Add(Content.Load<Texture2D>("Menu/easy"));
            MainMenuBtn.Add(Content.Load<Texture2D>("Menu/medium"));
            List<Texture2D> MainMenuActiveBtn = new List<Texture2D>();
            MainMenuActiveBtn.Add(Content.Load<Texture2D>("Menu/easyActive"));
            MainMenuActiveBtn.Add(Content.Load<Texture2D>("Menu/mediumActive"));

            GSS.DefaultTex = Content.Load<Texture2D>("Graphics/Maps/WorldMap/Minimap");

            //Worldmap tex
            Texture2D[] WorldMap = new Texture2D[3];
            WorldMap[0] = Content.Load<Texture2D>("Graphics/Maps/WorldMap/BaseMap");
            WorldMap[1] = Content.Load<Texture2D>("Graphics/Maps/WorldMap/BaseMap1");
            WorldMap[2] = Content.Load<Texture2D>("Graphics/Maps/WorldMap/BaseMap2");
            Texture2D[] WorldClouds = new Texture2D[1];
            WorldClouds[0] = Content.Load<Texture2D>("Graphics/Maps/WorldMap/Overlay/Cloud/cloud1");


            //WorldmapTiles
            List<Texture2D> WorldMapTextures = new List<Texture2D>();
            WorldMapTextures.Add(Content.Load<Texture2D>("Graphics/Maps/WorldMap/Overlay/Land/Grass"));
            WorldMapTextures.Add(Content.Load<Texture2D>("Graphics/Maps/WorldMap/Overlay/Land/Sand"));
            WorldMapTextures.Add(Content.Load<Texture2D>("Graphics/Maps/WorldMap/Overlay/Water/Shallow"));
            WorldMapTextures.Add(Content.Load<Texture2D>("Graphics/Maps/WorldMap/Overlay/Water/Ocean"));
            WorldMapTextures.Add(Content.Load<Texture2D>("Graphics/Maps/WorldMap/Overlay/Water/DeepOcean"));
            WorldMapTextures.Add(Content.Load<Texture2D>("Graphics/Maps/WorldMap/Overlay/City/CityGround"));
            WorldMapTextures.Add(Content.Load<Texture2D>("Graphics/Maps/WorldMap/Overlay/City/CityWall"));
            WorldMapTextures.Add(Content.Load<Texture2D>("Graphics/Maps/WorldMap/Overlay/City/Harbour"));
            WorldMapTextures.Add(Content.Load<Texture2D>("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofStoneUp"));
            WorldMapTextures.Add(Content.Load<Texture2D>("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofStoneDown"));
            WorldMapTextures.Add(Content.Load<Texture2D>("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofStoneRight"));
            WorldMapTextures.Add(Content.Load<Texture2D>("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofStoneLeft"));
            WorldMapTextures.Add(Content.Load<Texture2D>("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofStoneCornerUpRight"));
            WorldMapTextures.Add(Content.Load<Texture2D>("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofMiddleVertical"));
            WorldMapTextures.Add(Content.Load<Texture2D>("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofMiddleHorisontal"));
            WorldMapTextures.Add(Content.Load<Texture2D>("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofMiddleCornerUpRight"));

            Texture2D[] MinimapAssets = new Texture2D[2]{
            Content.Load<Texture2D>("Graphics/Maps/WorldMap/Minimap"),
            Content.Load<Texture2D>("Graphics/Misc/Minimap/MinimapCursor")
            };

            //Load Character sprites
            World_Character wCharacter = new World_Character(Content.Load<Texture2D>("Graphics/People/PC/baseChar"));

            //Init Intro
            //Init Main Menu
            GSS.mainMenu.InitMenu(MainMenuBtn.ToArray(), MainMenuActiveBtn.ToArray());
            //Init In-game Menu
            GSS.ingameMenu.initMenu(Content.Load<Texture2D>("Menu/Ingame/Menu1"));
            //Init WorldMap
            WorldAutoElements autoElements = new WorldAutoElements();
            autoElements.Init(Content.Load<Texture2D>("Graphics/Items/Transportation/smallBoat"));
            GSS.world.InitTiles(WorldMapTextures.ToArray(), WorldMap, MinimapAssets, WorldClouds, wCharacter, autoElements);
            //Init Area
            //Init Battle
            //Init Scene

        }

        protected override void UnloadContent()
        {
        }
        bool down = false;
        protected override void Update(GameTime gameTime)
        {

            GSS.PadState = GamePad.GetState(PlayerIndex.One);
            GSS.KeyState = Keyboard.GetState();

            // Allows the game to exit (Debug direct control)
            if (GSS.PadState.Buttons.Start == ButtonState.Pressed || GSS.KeyState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            else if (GSS.KeyState.IsKeyDown(Keys.F1))
            {
                if (!down)
                {
                    GSS.DebugMode = !GSS.DebugMode;
                    down = true;
                }
            }
            else if (GSS.KeyState.IsKeyDown(Keys.F2))
            {
                if (!down)
                {
                    GSS.DebugEconomyMode = !GSS.DebugEconomyMode;
                    down = true;
                }
            }
            else if (down)
            {
                down = false;
            }
            switch (GSS.RState)
            {
                case RenderState.Intro:
                    GSS.intro.Update();
                    break;
                case RenderState.MainMenu:
                    GSS.mainMenu.Update();
                    break;
                case RenderState.IngameMenu:
                    GSS.ingameMenu.Update();
                    break;
                case RenderState.World:
                    GSS.world.Update(gameTime);
                    break;
                case RenderState.Scene:
                    GSS.scene.Update(gameTime);
                    break;
                case RenderState.Battle:
                    GSS.battle.Update(gameTime);
                    break;
                case RenderState.Area:

                    break;
            }
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            switch (GSS.RState)
            {
                case RenderState.Intro:
                    GSS.intro.Draw();
                    break;
                case RenderState.MainMenu:
                    GSS.mainMenu.Draw();
                    break;
                case RenderState.IngameMenu:
                    GSS.ingameMenu.Draw();
                    break;
                case RenderState.World:
                    GSS.world.Draw();
                    break;
                case RenderState.Scene:
                    GSS.scene.Draw();
                    break;
                case RenderState.Battle:
                    GSS.battle.Draw();
                    break;
                case RenderState.Area:

                    break;
            }

            GSS.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            GSS.spriteBatch.DrawString(GSS.baseFont,
                "State: " + GSS.RState.ToString() + " " + GSS.ExtraOutput,
                new Vector2(pst10Xangle, pst10Yangle),
                Color.Black);
            
            GSS.spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
