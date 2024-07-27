using System;

namespace Pragma.AnimationEventListener
{
    [Serializable]
    public class IntAnimationEventParam : SimpleAnimationEventParam<int>
    {
        public IntAnimationEventParam() : base()
        {
        }

        public IntAnimationEventParam(int value) : base(value)
        {
        }
    }
}