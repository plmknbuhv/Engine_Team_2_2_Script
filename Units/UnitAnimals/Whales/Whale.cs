using Code.Util;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Code.Units.UnitAnimals.Whales
{
    public class Whale : FriendlyUnit
    {
        [FormerlySerializedAs("OnWaveAttackEffect")] public UnityEvent OnWaveAttackEvent;
        public bool CanUseSkill => _countCondition.CanUseSkill;

        private WhaleDataSO _whaleData;
        private CountCondition _countCondition;

        protected override void Awake()
        {
            base.Awake();
            
            _whaleData = UnitData as WhaleDataSO;
            Debug.Assert(_whaleData != null, $"whale data is null");
            
            _countCondition = GetCompo<CountCondition>();
            _countCondition.SetNeedCount(_whaleData.needAttackCnt);
        }

        public void AddAttackCount() => _countCondition.AddCount(1);

        public void ResetCondition() => _countCondition.ResetCondition();
    }
}