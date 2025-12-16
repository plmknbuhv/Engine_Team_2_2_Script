using Code.Combat;
using Code.Effects;
using Code.Entities;
using Code.Units.State.Friendly;
using Code.Units.UnitStat;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Units.UnitAnimals.Walruses
{
    public class WalrusBiteState : FriendlyAttackState
    {
        private readonly Walrus _walrus;
        private readonly WalrusDataSO _walrusData;
        
        public WalrusBiteState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _walrus = entity as Walrus;
            Debug.Assert(_walrus != null, $"Walrus is null : {entity}");
            _walrusData = _walrus.UnitData as WalrusDataSO;
        }

        protected override void HandleAttack()
        {
            base.HandleAttack();

            Unit target = _walrus.TargetUnit;
            
            if(target == null || target.IsDead) return;
            
            if (_walrusData.biteAttackEffect)
            {
                PoolingEffect effect = PoolManagerMono.Instance.Pop<PoolingEffect>(_walrusData.biteAttackEffect);
                effect.PlayVFX(target.transform.position, Quaternion.identity);
            }
            
            _walrus.OnWalrusBite?.Invoke();

            UnitHealth health = target.GetCompo<UnitHealth>();

            if (health != null)
            {
                target.ApplyDamage(health.CurrentHealth, _walrus);
            }
        }

        public override void Exit()
        {
            base.Exit();
            
            _walrus.StopChaseToBiteTarget();
        }
    }
}