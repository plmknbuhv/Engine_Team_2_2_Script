using Code.UI.Logics.Pause.Model;
using Code.UI.Logics.Pause.Presenter;
using Code.UI.Logics.Pause.View;
using EventSystem;
using Settings.InputSystem;
using UnityEngine;

namespace Code.UI.Logics.Pause
{
    public class PauseBootstrap : BaseBootstrap<PauseModel, PauseView, PausePresenter>
    {
        [SerializeField] private UIInputSO uiInputSO;
        [SerializeField] private GameEventChannelSO uiEventChannel;
        protected override PausePresenter CreatePresenter(PauseModel model, PauseView view)
        {
            return new PausePresenter(model, view, uiInputSO, uiEventChannel);
        }
    }
}