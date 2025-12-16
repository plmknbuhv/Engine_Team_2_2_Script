using Code.UI.Logics.StateMachine;
using Code.UI.Logics.Upgrade.Model;
using Code.UI.Logics.Upgrade.View;
using Cysharp.Threading.Tasks;
using EventSystem;
using UnityEngine;

namespace Code.UI.Logics.Upgrade.Presenter
{
    public class UpgradePresenter : BasePresenter<UpgradeModel, UpgradeView>
    {
        private GameEventChannelSO _uiChannel;
        private GameEventChannelSO _levelChannel;
        private GameEventChannelSO _upgradeChannel;

        public UpgradePresenter(UpgradeModel model, UpgradeView view,
            GameEventChannelSO uiChannel, GameEventChannelSO levelChannel,
            GameEventChannelSO upgradeChannel) : base(model, view)
        {
            _uiChannel = uiChannel;
            _levelChannel = levelChannel;
            _upgradeChannel = upgradeChannel;

            view.OnCardSelected += HandleCardSelected;
            view.OnRerollSelected += HandleRerollSelected;

            _levelChannel.AddListener<ChangeLevelEvent>(HandleChangeLevel);
            _upgradeChannel.AddListener<SelectRandomUpgradesEvent>(HandleSelectRandomUpgrades);
        }

        public override void Dispose()
        {
            base.Dispose();

            View.OnCardSelected -= HandleCardSelected;
            View.OnRerollSelected -= HandleRerollSelected;

            _levelChannel.RemoveListener<ChangeLevelEvent>(HandleChangeLevel);
            _upgradeChannel.RemoveListener<SelectRandomUpgradesEvent>(HandleSelectRandomUpgrades);
        }

        private void HandleRerollSelected()
        {
            _upgradeChannel.RaiseEvent(UpgradeEvents.RequestSelectUpgradesEvent);
        }

        private void HandleSelectRandomUpgrades(SelectRandomUpgradesEvent obj)
        {
            Debug.Log("UpgradePresenter HandleSelectRandomUpgrades");
            Model.Cards = obj.upgrades;
        }

        private async void HandleCardSelected(int obj)
        {
            var selectedUpgrade = Model.Cards[obj];
            _upgradeChannel.RaiseEvent(UpgradeEvents.TargetUpgradeEvent.Initializer(selectedUpgrade));
            if (--Model.RemainingUpgrades > 0)
            {
                await View.OnExit();
                _upgradeChannel.RaiseEvent(UpgradeEvents.RequestSelectUpgradesEvent);
                View.OnEnter().Forget();
            }
            else
            {
                _uiChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer(UIStateType.MainHud));
            }
        }

        private void HandleChangeLevel(ChangeLevelEvent obj)
        {
            if (obj.currentLevel == 0) return;
            Debug.Log("UpgradePresenter HandleChangeLevel");
            Model.RemainingUpgrades++;
            if (Model.RemainingUpgrades == 1)
            {
                _uiChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer(UIStateType.Upgrade));
                _upgradeChannel.RaiseEvent(UpgradeEvents.RequestSelectUpgradesEvent);
            }
        }

        protected override void OnModelChanged()
        {
            View.SetCards(Model.Cards).Forget();
        }
    }
}