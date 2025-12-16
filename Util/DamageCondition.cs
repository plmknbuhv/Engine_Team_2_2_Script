using Code.Combat;
using Code.Entities;
using UnityEngine;

namespace Code.Util
{
    public class DamageCondition : UnitSkillCondition
    {
        public override bool CanUseSkill { get; protected set; }

        private UnitHealth _unitHealth;
        private bool _isActive;
        private int _targetHp;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);

            _unitHealth = _unit.GetCompo<UnitHealth>();
        }

        public void SetTargetHp(int targetHp)
        {
            _targetHp = targetHp;

            if (_isActive) ResetCondition();
            
            _isActive = true;

            _unitHealth.OnDamageEvent.AddListener(HandleDamaged);
        }

        public override void ResetCondition()
        {
            _unitHealth.OnDamageEvent.RemoveListener(HandleDamaged);
            CanUseSkill = false;
            _isActive = false;
        }

        private void HandleDamaged(int current, int max) => CanUseSkill = current <= _targetHp;
    }
}