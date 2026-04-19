using Ami.BroAudio;
using EventSystem;
using UnityEngine;

namespace Code.UI.Visual.Feedback
{
    public class UISoundFeedback : MonoBehaviour, IUIFeedback
    {
        [field: SerializeField] public string FeedbackName { get; private set; } = "click";
        [SerializeField] private SoundID soundID;
        [SerializeField] private GameEventChannelSO soundChannel;
        public void Initialize(GameObject tar) { }

        public void Play()
        {
            soundChannel.RaiseEvent(SoundEvents.PlaySFXSoundEvent.Initializer(soundID));
        }
    }
}