using Code.Entities;
using UnityEngine;

namespace Code.Units.State.Friendly
{
    public class UnitFlyingState : UnitState
    {
        public UnitFlyingState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Update()
        {
            base.Update();
            const float rotationSpeed = 360f;

            _entityAnimator.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        public override void Exit()
        {
            _unit.transform.rotation = Quaternion.identity;
            _entityAnimator.transform.rotation = Quaternion.identity;
            
            base.Exit();
        }
    }
}