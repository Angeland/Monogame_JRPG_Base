using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JRPG_Base.States;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace JRPG_Base
{
    public enum RenderState
    {
        Intro, MainMenu, IngameMenu, World,
        Area, Battle, Scene
    };
    public static class GSS
    {
        public static GraphicsDeviceManager graphics;
        public static Random globalRand = new Random();
        private static Stopwatch GameTimeOfDay = new Stopwatch();
        private static int GameTimeOffset = 0;
        public const float ClockSpeed = 1f;
        public static void stopTime()
        {
            GameTimeOfDay.Stop();
        }
        public static void startTime()
        {
            GameTimeOfDay.Start();
        }
        public static DateTime getTime()
        {
            return new DateTime(358, 9, 20, 12, 34, 9).AddSeconds((GameTimeOfDay.ElapsedMilliseconds + GameTimeOffset) / ClockSpeed);
        }
        public static void IncreaseTime(int hour, int minutes)
        {
            GameTimeOffset += (hour * 60 + minutes) * 60 * 1000;
        }
        public static float percentDay()
        {
            return ((float)(GSS.getTime().Minute + GSS.getTime().Hour * 60)) / (24f * 60f);
        }

        public static Color getSunPositionColor()
        {
            double td = Math.Cos(GSS.percentDay() * MathHelper.TwoPi) + 1;
            int timeColorValue = 255 - ((byte)((int)(td * 100)));
            return new Color(timeColorValue, timeColorValue, timeColorValue);
        }
        public static RenderState RState = RenderState.MainMenu;

        public static Intro intro = new Intro();
        public static Battle battle = new Battle();
        public static MainMenu mainMenu = new MainMenu();
        public static IngameMenu ingameMenu = new IngameMenu();
        public static World world = new World();
        public static Scene scene = new Scene();
        public static GameState dyn = new GameState();

        //Controller Input
        public static GamePadState PadState;
        public static KeyboardState KeyState;


        public static SpriteBatch spriteBatch;

        //GameDetails
        static int _screenHeight = 640, _screenWidth = 480;
        public static int CenterScreenTileOffsetX, CenterScreenTileOffsetY;
        public static int ScreenHeight
        {
            get
            {
                return _screenHeight;
            }
            set
            {
                _screenHeight = value;
                CenterScreenTileOffsetY = (int)(value / (float)(2 * GSS.TileSize));
            }
        }
        public static int ScreenWidth
        {
            get
            {
                return _screenWidth;
            }
            set
            {
                _screenWidth = value;
                CenterScreenTileOffsetX = (int)(value / (float)(2 * GSS.TileSize));
            }
        }

        //Camera zoom
        public const int Zoom = 5;
        //Map
        public static int TileSize = 16;
        public static Texture2D DefaultTex;
        public static SpriteFont baseFont;
        public static string ExtraOutput = "";

        /**********************************
         *    DEVELOPMENT MODIFICATIONS   *
         *             CHEATS             *
         *********************************/
        public static bool NoRandomBattle = true,
            AutoWinCombat = false,
            GodMode = false,
            AllItems = false,
            DebugMode = false,
            DebugEconomyMode = false,
            CameraMode = true;

        public static int MaxMoney = 99999999,
            Level = 99,
            hHP = 9999,
            hStr = 255, //Strenght (Physical-Damage)
            hDef = 255, //Defence (Physical-Defence)
            hDex = 255, //Dexterity ()
            hSpr = 255, //Spirit (Magic-Defence)
            hMag = 255, //Magic (Magic-Damage)
            hLck = 255, //Luck (Effect all in some way)
            hSpd = 255, //Speed (Attack Speed)
            hBlk = 255, //Block (Shield/Fist)
            hEvd = 255; //Evade (Evd/255 = Evd%)

    }
    public class GameState
    {

        public bool //Game Window Active
            MainMenu = false,
            LoadMenu = false,
            inGameMenu = false,
            inGameMenuLock = false,
            NewGame = false,
            Combat = false,
            Outside = false,
            Minimap = false;

        public int //Value Stat
            Money = 0,
            MoneySpent = 0,
            Reputation = 0;

        public int //Quest Stat
            ActiveQuest = 0,
            FinishedQuest = 0,
            AbandonedQuest = 0;

        public int //Battle Stat
            EscapeCount = 0,
            EnemiesDefeated = 0,
            CountGoldStolen = 0,
            CountItemStolen = 0;

        public int //Misc Stat
            ItemCount = 0,
            ItemCountBought = 0,
            ItemCountSold = 0,
            TotalStepCount = 0;

        /*
        //Game Mechanics
        public string getPostionString()
        {

            return "X:" + getCenterX() + " Y:" + getCenterY() +
                "\nX:" + getPosition().X + " Y:" + getPosition().Y +
                "\nX:" + CameraLocation.X + " Y:" + CameraLocation.Y;
        }*/

    }
}
