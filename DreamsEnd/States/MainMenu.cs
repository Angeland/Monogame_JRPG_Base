using Microsoft.Xna.Framework;
using DreamsEnd.Exceptions;
using DreamsEnd.Textures;
using System;

namespace DreamsEnd.States
{
    public class MainMenu : IDisposable
    {
        private IButton[] Buttons;
        private int activeButton = 0;

        public void InitMenu(IButton[] buttons)
        {
            Buttons = buttons;
        }

        public void Update()
        {
            if (GSS.Controlls.Confirm())
            {
                switch (activeButton)
                {
                    case 0: //Easy
                    case 1: //Medium
                        GSS.StartTime();
                        GSS.World.LoadWorld();
                        GSS.RenderState = RenderState.World;
                        break;
                    default:
                        throw new InvalidManuSelectionException(GSS.RenderState, activeButton);
                }

                GSS.StartTime();
                GSS.RenderState = RenderState.World;
                Dispose(true);
            }
            else if (GSS.Controlls.Up(true))
            {
                activeButton++;
                if (activeButton == Buttons.Length)
                {
                    activeButton = 0;
                }
            }
            else if (GSS.Controlls.Down(true))
            {
                activeButton--;
                if (activeButton == -1)
                {
                    activeButton = Buttons.Length - 1;
                }
            }

        }
        public void Draw()
        {
            GSS.SpriteBatch.Begin();
            for (int a = 0; a < Buttons.Length; a++)
            {
                IButton button = Buttons[a];
                GSS.SpriteBatch.Draw(button.GetTexture(activeButton == a), button.Rect, Color.White);
            }
            GSS.SpriteBatch.End();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Buttons != null)
                {
                    foreach (var item in Buttons)
                    {
                        item.Dispose();
                    }
                    Buttons = null;
                }
            }
        }
    }
}
