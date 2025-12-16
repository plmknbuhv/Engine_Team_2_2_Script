using Enemies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Units.UnitAnimals.Frog
{
    [CreateAssetMenu(fileName = "FrogDataSO", menuName = "SO/Unit/Enemy/FrogData", order = 0)]
    public class FrogDataSO : EnemyDataSo
    {
        public float skillMovementRange;
        public float skillCoolTime;
        public PoolItemSO skillEffect;
    }
}