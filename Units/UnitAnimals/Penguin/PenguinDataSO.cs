using Code.Units.UnitDatas;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Units.UnitAnimals.Penguin
{
    [CreateAssetMenu(fileName = "PenguinData", menuName = "SO/Unit/UnitData/Penguin", order = 0)]
    public class PenguinDataSO : UnitDataSO
    {
        public PoolItemSO swimAttackEffect;
        public Vector3 effectOffset;
        public int swimAttackDamage;
        public float swimSpeed;
    }
}