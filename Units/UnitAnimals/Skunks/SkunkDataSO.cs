using Code.Units.UnitDatas;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Units.UnitAnimals.Skunks
{
    [CreateAssetMenu(fileName = "SkunkData", menuName = "SO/Unit/UnitData/Skunk", order = 0)]
    public class SkunkDataSO : UnitDataSO
    {
        public float slowCoefficient = 0.8f;
        public float slowRange = 0.8f;
        public PoolItemSO debuffEffectItem;
    }
}