using System;
using System.Collections.Generic;
using Code.Units.UnitDatas;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "WaveDataSo", menuName = "SO/Wave/WaveDataSo", order = 0)]
    public class WaveDataSo : ScriptableObject
    {
        [Serializable]
        public class WaveEnemyDataGroup
        {
            public EnemyDataSo enemyDataSo;
            public int count;
            [Space]
            public float maxSpawnDelay;
            public float minSpawnDelay;
        }
        
        public List<WaveEnemyDataGroup> waveEnemyGroup;
        public int allEnemyCount;
        public bool isBossWave;

        private void OnValidate()
        {
            int count = 0;
            
            foreach (var waveEnemy in waveEnemyGroup)
            {
                count += waveEnemy.count;
            }
            
            allEnemyCount = count;
        }
    }
}