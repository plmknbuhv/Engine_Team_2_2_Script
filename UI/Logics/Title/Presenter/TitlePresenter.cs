using Code.UI.Logics.StateMachine;
using Code.UI.Logics.Title.Model;
using Code.UI.Logics.Title.View;
using Cysharp.Threading.Tasks;
using EventSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.UI.Logics.Title.Presenter
{
    public class TitlePresenter : BasePresenter<TitleModel, TitleView>
    {
        private GameEventChannelSO _uiEventChannel;

        public TitlePresenter(TitleModel model, TitleView view, GameEventChannelSO uiEvent) : base(model, view)
        {
            _uiEventChannel = uiEvent;
            view.OnButtonClicked += HandleButtonClicked;
            
            int didPlay = PlayerPrefs.GetInt("Tutorial", 0);
            Debug.Log("Did play tutorial: " + didPlay);
            model.InGameSceneBuildIndex = didPlay + 1;
        }

        public override void Dispose()
        {
            base.Dispose();
            View.OnButtonClicked -= HandleButtonClicked;
        }

        private void HandleButtonClicked(TitleButtonType buttonType)
        {
            switch (buttonType)
            {
                case TitleButtonType.Start:
                {
                    View.HandleStartButtonClicked(Model.InGameSceneBuildIndex);
                    _uiEventChannel.RaiseEvent(UIEvents.UIFadeStartEvent.Initializer(false));
                    _uiEventChannel.AddListener<UIFadeCompleteEvent>(HandleUIFadeCompleteEvent);
                }
                    break;
                case TitleButtonType.Setting:
                    _uiEventChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer(UIStateType.Setting));
                    break;
                case TitleButtonType.Exit:
                    Application.Quit();
                    break;
            }
        }

        private void HandleUIFadeCompleteEvent(UIFadeCompleteEvent obj)
        {
            _uiEventChannel.RemoveListener<UIFadeCompleteEvent>(HandleUIFadeCompleteEvent);
            Debug.Log("Loading scene with build index: " + Model.InGameSceneBuildIndex);
            View.IsTransitionEnded = true;
        }


        protected override void OnModelChanged()
        {
        }
    }
}
