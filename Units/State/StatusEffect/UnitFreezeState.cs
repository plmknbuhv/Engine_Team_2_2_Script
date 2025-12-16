using Code.Effects;
using Code.Entities;
using Code.Units.UnitStatusEffects;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Units.State.StatusEffect
{
    public class UnitFreezeState : UnitState
    {
        private EntityAnimator _animator;
        private UnitStatusEffect _statusEffect;
        private float _currentTime;
        
        public UnitFreezeState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _animator = entity.GetCompo<EntityAnimator>();
            _statusEffect = entity.GetCompo<UnitStatusEffect>();
        }

        public override void Enter()
        {
            base.Enter();
            //Debug.Log("Freeze Start");
            
            _unit.CanChangeState = false;
            
            _animator.StopPlayAnimator();
            _currentTime = 0;
            PlayEffect();
        }

        public override void Update()
        {
            base.Update();
            const string idle = "IDLE";
            const string walk = "WALK";
            const string attack = "ATTACK";
            const string dead = "DEAD";

            if (_unit.IsDead)
            {
                _unit.CanChangeState = true;
                _unit.ChangeState(dead);
                return;
            }
            
            _currentTime += Time.deltaTime;

            if (_statusEffect.Duration <= _currentTime)
            {
                //Debug.Log("Freeze End");
                _unit.CanChangeState = true;
                //적, 아군 상태이상이 다를 거 없을 것 같아서 이렇게 작성
                if(_unit is FriendlyUnit)
                    _unit.ChangeState(idle);
                else if (_unit is EnemyUnit enemy)
                {
                    if (enemy.IsInBattle)
                        _unit.ChangeState(attack);
                    else
                        _unit.ChangeState(walk);
                }
                else
                {
                    Debug.LogAssertion("현재 Entity은 Unit이 아닙니다.");
                }
            }
        }
        
        private void PlayEffect()
        {
            PoolingEffect skillEffect = PoolManagerMono.Instance.Pop<PoolingEffect>(_statusEffect.CurrentStatusEffectData.effectPoolItem);
            skillEffect.SetDuration(_statusEffect.Duration);
            skillEffect.PlayVFX(_unit.transform.position, Quaternion.identity);
        }
        
        public override void Exit()
        {
            base.Exit();
            //Debug.Log("Freeze Exit");
            _animator.StartPlayAnimator();
            _unit.ApplyStatusEffect(StatusEffectType.DEFAULT);
            _currentTime = 0;
        }
    }
}