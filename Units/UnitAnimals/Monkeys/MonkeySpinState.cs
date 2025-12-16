using Code.Entities;
using Code.Units.State;
using DG.Tweening;

namespace Code.Units.UnitAnimals.Monkeys
{
    public class MonkeySpinState : UnitState
    {
        private Monkey _monkey;
        
        public MonkeySpinState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _monkey = entity as Monkey;
        }

        public override void Enter()
        {
            base.Enter();

            _monkey.rideStartPos = _monkey.transform.position; // 기린 타기 이전 위치 기억 
            
            _monkey.SetActiveNavAgent(false);
            _unit.transform.DOLocalMove(_monkey.RidePos, 0.4f);
            _unit.transform.DOLocalRotate(_monkey.RideRot, 0.4f);
        }

        public override void Exit()
        {
            _monkey.SetActiveNavAgent(true);
            
            base.Exit();
        }
    }
}