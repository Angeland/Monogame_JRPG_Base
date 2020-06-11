using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RPG.Controlls
{
    class GamePadControl : IControls
    {
        private GamePadState State => GamePad.GetState(PlayerIndex.One);

        public bool AltConfirm()
        {
            return State.Buttons.X == ButtonState.Pressed;
        }

        public bool Cancel()
        {
            return State.Buttons.B == ButtonState.Pressed;
        }

        public bool Confirm()
        {
            return State.Buttons.A == ButtonState.Pressed;
        }

        public bool Down(bool clickable = false)
        {
            return State.DPad.Down == ButtonState.Pressed;
        }

        public bool L1(bool clickable = false)
        {
            return State.Buttons.LeftShoulder == ButtonState.Pressed;
        }

        public bool L2(bool clickable = false)
        {
            return State.Triggers.Left > 10;
        }

        public bool L3()
        {
            return State.Buttons.LeftStick == ButtonState.Pressed;
        }

        public bool Left(bool clickable = false)
        {
            return State.DPad.Left == ButtonState.Pressed;
        }

        public bool Menu()
        {
            return State.Buttons.Y == ButtonState.Pressed;
        }

        public bool Option()
        {
            return State.Buttons.Back == ButtonState.Pressed;
        }

        public bool R1(bool clickable = false)
        {
            return State.Buttons.RightShoulder == ButtonState.Pressed;
        }

        public bool R2(bool clickable = false)
        {
            return State.Triggers.Right > 10;
        }

        public bool R3()
        {
            return State.Buttons.RightStick == ButtonState.Pressed;
        }

        public bool Right(bool clickable = false)
        {
            return State.DPad.Right == ButtonState.Pressed;
        }

        public bool Start()
        {
            return State.Buttons.Start == ButtonState.Pressed;
        }

        public bool Up(bool clickable = false)
        {
            return State.DPad.Up == ButtonState.Pressed;
        }
    }
}
