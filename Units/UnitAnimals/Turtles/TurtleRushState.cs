using Code.Effects;
using Code.Entities;
using Code.Units.State;
using Code.Units.UnitStat;
using DG.Tweening;
using Enemies;
using UnityEngine;
using UnityEngine.Splines;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Units.UnitAnimals.Turtles
{
    public class TurtleRushState : UnitState
    {
        private readonly Turtle _turtle;
        private readonly TurtleDataSO _turtleData;
        private readonly FollowLine _followLine;
        private readonly UnitStatCompo _statCompo;
        private float _rushStartTime;
        private float _rushDuration;

        public TurtleRushState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _turtle = entity as Turtle;
            Debug.Assert(_turtle != null, "Turtle is null");
            _turtleData = _turtle.UnitData as TurtleDataSO;
            _followLine = _turtle.GetCompo<FollowLine>();
            _statCompo = _turtle.GetCompo<UnitStatCompo>();
        }

        public override void Enter()
        {
            base.Enter();

            _rushStartTime = Time.time;

            float randomizeDuration = _turtleData.randomizeDuration;
            float randTime = Random.Range(-randomizeDuration, randomizeDuration);
            _rushDuration = _turtleData.rushDuration + randTime;
            
            _turtle.IsRushing = true;
            _turtle.OnEnemyCollisionEvent += HandleEnemyCollision;
        }

        public override void Update()
        {
            base.Update();

            bool isInStartPoint = _followLine.SplineAnimate.NormalizedTime < _turtleData.rushEndThreshold;
            if (_rushStartTime + _rushDuration < Time.time || isInStartPoint)
            {
                _turtle.ChangeState("IDLE");
            }
        }

        private void HandleEnemyCollision(EnemyUnit enemy)
        {
            if(enemy.CanTargeting == false) return;
            
            float damage = _turtleData.damage * _turtleData.rushDamageMultiplier;

            if (_statCompo.TryGetStat(UnitStatType.Damage, out Stat damageStat))
                damage *= damageStat.Value;
            
            enemy.ApplyDamage((int)damage, _turtle);

            if (_turtleData.rushSuccessEffect != null)
            {
                Vector3 direction = enemy.transform.position - _turtle.transform.position;
                Physics.Raycast(_turtle.transform.position, direction, out RaycastHit hit, direction.magnitude);

                PoolingEffect effect = PoolManagerMono.Instance.Pop<PoolingEffect>(_turtleData.rushSuccessEffect);
                effect.PlayVFX(hit.point, Quaternion.LookRotation(-direction) * Quaternion.Euler(0, -90, -90));
            }

            _turtle.OnRushAttackEvent?.Invoke();
            _turtle.ChangeState("IDLE");
        }

        public override void Exit()
        {
            base.Exit();

            _followLine.SetAlign(SplineAnimate.AlignmentMode.World);    
            _followLine.PauseMove();
            _followLine.SplineAnimate.Loop = SplineAnimate.LoopMode.Once;

            _turtle.OnRushEnd();
            _turtle.IsRushing = false;
            _turtle.OnEnemyCollisionEvent -= HandleEnemyCollision;
        }
    }
}