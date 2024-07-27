using System;
using UnityEngine;

namespace Pragma.AnimationEventListener
{
    [Serializable]
    public abstract class SimpleAnimationEventParam<TValue> : IAnimationEventParam
    {
        protected SimpleAnimationEventParam()
        {
        }

        protected SimpleAnimationEventParam(TValue value)
        {
            Value = value;
        }
        
        [field: SerializeField] public TValue Value { get; private set; }
    }
}