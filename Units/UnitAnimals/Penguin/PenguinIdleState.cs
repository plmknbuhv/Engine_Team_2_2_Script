using Code.Entities;
using Code.Units.State.Friendly;
using Code.Units.UnitStatusEffects;
using Code.Util;
using UnityEngine;

namespace Code.Units.UnitAnimals.Penguin
{
    public class PenguinIdleState : FriendlyIdleState
    {
        private readonly Penguin _penguin;

        public PenguinIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _penguin = entity as Penguin;
            Debug.Assert(_penguin != null, $"penguin is null {entity.gameObject.name}");
        }

        public override void FixedUpdate()
        {
            bool IsTarget(Unit unit) => unit.CanTargeting;
            bool IsFreezeTarget(Unit unit) => UnitStatusUtil.IsTargetStatus(unit, StatusEffectType.FREEZE) && IsTarget(unit);
            
            _detector.SetTarget(UnitTeamType.Enemy);

            bool hasFreezeEnemy = _detector.TryGetClosestEnemy(out Unit enemy, _unit.UnitData.detectRange, IsFreezeTarget);
            
            bool hasNonFreezeEnemy = hasFreezeEnemy == false && _detector.TryGetClosestEnemy(out enemy, _unit.UnitData.detectRange, IsTarget);
            
            if (hasFreezeEnemy || hasNonFreezeEnemy)
            {
                EnemyUnit enemyUnit = enemy as EnemyUnit;
                Debug.Assert(enemyUnit != null, "이거 찾은 애가 에너미가 아닌데?");
                
                enemyUnit.MarkAttackTarget(_friendlyUnit);
                _unit.TargetUnit = enemyUnit;
                ChangeChaseState(hasFreezeEnemy);
            }
        }

        private void ChangeChaseState(bool isFreezeEnemy)
        {
            if (_penguin.IsSwimEnd == false || (_penguin.IsSwimEnd && isFreezeEnemy))
            {
                Debug.Log("Penguin attack");
                _unit.ChangeState("PENGUIN_SWIM");
            }
            else
                base.ChangeChaseState();
        }
    }
}