using Code.Util;
using UnityEngine;

namespace Code.Units.UnitAnimals.Hares
{
    public class Hare : FriendlyUnit
    {
        public bool CanUseSkill => _countCondition.CanUseSkill;
        public float FreezeTimeAdder => _freezeTimeAdder;

        private HareDataSO _hareData;
        private CountCondition _countCondition;
        private float _freezeTimeAdder;

        protected override void Awake()
        {
            base.Awake();

            _hareData = UnitData as HareDataSO;
            Debug.Assert(_hareData != null, $"Hare data is null : {UnitData}");
            
            _countCondition = GetCompo<CountCondition>();
            _countCondition.SetNeedCount(_hareData.needAttackCount);
        }

        public void AddAttackCount() => _countCondition.AddCount(1);

        public void ResetAttackCount() => _countCondition.ResetCondition();
    }
}