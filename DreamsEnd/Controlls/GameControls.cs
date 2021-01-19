using DreamsEnd.States.DebugHelp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DreamsEnd.Controlls
{
    public class GameControls : IDebugControls
    {
        enum ButtonStates
        {
            Down, Up, HeldDown
        }
        private readonly IDictionary<Keys, bool> PendingClicks = new Dictionary<Keys, bool>();
        private readonly Dictionary<Buttons, ButtonStates> AwaitingRelease = new Dictionary<Buttons, ButtonStates>();
        private KeyboardState Keyboard => Microsoft.Xna.Framework.Input.Keyboard.GetState();
        private GamePadState State => GamePad.GetState(PlayerIndex.One);

        public bool AltConfirm()
        {
            return GamePadClick(Buttons.X, State.Buttons.X) || Clicked(Keys.R);
        }

        private bool GamePadClick(Buttons button, ButtonState state)
        {
            if (!AwaitingRelease.ContainsKey(button))
            {
                AwaitingRelease.Add(button, ButtonStates.Up);
            }
            DebugConsole.WriteLine($"Button {button}={AwaitingRelease[button]}");
            if (AwaitingRelease[button] == ButtonStates.Up && state == ButtonState.Pressed)
            {
                AwaitingRelease[button] = ButtonStates.Down;
            }
            else if (AwaitingRelease[button] != ButtonStates.Up && state == ButtonState.Pressed)
            {
                AwaitingRelease[button] = ButtonStates.HeldDown;
            }
            else
            {
                AwaitingRelease[button] = ButtonStates.Up;
            }
            return AwaitingRelease[button] == ButtonStates.Down;
        }

        public bool Cancel()
        {
            return GamePadClick(Buttons.B, State.Buttons.B) || Clicked(Keys.F);
        }

        public bool Confirm()
        {
            return GamePadClick(Buttons.A, State.Buttons.A) || Clicked(Keys.E);
        }

        public bool L1(bool clickable = false)
        {
            if (State.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                return true;
            }
            if (clickable)
            {
                return Clicked(Keys.Z);
            }
            return Held(Keys.Z);
        }

        public bool L2(bool clickable = false)
        {
            if (State.Triggers.Left > 0.3)
            {
                return true;
            }
            if (clickable)
            {
                return Clicked(Keys.LeftAlt);
            }
            return Held(Keys.LeftAlt);

        }

        public bool L3()
        {
            return GamePadClick(Buttons.LeftStick, State.Buttons.LeftStick) || Clicked(Keys.D2);
        }

        public bool Menu()
        {
            return GamePadClick(Buttons.Y, State.Buttons.Y) || Clicked(Keys.Q);
        }

        public bool Option()
        {
            return GamePadClick(Buttons.Back, State.Buttons.Back) || Clicked(Keys.Tab);
        }

        public bool R1(bool clickable = false)
        {
            if (State.Buttons.RightShoulder == ButtonState.Pressed)
            {
                return true;
            }
            if (clickable)
            {
                return Clicked(Keys.C);
            }
            return Held(Keys.C);
        }

        public bool R2(bool clickable = false)
        {
            if (State.Triggers.Right > 0.3)
            {
                return true;
            }
            if (clickable)
            {
                return Clicked(Keys.LeftControl);
            }
            return Held(Keys.LeftControl);
        }

        public bool R3()
        {
            return GamePadClick(Buttons.RightStick, State.Buttons.RightStick) || Clicked(Keys.D1);
        }


        public bool Start()
        {
            return GamePadClick(Buttons.Start, State.Buttons.Start) || Clicked(Keys.Escape);
        }

        public bool Right(bool clickable = false)
        {
            if (State.DPad.Right == ButtonState.Pressed)
            {
                return true;
            }
            if (State.ThumbSticks.Left.X > 0.1)
            {
                return true;
            }
            if (clickable)
            {
                return Clicked(Keys.Right) || Clicked(Keys.D);
            }
            return Held(Keys.Right) || Held(Keys.D);
        }

        public bool Left(bool clickable = false)
        {
            if (State.DPad.Left == ButtonState.Pressed)
            {
                return true;
            }
            if (State.ThumbSticks.Left.X < -0.1)
            {
                return true;
            }
            if (clickable)
            {
                return Clicked(Keys.Left) || Clicked(Keys.A);
            }
            return Held(Keys.Left) || Held(Keys.A);
        }

        public bool Up(bool clickable = false)
        {
            if (State.DPad.Up == ButtonState.Pressed)
            {
                return true;
            }
            if (State.ThumbSticks.Left.Y > 0.1)
            {
                return true;
            }
            if (clickable)
            {
                return Clicked(Keys.Up) || Clicked(Keys.W);
            }
            return Held(Keys.Up) || Held(Keys.W);
        }

        public bool Down(bool clickable = false)
        {
            if (State.DPad.Down == ButtonState.Pressed)
            {
                return true;
            }
            if (State.ThumbSticks.Left.Y < -0.1)
            {
                return true;
            }
            if (clickable)
            {
                return Clicked(Keys.Down) || Clicked(Keys.S);
            }

            return Held(Keys.Down) || Held(Keys.S);
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
        private bool Clicked(params Keys[] keys)
        {
            bool[] result = new bool[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                result[i] = false;
                Keys key = keys[i];
                bool isDown = Keyboard.IsKeyDown(key);
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
                result[i] = Keyboard.IsKeyDown(key);
            }
            return !result.Contains(false);
        }
    }
}
