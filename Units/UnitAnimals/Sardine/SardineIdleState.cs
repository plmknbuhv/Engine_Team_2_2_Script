using Code.Entities;
using Code.Units.State.Friendly;
using UnityEngine;

namespace Code.Units.UnitAnimals.Sardine
{
    public class SardineIdleState : FriendlyIdleState
    {
        private readonly Sardine _sardine;
        
        public SardineIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _sardine = entity as Sardine;
            Debug.Assert(_sardine != null, $"sardine is null : {entity.gameObject.name}");
        }

        protected override void ChangeChaseState()
        {
            _sardine.GroupAttack();
            
            base.ChangeChaseState();
        }
    }
}