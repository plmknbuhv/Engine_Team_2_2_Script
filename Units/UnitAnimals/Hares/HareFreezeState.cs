using System.Collections.Generic;
using Code.Effects;
using Code.Entities;
using Code.Units.State;
using Code.Units.State.Friendly;
using Code.Units.UnitStatusEffects;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Units.UnitAnimals.Hares
{
    public class HareFreezeState : UnitState
    {
        private readonly Hare _hare;
        private readonly HareDataSO _hareData;
        private readonly UnitDetector _unitDetector;

        public HareFreezeState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _hare = entity as Hare;
            Debug.Assert(_hare != null, $"Hare is null : {entity.gameObject.name}");

            _hareData = _hare.UnitData as HareDataSO;
            _unitDetector = entity.GetCompo<UnitDetector>();
        }

        public override void Enter()
        {
            base.Enter();

            PlayEffect();

            _hare.TargetUnit = null;
            
            _animatorTrigger.OnAttackTrigger += HandleAttack;
        }

        public override void Update()
        {
            base.Update();

            if (_isTriggerCall)
            {
                _hare.ChangeState("IDLE");
                _hare.ResetAttackCount();
            }
        }

        public override void Exit()
        {
            base.Exit();

            _animatorTrigger.OnAttackTrigger -= HandleAttack;
        }

        private void PlayEffect()
        {
            PoolingEffect freezeEffect = PoolManagerMono.Instance.Pop<PoolingEffect>(_hareData.freezeEffect);
            freezeEffect.PlayVFX(_unit.transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity);
        }

        private void HandleAttack()
        {
            if (_unitDetector.TryGetUnits(out HashSet<Unit> units, _hareData.skillRange, UnitTeamType.Enemy))
            {
                int damage = 5; // 스탯 시스템 스탯 컴포넌트로 옮기면 바꿀 예정
                float freezeDuration = _hareData.freezeTime + _hare.FreezeTimeAdder;
                
                foreach (Unit unit in units)
                {
                    unit.ApplyDamage(damage, _unit);
                    unit.ApplyStatusEffect(StatusEffectType.FREEZE, freezeDuration);
                }
            }
        }
    }
}