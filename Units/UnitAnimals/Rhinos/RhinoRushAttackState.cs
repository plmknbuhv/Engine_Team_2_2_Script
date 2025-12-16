using System.Collections.Generic;
using Code.Effects;
using Code.Entities;
using Code.Units.State;
using Enemies;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Units.UnitAnimals.Rhinos
{
    public class RhinoRushAttackState : UnitState
    {
        private readonly UnitDetector _unitDetector;
        private readonly FollowLine _followLine;
        private readonly RhinoDataSo _rhinoData;
        private readonly Rhino _rhino;
        private float _currentTime;
        
        public RhinoRushAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _unitDetector = entity.GetCompo<UnitDetector>();
            _followLine = entity.GetCompo<FollowLine>();
            
            _rhino = entity as Rhino;
            _rhinoData = _rhino.UnitData as RhinoDataSo;
        }

        public override void Enter()
        {
            base.Enter();
            
            _rhino.CanTargeting = false;
            
            _followLine.SetDuration(_followLine.SplineAnimate.MaxSpeed + _rhinoData.skillMoveSpeed);
            _followLine.PlayMove();
            
            _animatorTrigger.OnAttackTrigger += HandleAttack;
            
            PlayEffect();
        }

        public override void Update()
        {
            base.Update();
            
            _currentTime += Time.deltaTime;

            if (_rhinoData.duration <= _currentTime)
            {
                _rhino.ChangeState("WALK");
            }
        }

        public override void Exit()
        {
            base.Exit();
            _animatorTrigger.OnAttackTrigger -= HandleAttack;
            
            _rhino.CanTargeting = true;
            
            _followLine.PauseMove();
            
            _followLine.SetDuration(_followLine.SplineAnimate.MaxSpeed - _rhinoData.skillMoveSpeed);
            _rhino.ResetTimer();
            _currentTime = 0;
        }
        
        private void PlayEffect()
        {
            PoolingEffect skillEffect = PoolManagerMono.Instance.Pop<PoolingEffect>(_rhinoData.skillEffect);

            skillEffect.transform.SetParent(_rhino.transform);
            skillEffect.PlayVFX(_rhino.transform.position + new Vector3(0, -0.02f, 0), Quaternion.identity);
            skillEffect.transform.rotation = _rhino.transform.rotation * Quaternion.Euler(0, 180, 0);
        }

        private void HandleAttack()
        {
            if (_unitDetector.TryGetUnits(out HashSet<Unit> units, _rhinoData.skillRange, UnitTeamType.Friendly))
            {
                foreach (Unit unit in units)
                {
                    unit.ApplyDamage(_rhinoData.damage, _unit);
                }
            }
        }
    }
}