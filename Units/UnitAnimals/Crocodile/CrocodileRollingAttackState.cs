using System.Collections.Generic;
using Code.Effects;
using Code.Entities;
using Code.Units.State;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Units.UnitAnimals.Crocodile
{
    public class CrocodileRollingAttackState : UnitState
    {
        private readonly Crocodile _crocodile;
        private readonly CrocodileDataSo _crocodileData;
        private readonly UnitDetector _unitDetector;

        public CrocodileRollingAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _crocodile = entity as Crocodile;
            Debug.Assert(_crocodile != null, $"Crocodile is null : {entity.gameObject.name}");

            _crocodileData = _crocodile.UnitData as CrocodileDataSo;
            _unitDetector = entity.GetCompo<UnitDetector>();
        }

        public override void Enter()
        {
            base.Enter();

            PlayEffect();

            _animatorTrigger.OnAttackTrigger += HandleAttack;
        }

        public override void Update()
        {
            base.Update();

            if (_isTriggerCall)
            {
                _crocodile.ChangeState("BATTLEIDLE");
                _crocodile.ResetAttackCount();
            }
        }

        public override void Exit()
        {
            base.Exit();
            _crocodile.ResetAttackCount();
            _animatorTrigger.OnAttackTrigger -= HandleAttack;
        }

        private void PlayEffect()
        {
            PoolingEffect skillEffect = PoolManagerMono.Instance.Pop<PoolingEffect>(_crocodileData.skillEffect);
            skillEffect.PlayVFX(_unit.transform.position, _crocodile.transform.rotation);
            skillEffect.transform.rotation = _crocodile.transform.rotation * Quaternion.Euler(0, 20, 0);
        }

        private void HandleAttack()
        {
            if (_unitDetector.TryGetUnits(out HashSet<Unit> units, _crocodileData.skillRange, UnitTeamType.Friendly))
            {
                foreach (Unit unit in units)
                {
                    unit.ApplyDamage(_crocodileData.damage, _unit);
                }
            }
        }
    }
}