using System.Collections.Generic;
using Code.Units.UnitDatas;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Units.UnitAnimals.Bears
{
    [CreateAssetMenu(fileName = "BearData", menuName = "SO/Unit/UnitData/Bear", order = 0)]
    public class BearDataSO : UnitDataSO
    {
        public PoolItemSO miniBearItem;
        public int miniBearCount;
        public float angleOffset;
        public float spawnDistance;
        public List<Color> miniBearColors;
        public float miniBearInitDuration = 0.3f;
    }
}