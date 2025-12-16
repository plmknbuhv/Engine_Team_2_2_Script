using Code.Units.UnitDatas;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Units.UnitAnimals.Hares
{
    [CreateAssetMenu(fileName = "HareData", menuName = "SO/Unit/UnitData/Hare", order = 0)]
    public class HareDataSO : UnitDataSO
    {
        public int needAttackCount;
        public float skillRange;
        public float skillDamageMultiplier;
        public float freezeTime;
        public PoolItemSO freezeEffect;
    }
}