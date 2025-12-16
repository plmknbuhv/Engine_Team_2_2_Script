using EventSystem;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Feedbacks
{
    public class EffectFeedback : Feedback
    {
        [SerializeField] private GameEventChannelSO feedbackChannel;
        [SerializeField] private Transform spawnTrm;
        [SerializeField] private PoolItemSO effectItem;
        
        public override void CreateFeedback()
        {
            var evt = FeedbackEvents.PlayEffectFeedbackEvent.Initializer(effectItem, spawnTrm.position);
            feedbackChannel.RaiseEvent(evt);
        }
    }
}