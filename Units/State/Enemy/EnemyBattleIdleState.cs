using Code.Entities;
using Code.Units.UnitStat;
using UnityEngine;

namespace Code.Units.State.Enemy
{
    public class EnemyBattleIdleState : UnitState
    {
        private Stat _attackDelayStat;
        private float _timer;
        
        public EnemyBattleIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _attackDelayStat = _unit.GetCompo<UnitStatCompo>().GetStat(UnitStatType.AttackDelay);
        }

        public override void Enter()
        {
            base.Enter();
            _timer = 0;
        }

        public override void Update()
        {
            base.Update();

            _timer += Time.deltaTime;

            if (_timer >= (_unit.UnitData.attackDelay * _attackDelayStat.Value)) // 다시 공격할 시간일 때
            {
                _unit.ChangeState("ATTACK");
            }
        }
    }
}