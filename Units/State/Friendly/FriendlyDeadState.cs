using Code.Entities;
using UnityEngine;

namespace Code.Units.State.Friendly
{
    public class FriendlyDeadState : UnitDeadState
    {
        private FriendlyUnit _friendlyUnit;
        
        public FriendlyDeadState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _friendlyUnit = _unit as FriendlyUnit;
        }

        public override void Enter()
        {
            base.Enter();
            
            _friendlyUnit.DeadUnit();
        }
    }
}