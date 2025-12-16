using Code.Units.UnitDatas;
using Enemies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Units.UnitAnimals.SnappingTurtle
{
    [CreateAssetMenu(fileName = "SnappingTurtleDataSo", menuName = "SO/Unit/SnappingTurtleDataSo", order = 0)]
    public class SnappingTurtleDataSo : EnemyDataSo
    {
        public float creationDelay;
        public float duration;
        public PoolItemSO skillEffect;
    }
}