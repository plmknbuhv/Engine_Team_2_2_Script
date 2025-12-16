using Code.Entities;
using Code.Units.State.Friendly;

namespace Code.Units.UnitAnimals.Hares
{
    public class HareBattleIdleState : FriendlyBattleIdleState
    {
        private readonly Hare _hare;
        
        public HareBattleIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _hare = entity as Hare;
        }

        protected override void ChangeState(string stateName)
        {
            if (stateName == "ATTACK")
            {
                if(_hare.CanUseSkill)
                    _hare.ChangeState("FREEZE_SKILL");
                else
                    _hare.ChangeState("ATTACK");
            }
            else
            {
                base.ChangeState(stateName);
            }
        }
    }
}