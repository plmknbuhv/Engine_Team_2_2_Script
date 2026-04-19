using Code.Effects;
using EventSystem;
using GondrLib.Dependencies;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Managers
{
    public class EffectManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO feedbackChannel;
        [Inject] private PoolManagerMono poolM;
        
        private void Awake()
        {
            feedbackChannel.AddListener<PlayEffectFeedbackEvent>(HandlePlayEffectFeedback);
        }

        private void OnDestroy()
        {
            feedbackChannel.RemoveListener<PlayEffectFeedbackEvent>(HandlePlayEffectFeedback);
        }

        private void HandlePlayEffectFeedback(PlayEffectFeedbackEvent evt)
        {
            var effect = poolM.Pop<PoolingEffect>(evt.effectItem);
            effect.transform.localScale = Vector3.one * evt.effectSize;
            
            effect.PlayVFX(evt.position, Quaternion.identity);
        }
    }
}