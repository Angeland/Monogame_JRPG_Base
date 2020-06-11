using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG_Base.States
{
    public class IngameMenu
    {
        public RenderState PrevState;
        public static Texture2D MenuTextures;

        public void openMenu()
        {
            PrevState = GSS.RState;
            GSS.RState = RenderState.IngameMenu;
        }
        public void closeMenu()
        {
            GSS.RState = PrevState;
        }
        public void initMenu(Texture2D menuTex)
        {
            MenuTextures = menuTex;
        }
        public void Update()
        {
            //Exit Menu
            if (GSS.PadState.Buttons.B == ButtonState.Pressed || GSS.KeyState.IsKeyDown(Keys.Q))
                closeMenu();

        }
        public void Draw()
        {
            int margin = (int)(GSS.ScreenWidth * 0.002);
            GSS.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            GSS.spriteBatch.Draw(GSS.DefaultTex, new Rectangle(margin, margin,
                (int)(GSS.ScreenWidth - margin),
                (int)(GSS.ScreenHeight - margin)), Color.White);

            //Main Quad
            GSS.spriteBatch.Draw(MenuTextures, new Rectangle(margin, margin,
                (int)(GSS.ScreenWidth * 0.75),
                (int)(GSS.ScreenHeight - margin)), Color.White);

            //List Quad
            GSS.spriteBatch.Draw(MenuTextures, new Rectangle((int)(GSS.ScreenWidth * 0.76), margin,
                (int)(GSS.ScreenWidth * 0.24),
                (int)(GSS.ScreenHeight * 0.8)), Color.White);

            //Info Quad
            GSS.spriteBatch.Draw(MenuTextures, new Rectangle((int)(GSS.ScreenWidth * 0.76), (int)(GSS.ScreenHeight * 0.81) + margin,
                (int)(GSS.ScreenWidth * 0.24),
                (int)(GSS.ScreenHeight * 0.190) - margin), Color.White);
            GSS.spriteBatch.End();
        }
    }
}
