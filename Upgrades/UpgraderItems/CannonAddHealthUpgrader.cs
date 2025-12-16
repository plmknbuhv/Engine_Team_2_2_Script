using Code.Upgrades.Data;
using EventSystem;
using UnityEngine;

namespace Code.Upgrades.UpgraderItems
{
    public class CannonAddHealthUpgrader : Upgrader
    {
        [SerializeField] private GameEventChannelSO cannonCannel;
        
        protected override void Upgrade(int currentLevel, UpgradeDataSO data)
        {
            var evt = CannonEvents.CannonAddHealthEvent.Initializer((int)data.upgradeValue);
            cannonCannel.RaiseEvent(evt);
        }
    }
}