using Code.Entities;
using Code.Units.State.Friendly;
using Code.Units.UnitStatusEffects;
using Code.Util;
using Enemies;
using UnityEngine;

namespace Code.Units.UnitAnimals.Walruses
{
    public class WalrusChaseState : UnitChaseState
    {
        private readonly Walrus _walrus;
        
        public WalrusChaseState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _walrus = entity as Walrus;
            Debug.Assert(_walrus != null, $"Walrus is null : {entity.gameObject.name}");
        }

        protected override void ChangeAttackState()
        {
            if (UnitStatusUtil.IsTargetStatus(_unit.TargetUnit, StatusEffectType.FREEZE))
            {
                bool isTarget = _unit.TargetUnit.UnitData is EnemyDataSo { enemyType: EnemyType.Common };
                
                if(isTarget)
                    _walrus.ChangeState("BITE");
            }
            else
            {
                _walrus.StopChaseToBiteTarget();
                base.ChangeAttackState();
            }
        }
    }
}