using Code.UI.Logics.Setting.Model;
using Code.UI.Logics.Setting.Presenter;
using Code.UI.Logics.Setting.View;
using EventSystem;
using Settings.InputSystem;
using UnityEngine;

namespace Code.UI.Logics.Setting
{
    public class SettingBootstrap : BaseBootstrap<SettingModel, SettingView, SettingPresenter>
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private UIStateType prevState;
        [SerializeField] private UIInputSO uiInput;
        protected override SettingPresenter CreatePresenter(SettingModel model, SettingView view)
        {
            return new SettingPresenter(model, view, uiChannel, prevState, uiInput);
        }

#if UNITY_EDITOR
        [ContextMenu("Delete Json Data")]
        private void DeleteJsonData()
        {
            SettingModel.DeleteJsonData();
        }
#endif
    }
}