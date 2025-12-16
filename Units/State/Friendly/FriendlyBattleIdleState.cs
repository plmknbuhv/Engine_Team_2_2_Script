using Code.Entities;
using Code.Units.UnitStat;
using UnityEngine;

namespace Code.Units.State.Friendly
{
    public class FriendlyBattleIdleState : UnitState
    {
        private float _timer;
        private UnitDetector _detector;
        private FriendlyUnit _friendlyUnit;
        private Stat _attackDelayStat;
        
        public FriendlyBattleIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _attackDelayStat = _unit.GetCompo<UnitStatCompo>().GetStat(UnitStatType.AttackDelay);
            _detector = _unit.GetCompo<UnitDetector>();
            _friendlyUnit = _unit as FriendlyUnit;
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
            
            if (_friendlyUnit.TargetUnit == null || _friendlyUnit.TargetUnit.IsDead)
            {
                ChangeState("IDLE");
                return;
            }
            
            EnemyUnit enemyUnit = _friendlyUnit.TargetUnit as EnemyUnit;
            Debug.Assert(enemyUnit != null, "적을 가지고 있긴 한데 적이 아님");
            
            // 내가 지금 2ㄷ1 다구리중인데 안 싸우는애가 지나갈 떄 
            if (enemyUnit.TargetUnitList.Count >= 2 &&
                _detector.TryGetNonBattleEnemy(out Unit enemy, _unit.UnitData.detectRange
                    , UnitTeamType.Enemy, target => target.CanTargeting))
            {   // 새로 온 친구로 갈아 탈거임
                
                (enemy as EnemyUnit)?.MarkAttackTarget(_friendlyUnit);
                _friendlyUnit.TargetUnit = enemy;
                ChangeState("CHASE");
            }
            else if (_timer >= (_unit.UnitData.attackDelay * _attackDelayStat.Value) && enemyUnit.CanTargeting) // 다시 공격할 시간일 때
                ChangeState("ATTACK"); // 공격 ㄱㄱ
            else if (_friendlyUnit == null || _friendlyUnit.TargetUnit.IsDead || enemyUnit.CanTargeting == false) // 공격 대기중에 죽으면
                ChangeState("IDLE"); // 대기 ㄱㄱ
        }

        protected virtual void ChangeState(string stateName) => _unit.ChangeState(stateName);
    }
}