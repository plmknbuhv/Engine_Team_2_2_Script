using Code.Entities;
using UnityEngine;

namespace Code.Units.State
{
    public class UnitDeadState : UnitState
    {
        private float _timer;
        
        public UnitDeadState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _timer = 0;
            _unit.IsDead = true;
        }

        public override void Update()
        {
            base.Update();
            const float fallingSpeed = 1.5f;
            const float fallingTime = 1.5f;
            const float delayTime = 0.3f;
            
            if (_isTriggerCall)
            {
                _timer += Time.deltaTime;
                
                if (_timer >= delayTime)
                    _unit.transform.position += new Vector3(0, -1, 0) * (Time.deltaTime * fallingSpeed);
                if (_timer >= fallingTime)
                {
                    _isTriggerCall = false;
                    _unit.Pool.Push(_unit);
                }
            }

            if (_timer > 3f)
            {
                _unit.Pool.Push(_unit);
            }
        }
    }
}