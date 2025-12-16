using Code.Entities;
using Code.Units.State.Friendly;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Units.UnitAnimals.Penguin
{
    public class PenguinSwimState : UnitChaseState
    {
        private readonly Penguin _penguin;
        private readonly PenguinDataSO _penguinData;
        private readonly float _originSpeed;
        private readonly NavMeshAgent _navMeshAgent;
        
        public PenguinSwimState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _penguin = entity as Penguin;
            Debug.Assert(_penguin != null, $"penguin is null {entity.gameObject.name}");
            _penguinData = _penguin.UnitData as PenguinDataSO;

            _navMeshAgent = _penguin.GetComponent<NavMeshAgent>();
            _originSpeed = _navMeshAgent.speed;
        }

        public override void Enter()
        {
            base.Enter();

            _penguin.OnSwimStart?.Invoke();
            _navMeshAgent.speed = _penguinData.swimSpeed;
        }

        public override void Exit()
        {
            base.Exit();

            _navMeshAgent.speed = _originSpeed;
        }

        protected override void ChangeAttackState()
        {
            _penguin.IsSwimEnd = true;
            _penguin.ChangeState("PENGUIN_SWIM_ATTACK");
        }
    }
}