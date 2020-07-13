using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace RPG.Controlls
{
    class KeyboardControl : IControls, IDebugControls
    {
        private KeyboardState State => Keyboard.GetState();

        public bool AltConfirm()
        {
            return Clicked(Keys.R);
        }

        public bool Cancel()
        {
            return Clicked(Keys.F);
        }

        public bool Confirm()
        {
            return Clicked(Keys.E);
        }

        public bool Down(bool clickable = false)
        {
            if (clickable)
            {
                return Clicked(Keys.Down) || Clicked(Keys.S);
            }
            else
            {
                return Held(Keys.Down) || Held(Keys.S);
            }
        }
        public bool L1(bool clickable = false)
        {
            if (clickable)
            {
                return Clicked(Keys.Z);
            }
            else
            {
                return Held(Keys.Z);
            }
        }

        public bool L2(bool clickable = false)
        {
            if (clickable)
            {
                return Clicked(Keys.LeftAlt);
            }
            else
            {
                return Held(Keys.LeftAlt);
            }
        }

        public bool L3()
        {
            return Clicked(Keys.D2);
        }

        public bool Left(bool clickable = false)
        {
            if (clickable)
            {
                return Clicked(Keys.Left) || Clicked(Keys.A);
            }
            else
            {
                return Held(Keys.Left) || Held(Keys.A);
            }
        }

        public bool Menu()
        {
            return Clicked(Keys.Q);
        }

        public bool Option()
        {
            return Clicked(Keys.Tab);
        }

        public bool R1(bool clickable = false)
        {
            if (clickable)
            {
                return Clicked(Keys.C);
            }
            else
            {
                return Held(Keys.C);
            }
        }

        public bool R2(bool clickable = false)
        {
            if (clickable)
            {
                return Clicked(Keys.LeftControl);
            }
            else
            {
                return Held(Keys.LeftControl);
            }
        }

        public bool R3()
        {
            return Clicked(Keys.D1);
        }

        public bool Right(bool clickable = false)
        {
            if (clickable)
            {
                return Clicked(Keys.Right) || Clicked(Keys.D);
            }
            else
            {
                return Held(Keys.Right) || Held(Keys.D);
            }
        }

        public bool Start()
        {
            return Clicked(Keys.Escape);
        }

        public bool Up(bool clickable = false)
        {
            if (clickable)
            {
                return Clicked(Keys.Up) || Clicked(Keys.W);
            }
            else
            {
                return Held(Keys.Up) || Held(Keys.W);
            }
        }
        public bool EnableDebugMode()
        {
            return Clicked(Keys.F1);
        }
        public bool EnableDebugEconomyMode()
        {
            return Clicked(Keys.F2);
        }
        public bool EnableFreeCameraMode()
        {
            return Clicked(Keys.F3);
        }
        public bool DontRenderTilesOnX0OrY0()
        {
            return Clicked(Keys.F4);
        }
        public bool CameraFollowNext()
        {
            return Clicked(Keys.F10);
        }
        public bool EnableLogging()
        {
            return Clicked(Keys.F12);
        }
        private readonly Dictionary<Keys, bool> PendingClicks = new Dictionary<Keys, bool>();
        private bool Clicked(params Keys[] keys)
        {
            bool[] result = new bool[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                result[i] = false;
                Keys key = keys[i];
                bool isDown = State.IsKeyDown(key);
                if (!PendingClicks.ContainsKey(key))
                {
                    PendingClicks.Add(key, isDown);
                }
                else
                {
                    if (PendingClicks[key] && !isDown)
                    {
                        result[i] = true;
                    }
                    PendingClicks[key] = isDown;
                }
            }
            return !result.Contains(false);
        }
        private bool Held(params Keys[] keys)
        {
            bool[] result = new bool[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                Keys key = keys[i];
                result[i] = State.IsKeyDown(key);
            }
            return !result.Contains(false);
        }
    }
}
