using System;

namespace RPG.Exceptions
{
    public class InvalidManuSelectionException : Exception
    {
        public InvalidManuSelectionException(RenderState renderState, int activeButton) :
            base($"For renderstate:{renderState} the selection index {activeButton} is invalid")
        {
        }
    }
}
