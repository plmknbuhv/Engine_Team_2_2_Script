using Code.Entities;
using Code.Units.State.Friendly;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Units.UnitAnimals.Whales
{
    public class WhaleAttackState : FriendlyAttackState
    {
        private readonly int _indexParamHash = Animator.StringToHash("INDEX");
        private readonly Whale _whale;
        private readonly WhaleDataSO _whaleData;

        public WhaleAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _whale = entity as Whale;
            Debug.Assert(_whale != null, $"whale is null : {entity.gameObject.name}");
            _whaleData = _whale.UnitData as WhaleDataSO;
        }

        public override void Enter()
        {
            float targetIndex = _whale.CanUseSkill ? 1f : 0f;
            _entityAnimator.SetParam(_indexParamHash, targetIndex);

            if(_whale.CanUseSkill)
                _whale.OnWaveAttackEvent?.Invoke();
            
            base.Enter();
        }

        protected override void HandleAttack()
        {
            if (_whale.CanUseSkill)
            {
                Wave wave = PoolManagerMono.Instance.Pop<Wave>(_whaleData.waveItem);
                wave.Initialize(_whale, _whaleData.waveDamage, _whaleData.waveSpeed, _whaleData.waveDamageMultiplier);
                _whale.ResetCondition();
            }
            else
            {
                base.HandleAttack();
                _whale.AddAttackCount();
            }
        }
    }
}