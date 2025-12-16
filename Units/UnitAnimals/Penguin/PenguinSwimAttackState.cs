using Code.Effects;
using Code.Entities;
using Code.Units.State.Friendly;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Units.UnitAnimals.Penguin
{
    public class PenguinSwimAttackState : FriendlyAttackState
    {
        private readonly Penguin _penguin;
        private readonly PenguinDataSO _penguinData;
        
        public PenguinSwimAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _penguin = entity as Penguin;
            Debug.Assert(_penguin != null, $"penguin is null {entity.gameObject.name}");
            _penguinData = _penguin.UnitData as PenguinDataSO;
        }

        public override void Enter()
        {
            base.Enter();

            if (_penguinData.swimAttackEffect != null)
            {
                PoolingEffect effect = PoolManagerMono.Instance.Pop<PoolingEffect>(_penguinData.swimAttackEffect);
                Vector3 targetPos = _unit.TargetUnit.transform.position;
                effect.PlayVFX(targetPos + _penguinData.effectOffset, Quaternion.identity);
            }
            
            _penguin.OnSwimAttack?.Invoke();
        }

        protected override void HandleAttack()
        {
            _unit.TargetUnit.ApplyDamage(_penguinData.swimAttackDamage, _unit);
        }
    }
}