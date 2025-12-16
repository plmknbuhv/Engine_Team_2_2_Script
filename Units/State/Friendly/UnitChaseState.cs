using Code.Entities;
using Code.Units.Movements;
using UnityEngine;

namespace Code.Units.State.Friendly
{
    public class UnitChaseState : UnitState
    {
        private NavMovement _navMovement;
        private FriendlyUnit _friendlyUnit;
        private Vector3 _targetPosition;
        
        public UnitChaseState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _navMovement = _movement as NavMovement;
            _friendlyUnit = _unit as FriendlyUnit;
        }

        public override void Enter()
        {
            base.Enter();

            if (_unit.TargetUnit != null && _unit.TargetUnit.IsDead == false)
            {
                _targetPosition = _unit.TargetUnit.GetClosestPoint(_unit.transform.position);
                _navMovement.SetDestination(_targetPosition);
                _unit.IsInBattle = true;
                _navMovement.SetStop(false);
            }
            else
            {
                _unit.ChangeState("IDLE");
            }
        }

        public override void Update()
        {
            base.Update();

            if (_unit.TargetUnit == null || _unit.TargetUnit.IsDead)
            {
                _unit.ChangeState("IDLE");
                return;
            }
            
            _navMovement.LookAtDestination();
            EnemyUnit enemyUnit = _friendlyUnit.TargetUnit as EnemyUnit;
            
            
            Debug.Assert(enemyUnit != null, "이거 찾은 애가 에너미가 아닌데?");

            if (enemyUnit.IsDead == false
                && enemyUnit.TargetUnitList.Contains(_friendlyUnit) == false)
            {
                enemyUnit.TargetUnitList.Add(_friendlyUnit);
            }

            if (Vector3.Distance(_unit.transform.position, _targetPosition) < _unit.UnitData.attackRange) // 도착 했으면
            {
                _navMovement.SetStop(true);
                _navMovement.SetDestination(_unit.transform.position);
                
                enemyUnit.StartBattle();
                ChangeAttackState();
            }

        }

        protected virtual void ChangeAttackState()
        {
            _unit.ChangeState("ATTACK");
        }
    }
}