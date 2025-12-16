using Code.Entities;
using Code.Units.Movements;
using Code.Util;
using DG.Tweening;
using Enemies;
using UnityEngine;

namespace Code.Units.State.Enemy
{
    public class EnemyKnockbackState : UnitState
    {
        private readonly EnemyUnit _owner;
        private readonly EnemyMovement _enemyMovement;
        private readonly FollowLine _followLine;
        private readonly float _recoveryDuration = 0.5f;
        private readonly float _knockbackCheckStartTime = 0.1f;

        private float _knockbackEndTime;
        private float _enterTime;
        private bool _isKnockbackEnd;
        private Tween _recoveryTween;
        
        public EnemyKnockbackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _owner = entity as EnemyUnit;
            Debug.Assert(_owner != null, $"Enemy is null : {entity}");
            _enemyMovement = _movement as EnemyMovement;
            _followLine = _owner.GetCompo<FollowLine>();
        }

        public override void Enter()
        {
            base.Enter();

            foreach (FriendlyUnit target in _owner.TargetUnitList)
            {
                if(target != null && target.IsDead == false)
                    target.ChangeState("IDLE");
            }
            //_owner.TargetUnitList.Clear();

            _owner.CanChangeState = false;
            _owner.CanTargeting = false;

            _enemyMovement.Rigid.useGravity = true;
            
            _recoveryTween?.Kill();
            
            _enterTime = Time.time;
        }

        public override void Update()
        {
            base.Update();

            if (_isKnockbackEnd)
                OnKnockbackEnd();
            else
                UpdateKnockback();
        }

        private void OnKnockbackEnd()
        {
            if(_enemyMovement.IsGround == false || _recoveryTween != null) return;

            (Vector3 nearPos, float normalizedTime) = _followLine.GetNearestInfo(_owner.transform.position);

            _recoveryTween = _owner.transform.DOMove(nearPos, _recoveryDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                _followLine.PlayMove(0, false, normalizedTime);
                _owner.CanChangeState = true;
                _owner.ChangeState("WALK");
            });
        }

        private void UpdateKnockback()
        {
            Vector3 currentVelocity = _enemyMovement.Rigid.linearVelocity;

            if (_enterTime + _knockbackCheckStartTime < Time.time && currentVelocity.Approximately(Vector3.zero))
            {
                _isKnockbackEnd = true;
                _knockbackEndTime = Time.time;
            }
        }

        public override void Exit()
        {
            base.Exit();

            _owner.CanTargeting = true;
            _isKnockbackEnd = false;

            _enemyMovement.Rigid.useGravity = false;
        }
    }
}