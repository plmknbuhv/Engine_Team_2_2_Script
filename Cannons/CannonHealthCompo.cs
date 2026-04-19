using System;
using EventSystem;
using Unity.Mathematics;
using UnityEngine;

namespace Cannons
{
    public class CannonHealthCompo : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO cannonCannel;
        [SerializeField] private GameEventChannelSO enemyChannel;
        [SerializeField] private int maxHealth = 30;

        private int _currentHealth;

        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = Mathf.Clamp(value, 0, maxHealth);
                
                var changeEvt = CannonEvents.CannonChangeHealthEvent.Initializer(_currentHealth);
                cannonCannel.RaiseEvent(changeEvt);

                if (_currentHealth <= 0)
                {
                    var deathEvt = CannonEvents.CannonDeathEvent;
                    cannonCannel.RaiseEvent(deathEvt);
                }
            }
        }

        private void Awake()
        {
            cannonCannel.AddListener<CannonAddHealthEvent>(HandleAddHealth);
            enemyChannel.AddListener<EnemyPathCompleteEvent>(HandleEnemyAttack);
        }

        private void Start()
        {
            CurrentHealth = maxHealth;
        }

        private void HandleEnemyAttack(EnemyPathCompleteEvent evt)
        {
            AddHealth(-(int)evt.enemyType);
        }

        private void OnDestroy()
        {
            cannonCannel.RemoveListener<CannonAddHealthEvent>(HandleAddHealth);
            enemyChannel.RemoveListener<EnemyPathCompleteEvent>(HandleEnemyAttack);
        }

        public void AddHealth(int addHealth)
        {
            CurrentHealth += addHealth;
        }

        private void HandleAddHealth(CannonAddHealthEvent evt)
        {
            AddHealth(evt.addHealth);
        }
    }
}