using Code.Units.UnitDatas;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Units.UnitAnimals.Walruses
{
    [CreateAssetMenu(fileName = "Walrus data", menuName = "SO/Unit/UnitData/Walrus", order = 0)]
    public class WalrusDataSO : UnitDataSO
    {
        public float atkDelayDecreaseAmount;
        public float speedIncreaseOnBiteTarget;
        public PoolItemSO biteAttackEffect;
    }
}