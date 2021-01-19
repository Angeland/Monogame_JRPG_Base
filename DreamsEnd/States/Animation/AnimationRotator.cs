using Microsoft.Xna.Framework;
using DreamsEnd.States.DebugHelp;

namespace DreamsEnd.States.Animation
{
    public class AnimationRotator<T>
    {
        private readonly AnimationFunction function;
        private int activeIndex;
        private AnimationDirection direction;
        readonly float updateIntervall;
        float nextUpdateAfter = 0;
        readonly T[] ObjectCollection;
        public AnimationRotator(T[] objectCollection, AnimationFunction function, float playTimeSeconds, AnimationDirection direction = AnimationDirection.FORWARD, int startIndex = 0)
        {
            ObjectCollection = objectCollection;
            this.function = function;
            this.direction = direction;
            activeIndex = startIndex;
            updateIntervall = (playTimeSeconds * 1000) / ObjectCollection.Length;
        }

        internal int ActiveIndex()
        {
            return activeIndex;
        }

        public T Get()
        {
            return ObjectCollection[activeIndex];
        }
        internal void Update(GameTime gameTime)
        {
            DebugConsole.WriteLine($"{typeof(T)} Animator TotalGameTime {gameTime.TotalGameTime.TotalMilliseconds} > {nextUpdateAfter}");
            if (gameTime.TotalGameTime.TotalMilliseconds > nextUpdateAfter)
            {
                nextUpdateAfter += updateIntervall;
                NextIndex();
            }
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
                if (activeIndex == ObjectCollection.Length)
                    activeIndex = 0;
            }
            else
            {
                activeIndex--;
                if (activeIndex < 0)
                    activeIndex = ObjectCollection.Length - 1;
            }
        }

        private void BackwardForwardMotion()
        {
            if (AnimationDirection.FORWARD == direction)
            {
                if (activeIndex == ObjectCollection.Length - 1)
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
    }
}
