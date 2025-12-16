using Code.Upgrades.Data;
using EventSystem;
using UnityEngine;

namespace Code.Upgrades.UpgraderItems
{
    public class CannonStatUpgrader : Upgrader
    {
        [SerializeField] private GameEventChannelSO cannonChannel;

        protected override void Upgrade(int currentLevel, UpgradeDataSO data)
        {
            var cannonUpgraderData = data as CannonUpgraderDataSO;

            var evt = CannonEvents.CannonStatUpgradeEvent
                .Initializer(cannonUpgraderData.statType, data.upgradeValue);
            cannonChannel.RaiseEvent(evt);
        }
    }
}