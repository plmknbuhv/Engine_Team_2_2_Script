using Code.Units.UnitDatas;
using UnityEngine;

namespace Code.Units.UnitAnimals.Giraffes
{
    [CreateAssetMenu(fileName = "GiraffeData", menuName = "SO/Unit/UnitData/Giraffe", order = 0)]
    public class GiraffeDataSO : UnitDataSO
    {
        public float spinRange = 2.5f;
        public float spinAttackDelay = 1f;
        public float spinDuration = 10f;
        public int spinAttackDamage = 10;
    }
}