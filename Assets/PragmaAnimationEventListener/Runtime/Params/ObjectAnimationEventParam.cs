using System;
using Object = UnityEngine.Object;

namespace Pragma.AnimationEventListener
{
    [Serializable]
    public class ObjectAnimationEventParam : SimpleAnimationEventParam<Object>
    {
        public ObjectAnimationEventParam() : base()
        {
        }

        public ObjectAnimationEventParam(Object value) : base(value)
        {
        }
    }
}