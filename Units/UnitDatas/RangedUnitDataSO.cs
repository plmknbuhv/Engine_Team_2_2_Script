using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Units.UnitDatas
{
    [CreateAssetMenu(fileName = "RangedUnitData", menuName = "SO/Unit/UnitData/RangedUnitData", order = 0)]
    public class RangedUnitDataSO : UnitDataSO
    {
        public float rangedAttackRange;
        public float rangedAttackCoefficient;
        public float rangedAttackDelay;
        public PoolItemSO projectileItem;
    }
}