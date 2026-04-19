using EventSystem;
using Settings.InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Tutorials.TutorialSteps
{
    public class EndTutorialStep : TutorialStep
    {
        [SerializeField] private UIInputSO uiInput;
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private int mainSceneIndex;

        public override void Enter()
        {
            base.Enter();
            uiInput.OnClickPressed += HandleClickUI;
        }

        private void HandleClickUI()
        {
            NextStep();
        }

        private async void HandleFadeComplete(UIFadeCompleteEvent obj)
        {
            PlayerPrefs.SetInt("Tutorial", 1);
            
            uiChannel.RemoveListener<UIFadeCompleteEvent>(HandleFadeComplete);
            await SceneManager.LoadSceneAsync(mainSceneIndex);
        }

        public override void End()
        {
            uiChannel.RaiseEvent(UIEvents.UIFadeStartEvent.Initializer(false));
            uiChannel.AddListener<UIFadeCompleteEvent>(HandleFadeComplete);
            
            uiInput.OnClickPressed -= HandleClickUI;
            base.End();
        }
    }
}