using Enemies;
using UnityEngine;

namespace Code.Units.UnitAnimals.Narwhal
{
    [CreateAssetMenu(fileName = "NarwhalDataSO", menuName = "SO/Unit/Enemy/NarwhalData", order = 0)]
    public class NarwhalDataSO : EnemyDataSo
    {
        public float skillRange;
        [Space]
        public float moveSpeedStatMultiplier;
        public float damageStatMultiplier;
        public float attackDelayStatMultiplier;
    }
}