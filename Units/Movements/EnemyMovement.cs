using System;
using System.Drawing;
using Code.Entities;
using Code.Units.UnitStat;
using Enemies;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Code.Units.Movements
{
    public class EnemyMovement : Movement
    {
        public Rigidbody Rigid { get; private set; }

        public bool IsGround
        {
            get
            {
                Vector3 checkerPos = transform.position + groundCheckOffset;
                Vector3 halfSize = groundCheckSize / 2;
                
                int cnt = Physics.OverlapBoxNonAlloc(checkerPos, halfSize, _result, Quaternion.identity, groundLayer);
                return cnt > 0;
            }
        }

        [Header("Ground check settings")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Vector3 groundCheckOffset;
        [SerializeField] private Vector3 groundCheckSize;
        
        private FollowLine _followLine;
        private Transform _lookTarget;
        private EnemyUnit _owner;
        private Collider[] _result;

        private bool _isBuffDeBuff;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            
            _owner = entity as EnemyUnit;
            Debug.Assert(_owner != null, $"Enemy is null : {entity}");

            _followLine = entity.GetCompo<FollowLine>();
            _lookTarget = entity.transform;
            
            Rigid = _owner.GetComponent<Rigidbody>();

            _result = new Collider[1];
        }
        
        public override void LookAtTarget(Vector3 target, bool isSmooth = true)
        {
            Vector3 direction = (target - _lookTarget.position).normalized;
            direction.y = 0;
            
            if (isSmooth)
            {
                _lookTarget.rotation = Quaternion.Lerp(_lookTarget.rotation,
                    Quaternion.LookRotation(direction), Time.deltaTime * 8f);
            }
            else
            {
                _lookTarget.rotation = Quaternion.LookRotation(direction);
            }
        }

        public override void Knockback(Vector3 direction, float force, Entity dealer)
        {
            if(_owner.IsDead) return;
            
            _followLine.PauseMove();
            Rigid.linearVelocity = Vector3.zero;
            Rigid.AddForce(direction * (force * _unit.UnitData.knockbackMultiplier), ForceMode.Impulse);
            
            _owner.ChangeState("KNOCKBACK");
        }

        public void PlayMoveAtNearPoint(Vector3 targetPos)
        {
            (Vector3 _, float normalizedTime) = _followLine.GetNearestInfo(targetPos);
            _followLine.PlayMove(0, false, normalizedTime);
        }

        protected override void HandleChangeMoveStat(Stat stat, float currentValue, float prevValue)
        {
            if (_followLine == null)
            {
                Debug.LogWarning("FollowLine가 없습니다.");
                return;
            }

            if (stat.IsMoveSpeedModifier) return;

            float moveSpeed = _baseSpeed * currentValue;
            _followLine.SetDuration(moveSpeed);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(transform.position + groundCheckOffset, groundCheckSize);
        }
    }
}