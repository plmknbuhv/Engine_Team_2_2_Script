using Code.Effects;
using Code.Entities;
using Code.OverlapCheckers;
using Code.Units.State;
using Enemies;
using EventSystem;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Units.UnitAnimals.Orcas
{
    public class OrcaSkillAttackState : UnitState
    {
        private readonly FollowLine _followLine;
        private readonly CastChecker _castChecker;
        private readonly OrcaDataSo _orcaData;
        private readonly Orca _orca;
        
        private Quaternion _targetRotation;
        
        private float _currentDamageTime;
        private float _currentTime;
        
        private const string WaterBeam = "WaterBeam";
        
        
        public OrcaSkillAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _castChecker = entity.GetCompo<CastChecker>();
            _followLine = entity.GetCompo<FollowLine>();
            
            _orca = entity as Orca;
            _orcaData = _orca.UnitData as OrcaDataSo;
        }

        public override void Enter()
        {
            base.Enter();
            //_targetRotation = _orca.TargetRotation * Quaternion.Euler(0, 15f, 0);

            _orca.CanChangeState = false;
            _targetRotation = _orca.TargetRotation * Quaternion.Euler(0, 15f, 0);
            PlayEffect();
            
            _orca.SoundChannel.RaiseEvent(
                SoundEvents.PlayLongSFXSoundEvent.Initializer(_orca.SkillSound, WaterBeam));
        }

        public override void Update()
        {
            base.Update();

            _orca.transform.rotation
                = Quaternion.Lerp(_orca.transform.rotation, _targetRotation, Time.deltaTime * 3);
            
            _currentTime += Time.deltaTime;
            _currentDamageTime += Time.deltaTime;

            if (_orcaData.duration <= _currentTime)
            {
                _orca.CanChangeState = true;
                _orca.ChangeState("WALK");
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (_orcaData.damageOffsetTime <= _currentDamageTime)
            {
                foreach (var unit in _castChecker.GetCastData(CastType.Box))
                {
                    FriendlyUnit unitCompo = unit.GetComponent<FriendlyUnit>();
                    
                    if(unitCompo == null) continue;
                    
                    unitCompo.ApplyDamage(_orcaData.skillDamage, _orca);
                }
                _currentDamageTime = 0;
            }
        }

        public override void Exit()
        {
            base.Exit();
            _currentDamageTime = 0;
            _currentTime = 0;
            
            _orca.ResetTimer();
            
            _orca.SoundChannel.RaiseEvent(
                SoundEvents.StopLongSFXSoundEvent.Initializer(WaterBeam));
        }
        
        private void PlayEffect()
        {
            PoolingEffect skillEffect = PoolManagerMono.Instance.Pop<PoolingEffect>(_orcaData.skillEffect);
            
            skillEffect.transform.SetParent(_orca.transform);
            skillEffect.PlayVFX(_orca.transform.position, _orca.transform.rotation);
            skillEffect.transform.localPosition = new Vector3(0, 0.5f, 1);
            skillEffect.transform.rotation = _orca.transform.rotation * Quaternion.Euler(0, 180, 90);
        }
    }
}