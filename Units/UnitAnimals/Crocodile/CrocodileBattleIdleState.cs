using Code.Entities;
using Code.Units.State.Enemy;
using Code.Units.UnitStat;
using Code.Util;

namespace Code.Units.UnitAnimals.Crocodile
{
    public class CrocodileBattleIdleState : EnemyBattleIdleState
    {
        private Stat _attackDelayStat;
        private CountCondition _countCondition;
        private float _timer;
        
        public CrocodileBattleIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _attackDelayStat = _unit.GetCompo<UnitStatCompo>().GetStat(UnitStatType.AttackDelay);
            _countCondition = entity.GetCompo<CountCondition>();
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            base.Update();

            if (_countCondition.CanUseSkill)
            {
                _unit.ChangeState("ROLLING_ATTACK");
            }
        }
    }
}