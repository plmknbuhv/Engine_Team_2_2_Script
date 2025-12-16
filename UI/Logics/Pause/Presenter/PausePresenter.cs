using Code.UI.Logics.Pause.Model;
using Code.UI.Logics.Pause.View;
using EventSystem;
using Settings.InputSystem;
using UnityEngine;

namespace Code.UI.Logics.Pause.Presenter
{
    public class PausePresenter : BasePresenter<PauseModel, PauseView>
    {
        private UIInputSO _uiInputSO;
        private GameEventChannelSO _uiChannel;
        
        private bool _isShowing;
        private bool _isSetting;

        public PausePresenter(PauseModel model, PauseView view, UIInputSO uiInput, GameEventChannelSO uiChannel) :
            base(model, view)
        {
            _uiInputSO = uiInput;
            _uiChannel = uiChannel;

            _uiInputSO.OnCancelPressed += HandleCancelPressed;
            view.OnResumeButtonClicked += Resume;
            view.OnSettingButtonClicked += HandleSettingButtonClicked;
            view.OnQuitButtonClicked += HandleQuitButtonClicked;
            view.OnShowingChanged += HandleShowingChanged;
        }

        public override void Dispose()
        {
            base.Dispose();
            _uiInputSO.OnCancelPressed -= HandleCancelPressed;
            View.OnResumeButtonClicked -= Resume;
            View.OnSettingButtonClicked -= HandleSettingButtonClicked;
            View.OnQuitButtonClicked -= HandleQuitButtonClicked;
            View.OnShowingChanged -= HandleShowingChanged;
        }

        private void HandleShowingChanged(bool obj)
        {
            _isShowing = obj;
            if (_isShowing && _isSetting)
            {
                _isSetting = false;
                _uiInputSO.OnCancelPressed += HandleCancelPressed;
            }
        }

        private void HandleCancelPressed()
        {
            if (_isShowing)
            {
                Resume();
            }
            else
            {
                _uiChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer(UIStateType.Pause));
                Time.timeScale = 0;
            }
        }

        private void Resume()
        {
            _uiChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer());
            Time.timeScale = 1;
        }

        private void HandleSettingButtonClicked()
        {
            _uiChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer(UIStateType.Setting));
            _isSetting = true;
            _uiInputSO.OnCancelPressed -= HandleCancelPressed;
        }

        private void HandleQuitButtonClicked()
        {
            Application.Quit();
        }

        protected override void OnModelChanged()
        {
        }
    }
}
