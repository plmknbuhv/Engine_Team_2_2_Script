using Code.Entities;
using Code.Units.State.Friendly;
using UnityEngine;

namespace Code.Units.UnitAnimals.Hares
{
    public class HareAttackState : FriendlyAttackState
    {
        private readonly Hare _hare;
        
        public HareAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _hare = entity as Hare;
            Debug.Assert(_hare != null, $"Hare is null : {entity.gameObject.name}");
        }

        protected override void HandleAttack()
        {
            base.HandleAttack();
            
            _hare.AddAttackCount();
        }
    }
}