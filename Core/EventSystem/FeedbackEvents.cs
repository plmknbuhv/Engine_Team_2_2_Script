using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace EventSystem
{
    public static class FeedbackEvents
    {
        public static readonly ImpulseCameraFeedbackEvent ImpulseCameraFeedbackEvent = new ImpulseCameraFeedbackEvent();
        public static readonly PlayEffectFeedbackEvent PlayEffectFeedbackEvent = new PlayEffectFeedbackEvent();
    }

    public class ImpulseCameraFeedbackEvent : GameEvent
    {
        public float force;

        public ImpulseCameraFeedbackEvent Initializer(float power)
        {
            force = power;
            return this;
        }
    }
    
    public class PlayEffectFeedbackEvent : GameEvent
    {
        public Vector3 position;
        public float effectSize;
        public PoolItemSO effectItem;

        public PlayEffectFeedbackEvent Initializer(PoolItemSO item, Vector3 pos, float size = 1)
        {
            position = pos;
            effectSize = size;
            effectItem = item;
            return this;
        }
    }
}