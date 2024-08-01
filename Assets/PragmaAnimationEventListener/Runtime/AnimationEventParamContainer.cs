using System.Collections.Generic;
using UnityEngine;

namespace Pragma.AnimationEventListener
{
    [CreateAssetMenu(fileName = nameof(AnimationEventParamContainer), menuName = "Configs/Game/AnimationEvent/" + nameof(AnimationEventParamContainer))]
    public sealed class AnimationEventParamContainer : ScriptableObject
    {
        [SerializeReference] private List<IAnimationEventParam> _params;

        public IReadOnlyList<IAnimationEventParam> Params => _params;

        public T ReadParam<T>() where T : IAnimationEventParam
        {
            TryReadParam(out T value);

            return value;
        }

        public bool TryReadParam<T>(out T value) where T : IAnimationEventParam
        {
            foreach (var param in _params)
            {
                if(param is T readValue)
                {
                    value = readValue;
                    return true;
                }
            }

            value = default;
            return false;
        }

#if UNITY_EDITOR
        public void WriteParam<T>(T param) where T : IAnimationEventParam
        {
            _params.Add(param);
        }
#endif
    }
}