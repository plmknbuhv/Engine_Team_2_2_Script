using Enemies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Units.UnitAnimals.Kangaroo
{
    [CreateAssetMenu(fileName = "KangarooDataSO", menuName = "SO/Unit/Enemy/KangarooData", order = 0)]
    public class KangarooDataSO : EnemyDataSo
    {
        public float skillCoolTime;
        public PoolItemSO skillEffect;
    }
}