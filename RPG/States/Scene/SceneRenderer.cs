﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RPG.Library;
using RPG.States.Characters.Individuals;
using RPG.States.Configuration;
using RPG.States.Scene;
using RPG.States.Scene.SceneRigging;

namespace RPG.States
{
    public abstract class SceneRenderer
    {
        private readonly SceneCharacter sceneCharacter;
        private readonly Texture2DColors cityTilemap;

        protected SceneRenderer(SceneCharacter sceneCharacter, Texture2DColors cityTilemap)
        {
            this.cityTilemap = cityTilemap;
            this.sceneCharacter = sceneCharacter;
        }

        internal void Update(GameTime gameTime)
        {
            SceneWorker.AttemptSceneChange(sceneCharacter.GetTilePosition());
        }

        internal void Draw()
        {
            GSS.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.DepthRead, RasterizerState.CullNone);

            for (int x = 0; x < cityTilemap.Width; x++)
            {
                for (int y = 0; y < cityTilemap.Height; y++)
                {
                    uint col = cityTilemap.GetColor(x, y).PackedValue;
                    if (SceneMaps.Tiles.ContainsKey(col))
                    {
                        GSS.SpriteBatch.Draw(SceneMaps.Tiles[col],
                            new Rectangle(x * EngineSettings.TileSize,
                            y * EngineSettings.TileSize,
                            EngineSettings.TileSize,
                            EngineSettings.TileSize),
                            GSS.GetSunPositionColor());
                    }
                }
            }
            GSS.SpriteBatch.End();
        }
    }
}
