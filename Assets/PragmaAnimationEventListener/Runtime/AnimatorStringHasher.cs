using UnityEngine;

namespace Pragma.AnimationEventListener
{
    [CreateAssetMenu(fileName = nameof(AnimatorStringHasher), menuName = "Configs/Game/AnimationEvent/" + nameof(AnimatorStringHasher))]
    public class AnimatorStringHasher : ScriptableStringHasher
    {
        public override int GetHash(string value)
        {
            return Animator.StringToHash(value);
        }
    }
}