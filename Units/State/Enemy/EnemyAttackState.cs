using Code.Entities;
using Code.Units.Movements;
using UnityEngine;

namespace Code.Units.State.Enemy
{
    public class EnemyAttackState : UnitState
    {
        private EnemyMovement _enemyMovement;
        private EnemyUnit _enemy;
        public EnemyAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _enemyMovement = _movement as EnemyMovement;
            _enemy = entity as EnemyUnit;
        }

        public override void Enter()
        {
            base.Enter();
            _animatorTrigger.OnAttackTrigger += HandleAttack;
        }

        protected virtual void HandleAttack()
        { 
            //여기 수정함 ?붙임
            _unit.TargetUnit?.ApplyDamage(_unit.UnitData.damage, _unit);
        }

        public override void Update()
        {
            base.Update();
            
            if (_enemy.TargetUnit == null || _unit.TargetUnit.IsDead)
            {
                _unit.ChangeState("WALK");
                return;
            }
            
            if (_isTriggerCall)
                _unit.ChangeState("BATTLEIDLE"); // 괜찮을지는 모르겠는데 일단 해봄 
            
            _enemyMovement.LookAtTarget(_unit.TargetUnit.transform.position);
        }

        public override void Exit()
        {
            _animatorTrigger.OnAttackTrigger -= HandleAttack;
            base.Exit();
        }
    }
}