using Code.Units.UnitDatas;
using UnityEngine;

namespace Code.Units.UnitAnimals.Tigers
{
    [CreateAssetMenu(fileName = "TigerData", menuName = "SO/Unit/UnitData/Tiger", order = 0)]
    public class TigerDataSO : UnitDataSO
    {
        public int defaultGrowCount = 5;
        public int maxGlowCount = 8;
        public float eatingRange = 2.5f;
        public float glowDamage = 0.1f;
        public float glowSize = 0.1f;
    }
}