
using Enemies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Units.UnitAnimals.Orcas
{
    [CreateAssetMenu(fileName = "OrcaData", menuName = "SO/Unit/Enemy/OrcaData", order = 0)]
    public class OrcaDataSo : EnemyDataSo
    {
        public float skillCoolTime;
        public float duration;
        //대미지 받는 시간 간격
        public float damageOffsetTime;
        //지속 대미지
        public int skillDamage;
        public float skillRange;
        public PoolItemSO skillEffect;
    }
}