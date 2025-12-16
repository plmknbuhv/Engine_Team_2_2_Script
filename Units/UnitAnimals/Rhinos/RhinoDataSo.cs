using Code.Units.UnitDatas;
using Enemies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Units.UnitAnimals.Rhinos
{
    [CreateAssetMenu(fileName = "RhinoDataSo", menuName = "SO/Unit/Enemy/RhinoData", order = 0)]
    public class RhinoDataSo : EnemyDataSo
    {
        public float skillCoolTime;
        public float duration;
        public float skillMoveSpeed;
        public float skillRange;
        public float skillDamageMultiplier;
        public PoolItemSO skillEffect;
    }
}