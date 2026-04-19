using EventSystem;
using Unity.Cinemachine;
using UnityEngine;

namespace Code.Feedbacks
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class ImpulseFeedback : Feedback
    {
        [SerializeField] private GameEventChannelSO feedbackChannel;
        [SerializeField] private float impulsePower = 0.5f;
        
        public override void CreateFeedback()
        {
            var evt = FeedbackEvents.ImpulseCameraFeedbackEvent.Initializer(impulsePower);
            feedbackChannel.RaiseEvent(evt);
        }
    }
}