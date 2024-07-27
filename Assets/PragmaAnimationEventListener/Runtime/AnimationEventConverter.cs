using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Pragma.AnimationEventListener
{
    [CreateAssetMenu(fileName = nameof(AnimationEventConverter), menuName = "Configs/Game/AnimationEvent/" + nameof(AnimationEventConverter))]
    public sealed class AnimationEventConverter : ScriptableObject
    {
#if UNITY_EDITOR
        public const string FUNCTION_NAME = "OnAnimationEvent";
        
        public ScriptableStringHasher hasher;
        public AnimationEventParamContainer container;

        public void Convert(AnimationEvent @event)
        {
            var funcName = @event.functionName;
            var intParam = @event.intParameter;
            var floatParam = @event.floatParameter;
            var objectParam = @event.objectReferenceParameter;
            var stringParam = @event.stringParameter;

            @event.functionName = FUNCTION_NAME;
            @event.floatParameter = 0;
            @event.stringParameter = funcName;
            @event.intParameter = hasher != null ? hasher.GetHash(funcName) : Animator.StringToHash(funcName);
            @event.objectReferenceParameter = container;

            if (container == null)
            {
                return;
            }
            
            if (intParam != 0 )
            {
                container.WriteParam(new IntAnimationEventParam(intParam));
            }

            if (!string.IsNullOrWhiteSpace(stringParam))
            {
                container.WriteParam(new StringAnimationEventParam(stringParam));
            }

            if (floatParam != 0f)
            {
                container.WriteParam(new FloatAnimationEventParam(floatParam));
            }

            if (objectParam != null)
            {
                container.WriteParam(new ObjectAnimationEventParam(objectParam));
            }

            EditorUtility.SetDirty(container);
            AssetDatabase.SaveAssetIfDirty(container);
        }
        
        public void Convert(AnimationClip animationClip)
        {
            var events = AnimationUtility.GetAnimationEvents(animationClip);
            
            foreach (var @event in events)
            {
                Convert(@event);
            }

            AnimationUtility.SetAnimationEvents(animationClip, events);
            EditorUtility.SetDirty(animationClip);
            AssetDatabase.SaveAssetIfDirty(animationClip);
        }
        
        public void Convert(AnimatorController animatorController)
        {
        }
        
        public void Convert(Object asset)
        {
        }
#endif
    }
}