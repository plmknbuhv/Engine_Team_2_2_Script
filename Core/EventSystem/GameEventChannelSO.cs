using System;
using System.Collections.Generic;
using UnityEngine;

namespace EventSystem
{
    public abstract class GameEvent {}
    
    [CreateAssetMenu(fileName = "Channel", menuName = "SO/Events/Channel", order = 0)]
    public class GameEventChannelSO : ScriptableObject
    {
        private Dictionary<Type, Action<GameEvent>> _events = new Dictionary<Type, Action<GameEvent>>();
        private Dictionary<Delegate, Action<GameEvent>> _lookUp = new Dictionary<Delegate, Action<GameEvent>>();
        
        public void AddListener<T>(Action<T> handler) where T : GameEvent
        {
            if (_lookUp.ContainsKey(handler))
            {
                Debug.LogWarning("Listener already exists for this event type.");
                return;
            }
            
            Action<GameEvent> castHandler = (evt) => handler((T)evt);
            _lookUp.Add(handler, castHandler);
            
            Type castType = typeof(T);
            if (!_events.TryAdd(castType, castHandler)) 
            {
                _events[castType] += castHandler;
            }
        }
        
        public void RemoveListener<T>(Action<T> handler) where T : GameEvent
        {
            Type evtType = typeof(T);
            if (_lookUp.TryGetValue(handler, out var action))
            {
                if (_events.TryGetValue(evtType, out var internalAction))
                {
                    internalAction -= action;
                    if (internalAction == null)
                    {
                        _events.Remove(evtType);
                    }
                    else
                    {
                        _events[evtType] = internalAction;
                    }
                }
                
                _lookUp.Remove(handler);
            }
        }

        public void RaiseEvent(GameEvent evt)
        {
            if (_events.TryGetValue(evt.GetType(), out var handler))
            {
                handler?.Invoke(evt);
            }
        }
        
        public void Clear()
        {
            _events.Clear();
            _lookUp.Clear();
        }
    }
}
