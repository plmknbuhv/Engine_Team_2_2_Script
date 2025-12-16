using Code.Units.UnitAnimals.MiniBears;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Units.UnitAnimals.Bears
{
    public class Bear : FriendlyUnit
    {
        private BearDataSO _bearData;
        private int _miniBearCount;

        protected override void Awake()
        {
            base.Awake();

            _bearData = UnitData as BearDataSO;
            Debug.Assert(_bearData != null, "MiniBear data is null");
        }

        public override void SetUpUnit()
        {
            base.SetUpUnit();

            SpawnMiniBears();
        }

        private void SpawnMiniBears()
        {
            int bearCnt = _bearData.miniBearCount + _miniBearCount;
            
            float angleAdder = 360f / bearCnt;

            for (int i = 0; i < bearCnt; ++i)
            {
                float angle = (angleAdder * i + _bearData.angleOffset) * Mathf.Deg2Rad;
                float x = Mathf.Cos(angle);
                float z = Mathf.Sin(angle);

                Vector3 offset = new Vector3(x, 0, z) * _bearData.spawnDistance;
                Vector3 spawnPos = transform.position + offset;

                int randIdx = Random.Range(0, _bearData.miniBearColors.Count);
                Color color = _bearData.miniBearColors[randIdx];

                MiniBear newBear = PoolManagerMono.Instance.Pop<MiniBear>(_bearData.miniBearItem);
                newBear.Initialize(transform.position, spawnPos, transform.rotation, color, _bearData.miniBearInitDuration);
            }
        }
    }
}