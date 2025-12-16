using Code.Entities;
using Code.Units.State.Enemy;
using UnityEngine;

namespace Code.Units.UnitAnimals.Crocodile
{
    public class CrocodileAttackState : EnemyAttackState
    {
        private Crocodile _crocodile;
        
        public CrocodileAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _crocodile = entity as Crocodile;
            Debug.Assert(_crocodile != null, $"Crocodile is null : {entity.gameObject.name}");
        }

        protected override void HandleAttack()
        {
            base.HandleAttack();
            _crocodile.AddAttackCount();
        }
    }
}