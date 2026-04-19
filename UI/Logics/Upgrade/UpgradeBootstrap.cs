using Code.UI.Logics.Upgrade.Model;
using Code.UI.Logics.Upgrade.Presenter;
using Code.UI.Logics.Upgrade.View;
using EventSystem;
using UnityEngine;

namespace Code.UI.Logics.Upgrade
{
    public class UpgradeBootstrap : BaseBootstrap<UpgradeModel, UpgradeView, UpgradePresenter>
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private GameEventChannelSO levelChannel;
        [SerializeField] private GameEventChannelSO upgradeChannel;
        protected override UpgradePresenter CreatePresenter(UpgradeModel model, UpgradeView view)
        {
            return new UpgradePresenter(model, view, uiChannel, levelChannel, upgradeChannel);
        }
    }
}