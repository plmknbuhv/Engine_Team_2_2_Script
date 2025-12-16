using Code.Combat.Projectiles;
using Code.Entities;
using Code.Units.State.Friendly;
using DG.Tweening;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Units.UnitAnimals.Chickens
{
    public class ChickenIdleState : RangedIdleState
    {
        private Chicken _chicken;
        private readonly float _damageDecreaseAmount = -0.15f;
        
        public ChickenIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _chicken = _unit as Chicken;
        }

        protected override void Shoot(Unit enemy)
        {
            Debug.Log("쏩니다.");
            _timerCondition.StartTimer(_rangedUnitData.rangedAttackDelay * _attackDelayStat.Value);
                
            _jumpTween = _entityAnimator.transform.DOLocalMoveY(0.5f, 0.35f)
                .SetLoops(2, LoopType.Yoyo);
            
            ThrowEgg(enemy, 0);
            SequenceShoot(enemy);
        }

        private async void SequenceShoot(Unit enemy)
        {
            for (int i = 0; i < _chicken.extraEggCnt; i++)
            {
                await Awaitable.WaitForSecondsAsync(0.2f);
                ThrowEgg(enemy, i + 1);
            }
        }

        private void ThrowEgg(Unit enemy, int idx)
        {
            Egg egg = PoolManagerMono.Instance.Pop<Egg>(_rangedUnitData.projectileItem);
            int damage = Mathf.RoundToInt(
                _rangedUnitData.damage * (_rangedUnitData.rangedAttackCoefficient + _damageDecreaseAmount * idx) * _damageStat.Value);
                    
            egg.SetTarget(_friendlyUnit, enemy as EnemyUnit, damage);
        }
    }
}