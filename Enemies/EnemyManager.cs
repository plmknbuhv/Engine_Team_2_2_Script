using System.Collections.Generic;
using Code.Units;
using EventSystem;
using GondrLib.Dependencies;
using UnityEngine;

namespace Enemies
{
    [Provide]
    public class EnemyManager : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private GameEventChannelSO enemyChannel;
        public List<EnemyUnit> CurrentEnemies { get; private set; }

        private void Awake()
        {
            CurrentEnemies = new List<EnemyUnit>();
            enemyChannel.AddListener<EnemyListChangedEvent>(HandleAddEnemy);
        }

        private void OnDestroy()
        {
            enemyChannel.RemoveListener<EnemyListChangedEvent>(HandleAddEnemy);
        }

        public void HandleAddEnemy(EnemyListChangedEvent evt)
        {
            if (evt.enemyRegistry == EnemyRegistry.Add)
                CurrentEnemies.Add(evt.enemy);
            else
                CurrentEnemies.Remove(evt.enemy);
        }
    }
}