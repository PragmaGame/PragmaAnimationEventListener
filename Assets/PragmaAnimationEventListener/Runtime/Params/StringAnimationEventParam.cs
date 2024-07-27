using System;

namespace Pragma.AnimationEventListener
{
    [Serializable]
    public class StringAnimationEventParam : SimpleAnimationEventParam<string>
    {
        public StringAnimationEventParam() : base()
        {
        }

        public StringAnimationEventParam(string value) : base(value)
        {
        }
    }
}