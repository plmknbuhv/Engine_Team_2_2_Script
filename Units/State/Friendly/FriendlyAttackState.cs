using Code.Entities;
using Code.Units.UnitDatas;
using Code.Units.UnitStat;
using UnityEngine;

namespace Code.Units.State.Friendly
{
    public class FriendlyAttackState : UnitState
    {
        private Stat _damageStat;
        private UnitDataSO _unitData;
        
        public FriendlyAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _damageStat = _unit.GetCompo<UnitStatCompo>().GetStat(UnitStatType.Damage);
            _unitData = _unit.UnitData;
        }

        public override void Enter()
        {
            base.Enter();
            _animatorTrigger.OnAttackTrigger += HandleAttack;
        }

        protected virtual void HandleAttack()
        {
            if (_unit.TargetUnit != null && _unit.TargetUnit.IsDead == false)
            {
                int damage = Mathf.RoundToInt(_damageStat.Value * _unitData.damage); 
                _unit.TargetUnit.ApplyDamage(damage, _unit);
            }
        }

        public override void Update()
        {
            base.Update();

            if (_isTriggerCall)
                _unit.ChangeState("BATTLEIDLE");
            
            if (_unit == null || _unit.TargetUnit == null || _unit.TargetUnit.IsDead)
            {
                _unit.ChangeState("IDLE");
                return;
            }

            _movement.LookAtTarget(_unit.TargetUnit.transform.position);
        }

        public override void Exit()
        {
            _animatorTrigger.OnAttackTrigger -= HandleAttack;
            base.Exit();
        }
    }
}