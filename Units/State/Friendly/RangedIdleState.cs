using Code.Combat.Projectiles;
using Code.Entities;
using Code.Units.UnitDatas;
using Code.Units.UnitStat;
using Code.Util;
using DG.Tweening;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Units.State.Friendly
{
    public abstract class RangedIdleState : FriendlyIdleState
    {
        protected RangedUnitDataSO _rangedUnitData;
        protected TimerCondition _timerCondition;
        protected Tween _jumpTween;
        
        protected UnitStatCompo _statCompo;
        protected Stat _damageStat;
        protected Stat _attackDelayStat;
        
        public RangedIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _statCompo = _unit.GetCompo<UnitStatCompo>();
            
            _damageStat = _statCompo.GetStat(UnitStatType.Damage);
            _attackDelayStat = _statCompo.GetStat(UnitStatType.AttackDelay);
            _rangedUnitData = _unit.UnitData as RangedUnitDataSO;
            _timerCondition = _unit.GetCompo<TimerCondition>();
        }

        public override void Enter()
        {
            base.Enter();
            _timerCondition.StartTimer(_rangedUnitData.rangedAttackDelay);
        }

        public override void Update()
        {
            base.Update();
            
            if (_detector.TryGetNonBattleEnemy(out Unit enemy, _rangedUnitData.rangedAttackRange, UnitTeamType.Enemy) ||
                _detector.TryGetBattleEnemy(out enemy, _rangedUnitData.rangedAttackRange, UnitTeamType.Enemy))
            {
                _unit.TargetUnit = enemy;
                
                if (_timerCondition.CanUseSkill == false) return;
                
                Shoot(enemy);
            }
        }

        protected abstract void Shoot(Unit enemy);

        public override void Exit()
        {
            base.Exit();
            _jumpTween?.Complete();
            _entityAnimator.transform.localPosition = Vector3.zero;
            _timerCondition.ResetCondition();
        }
    }
}