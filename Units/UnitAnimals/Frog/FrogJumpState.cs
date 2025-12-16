using Code.Effects;
using Code.Entities;
using Code.Units.State;
using DG.Tweening;
using Enemies;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Units.UnitAnimals.Frog
{
    public class FrogJumpState : UnitState
    {
        private const float JumpReadyTime = 1.4f;
        
        private readonly FollowLine _followLine;

        private readonly FrogDataSO _frogData;
        private readonly Frog _frog;
        
        private EntityAnimator _animator;
        private Quaternion _targetRotation;
        
        private float _normalizedTime;
        private float _currentTime;

        private int _fallHash;

        private bool _isJumpComplete;
        private bool _isFallStart;
        
        
        public FrogJumpState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _fallHash  = Animator.StringToHash("FALL");
            
            _animator = entity.GetCompo<EntityAnimator>();
            _followLine = entity.GetCompo<FollowLine>();
            
            _frog = entity as Frog;
            _frogData = _frog.UnitData as FrogDataSO;
        }

        public override void Enter()
        {  
            base.Enter();
            
            _frog.CanTargeting = false;
            
            _frog.RigidCompo.linearVelocity = Vector3.zero;
            
            Vector3 endTarget = _frog.transform.right * 1.5f
                                + _frog.transform.forward * _frogData.skillMovementRange
                                + Vector3.up * 8 ;
            
            _frog.RigidCompo.AddForce(endTarget, ForceMode.Impulse);
            
            PlayEffect();
        }

        public override void Update()
        {
            base.Update();

            _currentTime += Time.deltaTime;

            if (_frog.RigidCompo.linearVelocity.y < 1f && _isFallStart == false)
            {
                _isFallStart = true;
                _animator.SetParam(_animationHash, false);
                _animator.SetParam(_fallHash, true);
            }
            
            if (_frog.RigidCompo.linearVelocity.y <= 0f 
                && _currentTime >= JumpReadyTime 
                && _isJumpComplete == false)
            {
                _isJumpComplete = true;

                (Vector3 nearPoint, float normalizedTime) = _followLine.GetNearestInfo(_unit.transform.position);
                _normalizedTime = normalizedTime;
                
                _unit.transform.DOMove(nearPoint, 0.05f)
                    .OnComplete(() => _unit.ChangeState("WALK"));
            }
        }

        public override void Exit()
        {
            base.Exit();
            
            _followLine.PlayMove(0, false, _normalizedTime);
            _animator.SetParam(_fallHash, false);
            _frog.ResetTimer();
            
            _isJumpComplete = false;
            _frog.CanTargeting = true;
            _isFallStart = false;
            _currentTime = 0;
            _normalizedTime = 0;
        }
        
        private void PlayEffect()
        {
            PoolingEffect skillEffect = PoolManagerMono.Instance.Pop<PoolingEffect>(_frogData.skillEffect);
            
            skillEffect.transform.SetParent(_frog.transform);
            skillEffect.PlayVFX(_frog.transform.position, _frog.transform.rotation);
        }
    }
}