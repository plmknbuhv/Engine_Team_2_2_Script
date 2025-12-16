using Code.Entities;
using Code.Units.Movements;
using Enemies;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Code.Units.State.Enemy
{
    public class EnemyIdleState : UnitState
    {
        private readonly FollowLine _followLine;
        private readonly EnemyMovement _enemyMovement;
        private readonly EnemyUnit _enemy;
        
        private readonly float _maxTime = 5f;
        private float _currentTime;
        
        public EnemyIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _followLine = entity.GetCompo<FollowLine>();
            _enemyMovement = entity.GetCompo<EnemyMovement>();

            _enemy = entity as EnemyUnit;
        }
        
        public override void Enter()
        {
            base.Enter();
            _currentTime = 0;
        }

        public override void Update()
        {
            base.Update();
            
            _currentTime += Time.deltaTime;
            
            const string walk = "WALK";
            
            if(_currentTime >= _maxTime)
                _unit.ChangeState(walk);
            
            
            /*if(_enemy.TargetUnit != null)
                _enemyMovement.LookAtTarget(_enemy.TargetUnit.transform.position);*/
        }
    }
}
