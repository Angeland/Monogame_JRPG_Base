using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RPG.Exceptions;
using RPG.Helper;
using RPG.Library;
using RPG.States.AutoElements;
using RPG.States.Characters;
using RPG.States.Configuration;
using RPG.States.DebugHelp;
using RPG.States.Scene;
using RPG.States.Scene.SceneRigging;
using RPG.States.World;
using RPG.Textures;
using System;
using System.Collections.Generic;

namespace RPG
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private float _consolePrintXPosition;
        private float _consolePrintYPosition;
        private readonly FrameCounter FrameCounter = new FrameCounter();

        public Game1()
            : base()
        {
            GraphicsSettings.GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = @"D:\JRPG_Base\JRPG_Base\JRPG_Base\Content";
            Content.RootDirectory = "Content";
            IsFixedTimeStep = false; //Låst fps
            GSS.TextureLoader = new TextureLoader(Content);
        }

        private void InitGraphicsMode()
        {
            DisplayOutputSettings.DisplayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            GraphicsSettings.SetPreferredBackBuffer(DisplayOutputSettings.ScreenWidth, DisplayOutputSettings.ScreenHeight);
            GraphicsSettings.SetFullscreen(DisplayOutputSettings.isFullScreen);
            GraphicsSettings.ApplyChanges();
        }

        protected override void Initialize()
        {
            _consolePrintXPosition = DisplayOutputSettings.ScreenWidth * 0.1f;
            _consolePrintYPosition = DisplayOutputSettings.ScreenHeight * 0.1f;
            IsMouseVisible = true;
            InitGraphicsMode();
            base.Initialize();
        }
        protected override void LoadContent()
        {
            GSS.SpriteBatch = new SpriteBatch(GraphicsDevice);

            //Font Loading
            GSS.BaseFont = Content.Load<SpriteFont>("Fonts/Arial");

            SceneWorker.TransitionMaps = new Dictionary<Scenes, Texture2DColors>()
            {
                { Scenes.WORLD, new Texture2DColors(Content.Load<Texture2D>("Graphics/Maps/WorldMap/SceneTransitionWorldMap"))}
            };



            //Scenes
            SceneMaps.AllScenes = new Dictionary<Scenes, Texture2DColors>()
            {
                { Scenes.CITY_ONE, new Texture2DColors(Content.Load<Texture2D>("Graphics/Maps/Scenes/CityOne/BaseMap")) }
            };

            //Scene tiles
            SceneMaps.Tiles = new Dictionary<uint, Texture2D>()
            {
                { new Color(255,127,39).PackedValue, Content.Load<Texture2D>("Graphics/Maps/Scenes/Overlay/Ground/Road1") },
                { new Color(34, 177,76).PackedValue, Content.Load<Texture2D>("Graphics/Maps/Scenes/Overlay/Ground/Grass1") }
            };


            GSS.DefaultTex = Content.Load<Texture2D>("Graphics/Maps/WorldMap/Minimap");
            //Load Character sprites

            //Init Intro
            //Init Main Menu
            GSS.MainMenu.InitMenu(GSS.TextureLoader.MainMenu());
            //Init In-game Menu
            GSS.IngameMenu.InitMenu(Content.Load<Texture2D>("Menu/Ingame/Menu1"));

        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            FrameCounter.Update(gameTime);
            DebugConsole.Reset();
            DebugKeyPress();

            if (GSS.Controlls.Start())
            {
                Exit();
            }

            switch (GSS.RenderState)
            {
                case RenderState.Intro:
                    GSS.Intro.Update();
                    break;
                case RenderState.MainMenu:
                    GSS.MainMenu.Update();
                    break;
                case RenderState.IngameMenu:
                    GSS.IngameMenu.Update();
                    break;
                case RenderState.World:
                    GSS.World.Update(gameTime);
                    break;
                case RenderState.Scene:
                    GSS.ActiveScene.Update(gameTime);
                    break;
                case RenderState.Battle:
                    GSS.Battle.Update(gameTime);
                    break;
                case RenderState.Area:

                    break;
            }
            base.Update(gameTime);
        }

        private void DebugKeyPress()
        {
            if (GSS.DebugControls.EnableDebugMode())
            {
                GSS.DebugMode = !GSS.DebugMode;
            }
            if (GSS.DebugControls.EnableDebugEconomyMode())
            {
                GSS.DebugEconomyMode = !GSS.DebugEconomyMode;
            }
            if (GSS.DebugControls.EnableFreeCameraMode())
            {
                GSS.FreeCameraMode = !GSS.FreeCameraMode;
            }
            if (GSS.DebugControls.EnableLogging())
            {
                GSS.LoggingEnabeled = !GSS.LoggingEnabeled;
            }
            if (GSS.DebugControls.CameraFollowNext())
            {
                GSS.FollowOverride = GSS.FollowOverride.Next();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            switch (GSS.RenderState)
            {
                case RenderState.Intro:
                    GSS.Intro.Draw();
                    break;
                case RenderState.MainMenu:
                    GSS.MainMenu.Draw();
                    break;
                case RenderState.IngameMenu:
                    GSS.IngameMenu.Draw();
                    break;
                case RenderState.World:
                    GSS.World.Draw();
                    break;
                case RenderState.Scene:
                    GSS.ActiveScene.Draw();
                    break;
                case RenderState.Battle:
                    GSS.Battle.Draw();
                    break;
                case RenderState.Area:

                    break;
            }

            GSS.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            if (GSS.LoggingEnabeled)
            {
                DebugConsole.WriteLine($"FPS: {FrameCounter.AverageFramesPerSecond}");
                GSS.SpriteBatch.DrawString(GSS.BaseFont,
                    "State: " + GSS.RenderState.ToString() + Environment.NewLine + DebugConsole.Read(),
                    new Vector2(_consolePrintXPosition, _consolePrintYPosition),
                    Color.Orange);
            }

            GSS.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}