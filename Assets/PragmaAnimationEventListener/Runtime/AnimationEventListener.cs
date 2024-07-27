using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Scripting;
using Object = UnityEngine.Object;

namespace Pragma.AnimationEventListener
{
    public class AnimationEventListener : MonoBehaviour
    {
        [Tooltip("If null used [Animator.StringToHash]")]
        [SerializeField] private ScriptableStringHasher _hasher;
        
        private readonly Dictionary<int, Action<AnimationEventParamContainer>> _map = new();
        private readonly Dictionary<string, int> _hashMap = new();

        public void Subscribe(int hashedKey, Action<AnimationEventParamContainer> action)
        {
            if (_map.ContainsKey(hashedKey))
            {
                _map[hashedKey] += action;
            }
            else
            {
                _map.Add(hashedKey, action);
            }
        }

        public void Unsubscribe(int hashedKey, Action<AnimationEventParamContainer> action)
        {
            if (!_map.ContainsKey(hashedKey))
            {
                return;
            }

            _map[hashedKey] -= action;
        }
        
        public void Subscribe(string key, Action<AnimationEventParamContainer> action)
        {
            if (!_hashMap.TryGetValue(key, out var hashedKey))
            {
                hashedKey = _hasher != null ? _hasher.GetHash(key) : Animator.StringToHash(key);
                _hashMap.Add(key, hashedKey);
            }

            Subscribe(hashedKey, action);
        }

        public void Unsubscribe(string key, Action<AnimationEventParamContainer> action)
        {
            if (!_hashMap.ContainsKey(key))
            {
                return;
            }

            Unsubscribe(_hashMap[key], action);
        }

        private void Invoke(int hash, Object paramContainer)
        {
            if (!_map.TryGetValue(hash, out var action))
            {
                return;
            }
            
            var container = paramContainer != null
                ? paramContainer as AnimationEventParamContainer
                : null;
                
            action?.Invoke(container);
        }

        /// <summary>
        /// AnimationEvent Subscribed to animation events
        /// </summary>
        [UsedImplicitly]
        [Preserve]
        public void OnAnimationEvent(AnimationEvent @event)
        {
            var hash = @event.intParameter;
            
            if (hash == 0 && !_hashMap.TryGetValue(@event.stringParameter, out hash))
            {
                return;
            }

            Invoke(hash, @event.objectReferenceParameter);
        }
    }
}