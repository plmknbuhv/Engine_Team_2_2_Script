using Code.Units;
using Enemies;

namespace EventSystem
{
    public enum EnemyRegistry
    {
        Add,
        Remove
    }
    public static class EnemyEvents
    {
        public static readonly EnemyListChangedEvent EnemyListChangedEvent = new EnemyListChangedEvent();
        public static readonly EnemyPathCompleteEvent EnemyPathCompleteEvent = new EnemyPathCompleteEvent();
        public static readonly BossDeathEvent BossDeathEvent = new BossDeathEvent();
        public static readonly EnemyDeathEvent EnemyDeathEvent = new EnemyDeathEvent();
        public static readonly EnemySpawnEvent EnemySpawnEvent = new EnemySpawnEvent();
    }
    
    public class EnemyListChangedEvent : GameEvent
    {
        public EnemyUnit enemy;
        public EnemyRegistry enemyRegistry;
        
        public EnemyListChangedEvent Initializer(EnemyUnit enemy, EnemyRegistry enemyRegistry)
        {
            this.enemy = enemy;
            this.enemyRegistry = enemyRegistry;
            return this;
        }
    }
    
    //경로를 끝까지 가면 발생
    public class EnemyPathCompleteEvent : GameEvent
    {
        public EnemyType enemyType;
        
        public EnemyPathCompleteEvent Initializer(EnemyType enemyType)
        {
            this.enemyType = enemyType;
            return this;
        }
    }
    
    public class BossDeathEvent : GameEvent
    {
        public EnemyUnit enemy;
        public BossDeathEvent Initializer(EnemyUnit enemy)
        {
            this.enemy = enemy;
            return this;
        }
    }
    
    public class EnemyDeathEvent : GameEvent
    {
        public EnemyUnit enemy;
        public EnemyDeathEvent Initializer(EnemyUnit enemy)
        {
            this.enemy = enemy;
            return this;
        }
    }
    
    public class EnemySpawnEvent : GameEvent
    {
        public EnemyUnit enemy;
        public EnemySpawnEvent Initializer(EnemyUnit enemy)
        {
            this.enemy = enemy;
            return this;
        }
    }
}