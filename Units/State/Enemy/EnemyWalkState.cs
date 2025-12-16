using Code.Entities;
using Enemies;
using UnityEngine;

namespace Code.Units.State.Enemy
{
    public class EnemyWalkState : UnitState
    {
        private readonly FollowLine _followLine;
        private readonly EnemyUnit _enemy;
        
        private readonly float _maxTime = 1f;
        
        private float _targetNormalized;
        private float _currentTime;
        
        public EnemyWalkState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _enemy = entity as EnemyUnit;
            _followLine = entity.GetCompo<FollowLine>();
        }
        
        public override void Enter()
        {
            base.Enter();
            
            _targetNormalized = _followLine.NormalizedTime;
            const string battleidle = "BATTLEIDLE";

            if (_enemy.TargetUnit != null && _unit.TargetUnit.IsDead == false)
                _unit.ChangeState(battleidle);
            else            
                _followLine.PlayMove();
        }

        public override void Update()
        {
            base.Update();

            _currentTime += Time.deltaTime;
            
            const string dead = "DEAD";
            
            if(_followLine.IsMoving == false)
                _followLine.PlayMove();
            
            if(_maxTime <= _currentTime && Mathf.Approximately(_targetNormalized, _followLine.NormalizedTime))
                _enemy.ChangeState(dead);
        }

        public override void Exit()
        {
            base.Exit();

            _enemy.TargetRotation = _enemy.transform.rotation;
            _followLine.PauseMove();
            _currentTime = 0;
        }
    }
}
