using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.UI.Visual.Feedback
{
    public class UIPunchFeedback : MonoBehaviour, IUIFeedback
    {
        [field: SerializeField] public string FeedbackName { get; private set; }
        [SerializeField] private float duration = 0.25f;
        [SerializeField] private float magnitude = 10f;
        
        private bool _isInitialized = false;
        
        private RectTransform _target;

        private async void Start()
        {
            try
            {
                await UniTask.WaitForEndOfFrame();
                _isInitialized = true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error initializing UIPunchFeedback: {e.Message}");
            }
        }

        public void Initialize(GameObject tar)
        {
            _target = tar.GetComponent<RectTransform>();
        }

        public void Play()
        {
            if (!_isInitialized) return;
            _target.DOPunchScale(new Vector2(magnitude, magnitude), duration); 
        }
    }
}