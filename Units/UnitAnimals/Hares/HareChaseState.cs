using Code.Entities;
using Code.Units.State.Friendly;

namespace Code.Units.UnitAnimals.Hares
{
    public class HareChaseState : UnitChaseState
    {
        private readonly Hare _hare;
        
        public HareChaseState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _hare = entity as Hare;
        }

        protected override void ChangeAttackState()
        {
            if(_hare.CanUseSkill)
                _hare.ChangeState("FREEZE_SKILL");
            else
                base.ChangeAttackState();
        }
    }
}