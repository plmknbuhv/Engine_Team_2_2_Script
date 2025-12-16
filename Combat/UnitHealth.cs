using System;
using Code.Entities;
using Code.Units;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Combat
{
    public class UnitHealth : MonoBehaviour, IEntityComponent
    {
        private Unit _unit;
        private int _maxHealth;

        public int CurrentHealth { get; private set; }
        
        // 첫 번째 인수 : 현재 체력, 두 번째 인수 최대 체력
        public UnityEvent<int, int> OnDamageEvent;
        public UnityEvent<int, int> OnDeadEvent;
        
        public void Initialize(Entity entity)
        {
            _unit = entity as Unit;
        }

        public void SetUpHealth(int health) // 시작할떄 쓰는거
        {
            _maxHealth = health;
            CurrentHealth = health;
        }

        public void ApplyDamage(int damage)
        {
            if (_unit.IsDead) return;
            
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, _maxHealth);
            OnDamageEvent?.Invoke(CurrentHealth, _maxHealth);
            _unit.OnHitEvent?.Invoke();
            
            if (CurrentHealth <= 0)
            {
                _unit.OnDeadEvent?.Invoke();
                OnDeadEvent?.Invoke(CurrentHealth, _maxHealth);
                _unit.IsDead = true;
            }
        }
    }
}