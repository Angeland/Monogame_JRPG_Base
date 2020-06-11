using Microsoft.Xna.Framework;
using RPG.States.DebugHelp;

namespace RPG.States.Animation
{
    public class AnimationRotator
    {
        private AnimationFunction function;
        private readonly int maxIndex;
        private int activeIndex;
        private AnimationDirection direction;
        readonly float updateIntervall;
        float nextUpdateAfter = 0;
        public AnimationRotator(AnimationFunction function, int maxIndex, float playTimeSeconds, AnimationDirection direction = AnimationDirection.FORWARD, int startIndex = 0)
        {
            this.function = function;
            this.maxIndex = maxIndex;
            this.direction = direction;
            activeIndex = startIndex;
            updateIntervall = (playTimeSeconds * 1000) / maxIndex;
        }

        internal int ActiveIndex()
        {
            return activeIndex;
        }

        private void NextIndex()
        {
            switch (function)
            {
                case AnimationFunction.FORWARD_BACKWARD:
                    BackwardForwardMotion();
                    break;
                case AnimationFunction.LOOP:
                    LoopMotion();
                    break;
            }
        }

        private void LoopMotion()
        {
            if (AnimationDirection.FORWARD == direction)
            {
                activeIndex++;
                if (activeIndex == maxIndex)
                    activeIndex = 0;
            }
            else
            {
                activeIndex--;
                if (activeIndex < 0)
                    activeIndex = maxIndex - 1;
            }
        }

        private void BackwardForwardMotion()
        {
            if (AnimationDirection.FORWARD == direction)
            {
                if (activeIndex == maxIndex - 1)
                {
                    direction = AnimationDirection.BACKWARD;
                    BackwardForwardMotion();
                }
                else
                {
                    activeIndex++;
                }
            }
            else
            {
                if (activeIndex == 0)
                {
                    direction = AnimationDirection.FORWARD;
                    BackwardForwardMotion();
                }
                else
                {
                    activeIndex--;
                }
            }
        }

        internal void Update(GameTime gameTime)
        {
            DebugConsole.WriteLine($"TotalGameTime {gameTime.TotalGameTime.TotalMilliseconds} > {nextUpdateAfter}");
            if (gameTime.TotalGameTime.TotalMilliseconds > nextUpdateAfter)
            {
                nextUpdateAfter += updateIntervall;
                NextIndex();
            }
        }
    }
}
