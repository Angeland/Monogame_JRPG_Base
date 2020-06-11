using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG_Base.States
{
    public class MainMenu
    {
        Texture2D[] ButtonTextures;
        Texture2D[] ActiveButtonTextures;
        Rectangle[] ButtonRect;

        public void InitMenu(Texture2D[] Buttons, Texture2D[] ActiveButtons)
        {
            ButtonTextures = Buttons;
            ActiveButtonTextures = ActiveButtons;
            ButtonRect = new Rectangle[ButtonTextures.Length];
            int buttonHeight = ButtonTextures[0].Height;
            int buttonWidth = ButtonTextures[0].Width;
            int BorderBottom = 10;
            for (int a = 0; a < ButtonTextures.Length; a++)
            {
                ButtonRect[a] = new Rectangle(
                    (int)(GSS.ScreenWidth / 2f) - (int)(buttonWidth / 2f),
                    (int)(GSS.ScreenHeight / 2f) - (int)(((buttonHeight + BorderBottom) * ButtonTextures.Length) / 2f),
                    buttonWidth, buttonHeight);
            }

        }
        int activeButton = 0;
        public void Update()
        {
            if (GSS.KeyState.IsKeyDown(Keys.Enter))
            {
                GSS.startTime();
                GSS.RState = RenderState.World;
            }
        }
        public void Draw()
        {
            GSS.spriteBatch.Begin();
            for (int a = 0; a < ButtonTextures.Length; a++)
            {
                if (activeButton == a)
                {
                    GSS.spriteBatch.Draw(ButtonTextures[a], ButtonRect[a], Color.White);
                }
                else
                {
                    GSS.spriteBatch.Draw(ActiveButtonTextures[a], ButtonRect[a], Color.White);
                }
            }
            GSS.spriteBatch.End();
        }
    }
}
