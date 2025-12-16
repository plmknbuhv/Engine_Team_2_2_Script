using Code.Entities;
using Code.Units.UnitStat;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Units.Movements
{
    public class NavMovement : Movement
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float rotationSpeed = 10f;
        
        private UnitStatCompo _statCompo;
        
        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            agent.speed = _baseSpeed;
        }
        
        protected override void HandleChangeMoveStat(Stat stat, float currentValue, float prevValue)
        {
            float moveSpeed = _baseSpeed * currentValue;
            agent.speed = moveSpeed;
        }

        public void LookAtDestination(bool isSmooth = true) => LookAtTarget(agent.steeringTarget, isSmooth);
        
        public override void LookAtTarget(Vector3 target, bool isSmooth = true)
        {
            Vector3 direction = target - _unit.transform.position;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);

            if (isSmooth)
            {
                _unit.transform.rotation = Quaternion.Slerp(_unit.transform.rotation, lookRotation,
                    Time.deltaTime * rotationSpeed);
            }
            else
            {
                _unit.transform.rotation = lookRotation;    
            }
        }

        public override void Knockback(Vector3 direction, float force, Entity dealer)
        {
        }

        public void SetDestination(Vector3 target) => agent.SetDestination(target);
        public void SetStop(bool isStop) => agent.isStopped = isStop;

        public void WarpTo(Vector3 endPos) => agent.Warp(endPos);
        public void SetSpeed(float speed) => agent.speed = speed;
    }
}