using Code.Entities;
using Code.Units.State.Friendly;
using UnityEngine;

namespace Code.Units.UnitAnimals.MiniBears
{
    public class MiniBearIdleState : FriendlyIdleState
    {
        private readonly MiniBear _miniBear;
        
        public MiniBearIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _miniBear = entity as MiniBear;
            Debug.Assert(_miniBear != null, $"Mini bear is null : {entity}");
        }

        public override void FixedUpdate()
        {
            if(_miniBear.IsInit == false) return;
            
            base.FixedUpdate();
        }
    }
}