using Code.Entities;
using Code.Units.State.Friendly;
using Code.Units.UnitStat;
using UnityEngine;

namespace Code.Units.UnitAnimals.Walruses
{
    public class WalrusIdleState : FriendlyIdleState
    {
        private Unit _beforeTarget;
        
        private readonly Walrus _walrus;
        private readonly WalrusDataSO _walrusData;
        private readonly string _attackDelayKey = "Walrus";
        
        public WalrusIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _walrus = entity as Walrus;
            Debug.Assert(_walrus != null, $"Walrus is null : {entity.gameObject.name}");
            _walrusData = _walrus.UnitData as WalrusDataSO;
        }

        protected override void ChangeChaseState()
        {
            if (_beforeTarget != null && _unit.TargetUnit != _beforeTarget)
            {
                Stat stat = GetAttackDelayStat(_beforeTarget);
                stat?.RemoveModifier(_attackDelayKey);

                _beforeTarget = _unit.TargetUnit;
            }
            else if (_unit.TargetUnit != null)
            {
                Stat stat = GetAttackDelayStat(_unit.TargetUnit);
                stat?.AddModifier(_attackDelayKey, _walrusData.atkDelayDecreaseAmount);
            }

            if (_walrus.TryGetFreezeEnemy(out Unit target))
            {
                _walrus.StartChaseToBiteTarget(target);
            }
            
            base.ChangeChaseState();
        }

        private Stat GetAttackDelayStat(Unit target)
        {
            UnitStatCompo targetStatCompo = target.GetCompo<UnitStatCompo>();
            if (targetStatCompo.TryGetStat(UnitStatType.AttackDelay, out Stat stat))
            {
                return stat;
            }

            return null;
        }
    }
}