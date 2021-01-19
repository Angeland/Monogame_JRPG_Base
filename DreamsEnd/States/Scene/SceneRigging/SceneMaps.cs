using Microsoft.Xna.Framework.Graphics;
using DreamsEnd.Library;
using DreamsEnd.States.World;
using System.Collections.Generic;

namespace DreamsEnd.States.Scene.SceneRigging
{
    public static class SceneMaps
    {
        public static Dictionary<Scenes, Texture2DColors> AllScenes { get; internal set; }
        public static Dictionary<uint, Texture2D> Tiles { get; internal set; }
    }
}
