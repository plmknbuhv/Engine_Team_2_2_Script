using System.Collections.Generic;
using Code.Combat;
using Code.Effects;
using Code.Entities;
using Code.Units.State;
using Code.Units.UnitStatusEffects;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Units.UnitAnimals.Elephants
{
    public class ElephantSkillAttackState : UnitState
    {
        private readonly UnitDetector _unitDetector;
        private readonly ElephantDataSo _elephantData;
        private readonly UnitHealth _unitHealth;
        private readonly Elephant _elephant;
        
        public ElephantSkillAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _unitDetector = entity.GetCompo<UnitDetector>();
            _unitHealth = entity.GetCompo<UnitHealth>();
            _elephant = entity as Elephant;
            _elephantData = _elephant.UnitData as ElephantDataSo;
        }

        public override void Enter()
        {
            base.Enter();
            _animatorTrigger.OnAttackTrigger += HandleAttack;
            PlayEffect();
        }

        public override void Update()
        {
            base.Update();
            
            const string battleidle = "BATTLEIDLE";
            const string dead = "DEAD";

            if (_unit.IsDead)
            {
                _elephant.ChangeState(dead);
            }
            
            if (_isTriggerCall)
            {
                _elephant.ChangeState(battleidle);
            }
        }

        public override void Exit()
        {
            base.Exit();
            _animatorTrigger.OnAttackTrigger -= HandleAttack;
            _elephant.ResetTimer();
            _elephant.IsFirstUltimate = false;
        }
        
        private void PlayEffect()
        {
            PoolItemSO effectPoolItem = _elephant.IsFirstUltimate ? _elephantData.skillUltimateEffect : _elephantData.skillEffect;
            PoolingEffect skillEffect = PoolManagerMono.Instance.Pop<PoolingEffect>(effectPoolItem);
            skillEffect.PlayVFX(_elephant.transform.position + new Vector3(0, -0.02f, 0), _elephant.transform.rotation);
        }

        private void HandleAttack()
        {
            if (_unitDetector.TryGetUnits(out HashSet<Unit> units, _elephantData.skillRange, UnitTeamType.Friendly))
            {
                foreach (Unit unit in units)
                {
                    int damage = _elephant.IsFirstUltimate ? 9999 : _elephantData.skillDamage;

                    unit.ApplyDamage(damage, _unit);
                    
                    if(unit.IsDead == false)
                        unit.ApplyStatusEffect(StatusEffectType.STUN, _elephantData.stunDuration);
                }
            }
        }
    }
}