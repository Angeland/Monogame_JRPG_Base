using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RPG.Controlls;
using RPG.States;
using RPG.States.DebugEnums;
using RPG.Textures;
using System;
using System.Diagnostics;

namespace RPG
{
    public enum RenderState
    {
        Intro, MainMenu, IngameMenu, World,
        Area, Battle, Scene
    };
    public static class GSS
    {
        public static Random GlobalRand { get; set; } = new Random();
        private static readonly Stopwatch GameTimeOfDay = new Stopwatch();
        private static int GameTimeOffset = 0;
        public const float ClockSpeed = 1f;
        public static void StopTime()
        {
            GameTimeOfDay.Stop();
        }
        public static void StartTime()
        {
            GameTimeOfDay.Start();
        }
        public static DateTime GetTime()
        {
            return new DateTime(358, 9, 20, 12, 34, 9).AddSeconds((GameTimeOfDay.ElapsedMilliseconds + GameTimeOffset) / ClockSpeed);
        }
        public static void IncreaseTime(int hour, int minutes)
        {
            GameTimeOffset += (hour * 60 + minutes) * 60 * 1000;
        }
        public static float PercentDay()
        {
            return (GetTime().Minute + GetTime().Hour * 60) / (24f * 60f);
        }

        public static Color GetSunPositionColor()
        {
            double td = Math.Cos(PercentDay() * MathHelper.TwoPi) + 1;
            int timeColorValue = 255 - ((byte)((int)td * 100));
            return new Color(timeColorValue, timeColorValue, timeColorValue);
        }

        public static TextureLoader TextureLoader { get; set; }
        public static RenderState RenderState { get; set; } = RenderState.MainMenu;
        public static Intro Intro { get; set; } = new Intro();
        public static Battle Battle { get; set; } = new Battle();
        public static MainMenu MainMenu { get; set; } = new MainMenu();
        public static IngameMenu IngameMenu { get; set; } = new IngameMenu();
        public static WorldRenderer World { get; set; } = new WorldRenderer();
        public static SceneRenderer ActiveScene { get; set; }
        public static GameState Dyn { get; set; } = new GameState();

        //Controller Input
        public static IDebugControls DebugControls { get; set; } = new KeyboardControl();
        public static IControls Controlls { get; set; } = DebugControls;//= new GamePadControl();

        public static SpriteBatch SpriteBatch { get; set; }

        //GameDetails
        public static CameraOverride FollowOverride { get; set; } = CameraOverride.NORMAL;

        //Camera zoom
        public const int Zoom = 5;
        //Map
        public static Texture2D DefaultTex { get; set; }
        public static SpriteFont BaseFont { get; set; }
        public static string ExtraOutput { get; internal set; }

        /**********************************
         *    DEVELOPMENT MODIFICATIONS   *
         *             CHEATS             *
         *********************************/
        public static bool NoRandomBattle = true,
            AutoWinCombat = false,
            GodMode = false,
            AllItems = false;
        public static bool DebugMode { get; internal set; } = false;
        public static bool DebugEconomyMode { get; internal set; } = false;
        public static bool FreeCameraMode { get; internal set; } = false;
        public static bool LoggingEnabeled { get; internal set; } = false;

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
