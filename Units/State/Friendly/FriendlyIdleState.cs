using Code.Entities;
using Code.Units.Movements;
using Code.Units.UnitStat;
using UnityEngine;

namespace Code.Units.State.Friendly
{
    public class FriendlyIdleState : UnitState // 이름 일단 임시로 이렇게
    {
        protected UnitDetector _detector;
        protected FriendlyUnit _friendlyUnit;
        
        public FriendlyIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _detector = _unit.GetCompo<UnitDetector>();
            _friendlyUnit = _unit as FriendlyUnit;
        }

        public override void Enter()
        {
            _unit.IsInBattle = false;
            base.Enter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            bool IsTarget(Unit unit) => unit.CanTargeting;
            _detector.SetTarget(UnitTeamType.Enemy);
            if (_detector.TryGetClosestEnemy(out Unit enemy, _unit.UnitData.detectRange, IsTarget))
            {
                EnemyUnit enemyUnit = enemy as EnemyUnit;
                Debug.Assert(enemyUnit != null, "이거 찾은 애가 에너미가 아닌데?");
                
                enemyUnit.MarkAttackTarget(_friendlyUnit);
                _unit.TargetUnit = enemyUnit;
                ChangeChaseState();
            }
        }

        protected virtual void ChangeChaseState()
        {
            _unit.ChangeState("CHASE");
        }
    }
}