using Ami.BroAudio;
using EventSystem;
using UnityEngine;

namespace Code.Feedbacks
{
    public class SoundFeedback : Feedback
    {
        [SerializeField] private GameEventChannelSO soundChannel;
        [SerializeField] private SoundID soundID;
        
        public override void CreateFeedback()
        {
            var evt = SoundEvents.PlaySFXSoundEvent.Initializer(soundID);
            soundChannel.RaiseEvent(evt);
        }
    }
}