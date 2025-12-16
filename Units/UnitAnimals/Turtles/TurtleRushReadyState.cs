using Code.Entities;
using Code.Units.State;
using DG.Tweening;
using Enemies;
using UnityEngine;
using UnityEngine.Splines;

namespace Code.Units.UnitAnimals.Turtles
{
    public class TurtleRushReadyState : UnitState
    {
        private readonly Turtle _turtle;
        private readonly TurtleDataSO _turtleData;
        private readonly FollowLine _followLine;

        public TurtleRushReadyState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _turtle = entity as Turtle;
            Debug.Assert(_turtle != null, "Turtle is null");
            _turtleData = _turtle.UnitData as TurtleDataSO;
            _followLine = _turtle.GetCompo<FollowLine>();
        }

        public override void Enter()
        {
            base.Enter();

            ReadyToRush();
        }

        private void ReadyToRush()
        {
            (Vector3 targetPoint, float normalizedTime) = _followLine.GetNearestInfo(_turtle.transform.position);

            _turtle.transform.DOMove(targetPoint, _turtleData.rushReadyMoveDuration).SetEase(Ease.InQuad)
                .OnComplete(() => HandleReady(normalizedTime));
        }

        private void HandleReady(float normalizedTime)
        {
            _followLine.SetAlign(SplineAnimate.AlignmentMode.SplineElement);
            _followLine.PlayMove(0, true, normalizedTime);

            if (_turtle.TargetUnit != null && _turtle.TargetUnit.IsDead == false
                                           && _turtle.TargetUnit is EnemyUnit enemy)
            {
                _turtle.TargetUnit = null;
                enemy.ChangeState("WALK");
            }

            _turtle.ChangeState("RUSH");
        }
    }
}