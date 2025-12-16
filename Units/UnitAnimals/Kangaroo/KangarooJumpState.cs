using Code.Effects;
using Code.Entities;
using Code.Units.State;
using DG.Tweening;
using Enemies;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Units.UnitAnimals.Kangaroo
{
    public class KangarooJumpState : UnitState
    {
        private const float JumpReadyTime = 1.4f;
        
        private readonly FollowLine _followLine;

        private readonly KangarooDataSO _kangarooData;
        private readonly Kangaroo _kangaroo;
        
        private EntityAnimator _animator;
        private Quaternion _targetRotation;
        
        private float _normalizedTime;
        private float _currentTime;

        private int _fallHash;

        private bool _isJumpComplete;
        private bool _isFallStart;
        
        
        public KangarooJumpState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _fallHash  = Animator.StringToHash("FALL");
            
            _animator = entity.GetCompo<EntityAnimator>();
            _followLine = entity.GetCompo<FollowLine>();
            
            _kangaroo = entity as Kangaroo;
            _kangarooData = _kangaroo.UnitData as KangarooDataSO;
        }

        public override void Enter()
        {  
            base.Enter();
                
            _kangaroo.CanTargeting = false;
            _kangaroo.CanChangeState = false;
            
            Quaternion lookLineRotation = _followLine.GetLookLineRotation();
            _targetRotation = lookLineRotation * Quaternion.Euler(0, 90, 0);
            
            _kangaroo.RigidCompo.linearVelocity = Vector3.zero;

            Quaternion rotationTemp = _kangaroo.transform.rotation;
            
            _kangaroo.transform.rotation = lookLineRotation;
            Vector3 endTarget = _kangaroo.transform.right * 2f
                                + Vector3.up * 17f;
            
            _kangaroo.transform.rotation = rotationTemp;
            
            _kangaroo.RigidCompo.AddForce(endTarget, ForceMode.Impulse);
            

            foreach (var unit in _kangaroo.TargetUnitList)
            {
                Unit unitCompo = unit.GetComponentInChildren<FriendlyUnit>();
                //unitCompo.TargetUnit = null;
            }
            
            PlayEffect();
        }

        public override void Update()
        {
            base.Update();

            _currentTime += Time.deltaTime;
            
            Quaternion currentRotation = _unit.transform.rotation;
            _kangaroo.transform.rotation = Quaternion.Lerp(currentRotation,
                _targetRotation, _currentTime * 4);

            if (_kangaroo.RigidCompo.linearVelocity.y < 1f && _isFallStart == false)
            {
                _isFallStart = true;
                _animator.SetParam(_animationHash, false);
                _animator.SetParam(_fallHash, true);
            }
            
            if (_kangaroo.RigidCompo.linearVelocity.y <= -10f 
                && _currentTime >= JumpReadyTime 
                && _isJumpComplete == false)
            {
                _isJumpComplete = true;

                (Vector3 nearPoint, float normalizedTime) = _followLine.GetNearestInfo(_unit.transform.position);
                _normalizedTime = normalizedTime;

                _kangaroo.CanChangeState = true;
                
                _unit.transform.DOMove(nearPoint, 0.5f)
                    .OnComplete(() => _unit.ChangeState("WALK"));
            }
        }

        public override void Exit()
        {
            base.Exit();
            
            _followLine.PlayMove(0, false, _normalizedTime);
            _animator.SetParam(_fallHash, false);
            
            _kangaroo.IsInBattle = false;
            _kangaroo.CanTargeting = true;
            
            _isJumpComplete = false;
            _isFallStart = false;
            
            _currentTime = 0;
            _normalizedTime = 0;
        }
        
        private void PlayEffect()
        {
            PoolingEffect skillEffect = PoolManagerMono.Instance.Pop<PoolingEffect>(_kangarooData.skillEffect);
            
            skillEffect.transform.SetParent(_kangaroo.transform);
            skillEffect.PlayVFX(_kangaroo.transform.position, _kangaroo.transform.rotation);
        }
    }
}