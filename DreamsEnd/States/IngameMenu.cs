using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DreamsEnd.States.Configuration;

namespace DreamsEnd.States
{
    public class IngameMenu
    {
        public RenderState PrevState;
        public static Texture2D MenuTextures;

        public void OpenMenu()
        {
            PrevState = GSS.RenderState;
            GSS.RenderState = RenderState.IngameMenu;
        }
        public void CloseMenu()
        {
            GSS.RenderState = PrevState;
        }
        public void InitMenu(Texture2D menuTex)
        {
            MenuTextures = menuTex;
        }
        public void Update()
        {
            //Exit Menu
            if (GSS.Controlls.Cancel())
            {
                CloseMenu();
            }
        }
        public void Draw()
        {
            int margin = (int)(DisplayOutputSettings.ScreenWidth * 0.002);
            GSS.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            GSS.SpriteBatch.Draw(GSS.DefaultTex, new Rectangle(margin, margin,
                DisplayOutputSettings.ScreenWidth - margin,
                DisplayOutputSettings.ScreenHeight - margin), Color.White);

            //Main Quad
            GSS.SpriteBatch.Draw(MenuTextures, new Rectangle(margin, margin,
                (int)(DisplayOutputSettings.ScreenWidth * 0.75),
                DisplayOutputSettings.ScreenHeight - margin), Color.White);

            //List Quad
            GSS.SpriteBatch.Draw(MenuTextures, new Rectangle((int)(DisplayOutputSettings.ScreenWidth * 0.76), margin,
                (int)(DisplayOutputSettings.ScreenWidth * 0.24),
                (int)(DisplayOutputSettings.ScreenHeight * 0.8)), Color.White);

            //Info Quad
            GSS.SpriteBatch.Draw(MenuTextures, new Rectangle((int)(DisplayOutputSettings.ScreenWidth * 0.76), (int)(DisplayOutputSettings.ScreenHeight * 0.81) + margin,
                (int)(DisplayOutputSettings.ScreenWidth * 0.24),
                (int)(DisplayOutputSettings.ScreenHeight * 0.190) - margin), Color.White);
            GSS.SpriteBatch.End();
        }
    }
}
