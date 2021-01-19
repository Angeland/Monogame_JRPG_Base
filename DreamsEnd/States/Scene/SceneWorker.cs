using Microsoft.Xna.Framework;
using DreamsEnd.Library;
using DreamsEnd.States.DebugHelp;
using DreamsEnd.States.Scene.SceneRigging;
using DreamsEnd.States.World;
using System.Collections.Generic;

namespace DreamsEnd.States.Scene
{
    public static class SceneWorker
    {
        private static readonly SceneRigger rigger = new SceneRigger();
        public static Scenes ActiveScene { get; private set; } = Scenes.WORLD;
        public static Dictionary<Scenes, Texture2DColors> TransitionMaps { get; internal set; }

        public static readonly Dictionary<Color, Scenes> sceneTriggers = new Dictionary<Color, Scenes>();

        public static void AttemptSceneChange(Vector2 position)
        {
            if (TransitionMaps.ContainsKey(ActiveScene))
            {
                Color sceneValue = TransitionMaps[ActiveScene].GetColor((int)position.X, (int)position.Y);

                if (sceneTriggers.ContainsKey(sceneValue))
                {
                    ActiveScene = sceneTriggers[sceneValue];
                    rigger.BuildScene(ActiveScene);
                }
                else
                {
                    ActiveScene = Scenes.WORLD;
                }
            }
            DebugConsole.WriteLine($"Active Scene:{ActiveScene}");
        }

        static SceneWorker()
        {
            sceneTriggers.Add(new Color(1, 0, 0), Scenes.CITY_ONE);

        }
    }
}
