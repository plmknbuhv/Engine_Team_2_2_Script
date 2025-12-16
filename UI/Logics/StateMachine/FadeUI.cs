using System;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using EventSystem;
using UnityEngine;

namespace Code.UI.Logics.StateMachine
{
    public class FadeUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO uiEventChannel;
        [SerializeField] private UIElement fadeUIElement;
        [SerializeField] private bool startVisible = false;

        private const string ShowKey = "show";
        private const string FadeKey = "fade";

        private void Awake()
        {
            uiEventChannel.AddListener<UIFadeStartEvent>(HandleUIFadeStart);
        }

        private void OnDestroy()
        {
            uiEventChannel.RemoveListener<UIFadeStartEvent>(HandleUIFadeStart);
        }

        private async void Start()
        {
            if (startVisible)
            {
                await fadeUIElement.AddState(ShowKey, 10);
                await UniTask.WaitForSeconds(0.1f);
                fadeUIElement.RemoveState(ShowKey).Forget();
            }
        }

        private void HandleUIFadeStart(UIFadeStartEvent obj)
        {
            if (obj.IsFadeIn)
                FadeIn();
            else
                FadeOut();
        }

        private async void FadeOut()
        {
            fadeUIElement.PlayFeedback(FadeKey);
            await fadeUIElement.AddState(ShowKey, 10, true);
            uiEventChannel.RaiseEvent(UIEvents.UIFadeCompleteEvent.Initializer(false));
        } 
        
        private async void FadeIn()
        {
            await fadeUIElement.RemoveState(ShowKey);
            uiEventChannel.RaiseEvent(UIEvents.UIFadeCompleteEvent.Initializer(true));
        }
    }
}