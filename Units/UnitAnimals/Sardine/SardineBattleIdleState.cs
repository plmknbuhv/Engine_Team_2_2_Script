using Code.Entities;
using Code.Units.State.Friendly;
using UnityEngine;

namespace Code.Units.UnitAnimals.Sardine
{
    public class SardineBattleIdleState : FriendlyBattleIdleState
    {
        private readonly Sardine _sardine;
        
        public SardineBattleIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _sardine = entity as Sardine;
            Debug.Assert(_sardine != null, $"sardine is null : {entity.gameObject.name}");
        }

        protected override void ChangeState(string stateName)
        {
            base.ChangeState(stateName);
            
            if(stateName == "ATTACK") _sardine.GroupAttack();
        }
    }
}