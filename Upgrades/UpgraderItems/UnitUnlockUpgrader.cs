using Code.Upgrades.Data;
using EventSystem;
using UnityEngine;

namespace Code.Upgrades.UpgraderItems
{
    public class UnitUnlockUpgrader : Upgrader
    {
        [SerializeField] private GameEventChannelSO slotChannel;
        
        protected override void Upgrade(int currentLevel, UpgradeDataSO data)
        {
            var unlockData = data as UnitUnlockDataSO;

            var evt = SlotEvents.UnitUnlockToSlotEvent.Initializer(unlockData.unitData);
            slotChannel.RaiseEvent(evt);
        }
    }
}