using Code.Combat.Projectiles;
using Code.Entities;
using Code.Units.State.Friendly;
using Code.Units.UnitAnimals.Giraffes;
using DG.Tweening;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Units.UnitAnimals.Monkeys
{
    public class MonkeyIdleState : RangedIdleState
    {
        public MonkeyIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public override void Update()
        {
            if (_detector.TryGetClosestEnemy<Giraffe>(out var giraffe, 2, null, UnitTeamType.Friendly))
            {
                if (giraffe.Monkey != null) return;
                if (giraffe.GetCurrentState() is not FriendlyIdleState) return;
                if (giraffe.IsAlreadySpin) return;
                
                giraffe.TakeGiraffe(_unit as Monkey);
                _unit.ChangeState("SPIN");
                
                return;
            }
            
            base.Update(); // 원거리 공격 감지

            if (_unit.TargetUnit != null)
                _movement.LookAtTarget(_unit.TargetUnit.transform.position);
        }

        protected override void Shoot(Unit enemy)
        {
            _timerCondition.StartTimer(_rangedUnitData.rangedAttackDelay * _attackDelayStat.Value);
            
            _jumpTween = _entityAnimator.transform.DOLocalMoveY(0.5f, 0.3f)
                .SetLoops(2, LoopType.Yoyo);
            
            Banana banana = PoolManagerMono.Instance.Pop<Banana>(_rangedUnitData.projectileItem);
            int damage = Mathf.RoundToInt(
                _rangedUnitData.damage * _rangedUnitData.rangedAttackCoefficient * _damageStat.Value);
                        
            banana.SetTarget(_friendlyUnit, enemy as EnemyUnit, damage);
        }
    }
}