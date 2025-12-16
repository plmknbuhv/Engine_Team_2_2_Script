using Enemies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Units.UnitAnimals.Elephants
{
    [CreateAssetMenu(fileName = "SnappingTurtleDataSo", menuName = "SO/Unit/Enemy/ElephantData", order = 0)]
    public class ElephantDataSo : EnemyDataSo
    {
        public float skillCoolTime;
        public float stunDuration;
        
        public float angerModeAttackSpeed;
        public float angerModeMoveSpeed;
        
        public float skillRange;
        public int skillDamage;
        public PoolItemSO skillEffect;
        public PoolItemSO skillUltimateEffect;
    }
}