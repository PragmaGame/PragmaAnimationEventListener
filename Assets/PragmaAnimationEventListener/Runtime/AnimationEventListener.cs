using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        
        private readonly Dictionary<string, int> _hashMap = new();
        
        private readonly Dictionary<int, Action<AnimationEventParamContainer>> _map = new();
        private readonly List<WaitAnimationEventData> _waitAnimationEventDates = new();

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
            var hashedKey = GetHash(key);

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

        public Task<AnimationEventParamContainer> WaitAnimationEvent(string key)
        {
            var hashedKey = GetHash(key);

            return WaitAnimationEvent(hashedKey);
        }

        public Task<AnimationEventParamContainer> WaitAnimationEvent(int hashedKey)
        {
            var waiter = _waitAnimationEventDates.Find(waiter => waiter.hashedKey == hashedKey);

            if (waiter == null)
            {
                var completionSource = new TaskCompletionSource<AnimationEventParamContainer>();
                
                _waitAnimationEventDates.Add(new WaitAnimationEventData()
                {
                    hashedKey = hashedKey,
                    completionSource = completionSource,
                });

                return completionSource.Task;
            }

            return waiter.completionSource.Task;
        }

        private void SendWaitCallback(int hash, AnimationEventParamContainer container)
        {
            if (_waitAnimationEventDates.Count == 0)
            {
                return;
            }
            
            var waiterIndex = _waitAnimationEventDates.FindIndex(waiter => waiter.hashedKey == hash);

            if (waiterIndex == -1)
            {
                return;
            }
            
            _waitAnimationEventDates[waiterIndex].completionSource.SetResult(container);
            _waitAnimationEventDates.RemoveAt(waiterIndex);
        }

        private void SendEventCallback(int hash, AnimationEventParamContainer container)
        {
            if (!_map.TryGetValue(hash, out var action))
            {
                return;
            }
            
            action?.Invoke(container);
        }

        private void SendCallback(int hash, Object paramContainer)
        {
            var container = paramContainer != null
                ? paramContainer as AnimationEventParamContainer
                : null;
            
            SendWaitCallback(hash, container);
            SendEventCallback(hash, container);
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

            SendCallback(hash, @event.objectReferenceParameter);
        }
        
        private int GetHash(string key)
        {
            if (!_hashMap.TryGetValue(key, out var hashedKey))
            {
                hashedKey = _hasher != null ? _hasher.GetHash(key) : Animator.StringToHash(key);
                _hashMap.Add(key, hashedKey);
            }

            return hashedKey;
        }
    }
}