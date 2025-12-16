using Code.Entities;
using Enemies;
using EventSystem;

namespace Code.Units.State.Enemy
{
    public class EnemyDeadState : UnitDeadState
    {
        private EnemyUnit _enemyUnit;
        
        public EnemyDeadState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _enemyUnit = _unit as EnemyUnit;
        }

        public override void Enter()
        {
            base.Enter();
            if (_enemyUnit.TargetUnitList.Count > 0) // 만약에 싸우던 애가 있으면 타겟 클리어
                _enemyUnit.TargetUnitList.Clear();
            
            _enemyUnit.Collider.enabled = false;
            
            _enemyUnit.EnemyChannel.RaiseEvent(EnemyEvents.EnemyListChangedEvent.Initializer(_enemyUnit, EnemyRegistry.Remove));
            _enemyUnit.LevelChannel.RaiseEvent(LevelEvents.AddExpEvent.Initializer(_enemyUnit.EnemyData.dropExpCount));
            
            if (_enemyUnit.IsBoss)
            {
                _enemyUnit.EnemyChannel.RaiseEvent(EnemyEvents.BossDeathEvent.Initializer(_enemyUnit));
            }
            
            _enemyUnit.EnemyChannel.RaiseEvent(EnemyEvents.EnemyDeathEvent.Initializer(_enemyUnit));
            _unit.UnitCounter.RemoveUnit(UnitTeamType.Enemy, _unit);
        }
    }
}