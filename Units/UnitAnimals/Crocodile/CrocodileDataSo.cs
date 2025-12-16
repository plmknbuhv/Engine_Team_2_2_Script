using Enemies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Units.UnitAnimals.Crocodile
{
    [CreateAssetMenu(fileName = "SnappingTurtleDataSo", menuName = "SO/Unit/Enemy/CrocodileData", order = 0)]
    public class CrocodileDataSo : EnemyDataSo
    {
        public int needAttackCount;
        public float skillRange;
        public PoolItemSO skillEffect;
    }
}