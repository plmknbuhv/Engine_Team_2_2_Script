using Code.Util;
using UnityEngine;

namespace Code.Units.UnitAnimals.Crocodile
{
    public class Crocodile : EnemyUnit
    {
        public bool CanUseSkill => _countCondition.CanUseSkill;

        private CrocodileDataSo _crocodileData;
        private CountCondition _countCondition;
        private bool _isUseSkill;

        protected override void Awake()
        {
            base.Awake();

            _crocodileData = UnitData as CrocodileDataSo;
            Debug.Assert(_crocodileData != null, $"Crocodile data is null : {UnitData}");
            
            _countCondition = GetCompo<CountCondition>();
            _countCondition.SetNeedCount(_crocodileData.needAttackCount);
        }

        public void AddAttackCount() => _countCondition.AddCount(1);

        public void ResetAttackCount() => _countCondition.ResetCondition();
    }
}