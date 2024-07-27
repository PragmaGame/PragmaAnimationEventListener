using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Pragma.AnimationEventListener
{
    public abstract class ScriptableStringHasher : ScriptableObject, IStringHasher
    {
        public abstract int GetHash(string value);

#if UNITY_EDITOR
        public void LogHash(string value)
        {
            Debug.Log($"Hash {value} = {GetHash(value)}");
        }
        
        public void DoHash(AnimationClip animationClip)
        {
            var events = AnimationUtility.GetAnimationEvents(animationClip);
            
            foreach (var @event in events)
            {
                @event.intParameter = GetHash(@event.stringParameter);
            }

            AnimationUtility.SetAnimationEvents(animationClip, events);
            
            EditorUtility.SetDirty(animationClip);
            AssetDatabase.SaveAssetIfDirty(animationClip);
        }
        
        public void DoHash(AnimatorController animatorController)
        {
            foreach (var animationClip in animatorController.animationClips)
            {
                DoHash(animationClip);
            }
            
            EditorUtility.SetDirty(animatorController);
            AssetDatabase.SaveAssetIfDirty(animatorController);
        }
        
        public void DoHash(Object asset)
        {
            var assetPath = AssetDatabase.GetAssetPath(asset);
            var subAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);

            foreach (var subAsset in subAssets)
            {
                if (subAsset is AnimationClip clip)
                {
                    DoHash(clip);
                }
            }
            
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssetIfDirty(asset);
        }
#endif
    }
}