
namespace EventSystem
{
    public static class WaveEvents
    {
        public static readonly StartWaveEvent StartWaveEvent = new StartWaveEvent();
        public static readonly StartBossWaveEvent StartBossWaveEvent = new StartBossWaveEvent();
        public static readonly BossWaveCompleteEvent BossWaveCompleteEvents = new BossWaveCompleteEvent();
        public static readonly AllWaveCompleteEvent AllWaveCompleteEvent = new AllWaveCompleteEvent();
    }
    
    //웨이브 시작 시 발생
    public class StartWaveEvent : GameEvent
    {
        public int waveCount;
        public int allEnemyCount;
        
        public StartWaveEvent Initializer(int waveCount, int allEnemyCount)
        {
            this.waveCount = waveCount;
            this.allEnemyCount = allEnemyCount;
            return this;
        }
    }
    
    //보스 웨이브 클리어했을 때 발생
    public class BossWaveCompleteEvent : GameEvent
    {
        public int bossWaveCount;
        
        public BossWaveCompleteEvent Initializer(int bossWaveCount)
        {
            this.bossWaveCount = bossWaveCount;
            return this;
        }
    }
    
    //보스 웨이브 시작 시 발생
    public class StartBossWaveEvent : GameEvent
    {
        public StartBossWaveEvent Initializer()
        {
            return this;
        }
    }
    
    //모든 웨이브 클리어 시 발생
    public class AllWaveCompleteEvent : GameEvent
    {
        public AllWaveCompleteEvent Initializer()
        {
            return this;
        }
    }
}