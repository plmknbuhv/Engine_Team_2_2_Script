using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        public bool IsDead { get; set; }
        public UnityEvent OnHitEvent;
        public UnityEvent OnDeadEvent;
        
        protected Dictionary<Type, IEntityComponent> _components;

        protected virtual void Awake()
        {
            _components = new Dictionary<Type, IEntityComponent>();
            AddComponents();
            InitializeComponents();
            AfterInitialize();
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void AddComponents()
        {
            GetComponentsInChildren<IEntityComponent>().ToList()
                .ForEach(component => _components.Add(component.GetType(), component));
        }

        protected virtual void InitializeComponents()
        {
            _components.Values.ToList().ForEach(component => component.Initialize(this));
        }
        
        protected virtual void AfterInitialize()
        {
            _components.Values.OfType<IAfterInitialize>()
                .ToList().ForEach(compo => compo.AfterInitialize());
        }

        public T GetCompo<T>() where T : IEntityComponent
        {
            if (_components.TryGetValue(typeof(T), out var component))
            {
                return (T)component;
            }
            
            // 만약에 없으면 자식까지 찾기
            Type type = _components.Keys.FirstOrDefault(type => type.IsSubclassOf(typeof(T)));
            if (type == null)
            {
                Debug.Log("얘도 없고 얘 자식도 없음");
                return default;
            }
            
            return (T)_components[type];
        }

        public IEntityComponent GetCompo(Type type)
            => _components.GetValueOrDefault(type);

        public void DestroyEntity()
        {
            Destroy(gameObject);
        }
    }
}