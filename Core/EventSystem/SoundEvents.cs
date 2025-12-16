using Ami.BroAudio;

namespace EventSystem
{
    public static class SoundEvents
    {
        public static readonly PlaySFXSoundEvent PlaySFXSoundEvent = new PlaySFXSoundEvent();
        public static readonly PlayLongSFXSoundEvent PlayLongSFXSoundEvent = new PlayLongSFXSoundEvent();
        public static readonly StopLongSFXSoundEvent StopLongSFXSoundEvent = new StopLongSFXSoundEvent();
    }

    public class PlaySFXSoundEvent : GameEvent
    {
        public SoundID soundID;

        public PlaySFXSoundEvent Initializer(SoundID id)
        {
            soundID = id;
            
            return this;
        }
    }
    
    public class PlayLongSFXSoundEvent : GameEvent
    {
        public SoundID soundID;
        public string stopKey;
        
        public PlayLongSFXSoundEvent Initializer(SoundID id, string key)
        {
            soundID = id;
            stopKey = key;
            
            return this;
        }
    }
    
    public class StopLongSFXSoundEvent : GameEvent
    {
        public string stopKey;
        
        public StopLongSFXSoundEvent Initializer(string key)
        {
            stopKey = key;
            
            return this;
        }
    }
}