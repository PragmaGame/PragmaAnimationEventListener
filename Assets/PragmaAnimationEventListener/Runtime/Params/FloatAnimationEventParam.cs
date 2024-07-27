using System;

namespace Pragma.AnimationEventListener
{
    [Serializable]
    public class FloatAnimationEventParam : SimpleAnimationEventParam<float>
    {
        public FloatAnimationEventParam() : base()
        {
        }

        public FloatAnimationEventParam(float value) : base(value)
        {
        }
    }
}