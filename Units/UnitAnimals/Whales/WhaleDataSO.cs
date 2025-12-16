using System.Collections.Generic;
using Code.Units.UnitAnimals.Sardine;
using Code.Units.UnitDatas;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Units.UnitAnimals.Whales
{
    [CreateAssetMenu(fileName = "WhaleData", menuName = "SO/Unit/UnitData/Whale", order = 0)]
    public class WhaleDataSO : UnitDataSO
    {
        public PoolItemSO waveItem;
        public float waveSpeed;
        public int waveDamage;
        public int needAttackCnt;
        public float waveDamageMultiplier;
    }
}