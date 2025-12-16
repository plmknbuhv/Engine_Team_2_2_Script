using Code.Entities;
using Code.Units.State;
using UnityEngine;

namespace Code.Units.UnitAnimals.Tigers
{
    public class TigerEatState : UnitState
    {
        private Tiger _tiger;
        
        public TigerEatState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _tiger = _unit as Tiger;
        }

        public override void Enter()
        {
            base.Enter();
            _animatorTrigger.OnAttackTrigger += HandleAttack;
            _tiger.CanEat = false;
        }

        public override void Update()
        {
            base.Update();

            _movement.LookAtTarget(_tiger.EatingTarget.transform.position);
            
            if (_isTriggerCall)
            {
                _tiger.ChangeState("IDLE");
                _isTriggerCall = false;
            }
        }
        
        protected virtual void HandleAttack()
        {
            _tiger.EatingTarget.DestroyUnit();
            _tiger.OnEat?.Invoke();
            _tiger.GlowStat();
        }

        public override void Exit()
        {
            _animatorTrigger.OnAttackTrigger -= HandleAttack;
            _tiger.CanEat = true;
            base.Exit();
        }
    }
}