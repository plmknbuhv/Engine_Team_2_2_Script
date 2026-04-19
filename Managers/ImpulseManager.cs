using EventSystem;
using Unity.Cinemachine;
using UnityEngine;

namespace Code.Managers
{
    public class ImpulseManager : MonoBehaviour
    {
        [SerializeField] private CinemachineImpulseSource source;
        [SerializeField] private GameEventChannelSO feedbackChannel;

        private void Awake()
        {
            feedbackChannel.AddListener<ImpulseCameraFeedbackEvent>(HandlePlayEffectFeedback);
        }

        private void OnDestroy()
        {
            feedbackChannel.RemoveListener<ImpulseCameraFeedbackEvent>(HandlePlayEffectFeedback);
        }

        private void HandlePlayEffectFeedback(ImpulseCameraFeedbackEvent evt)
        {
            source.GenerateImpulse(evt.force);
        }
    }
}