using System;
using Code.Upgrades.Data;
using UnityEngine;

namespace Code.Upgrades
{
    public abstract class Upgrader : MonoBehaviour
    {
        [SerializeField] protected UpgradeEventInvokerSO upgradeInvoker;
        [SerializeField] protected UpgradeDataSO[] upgradeDatas;

        protected virtual void Awake()
        {
            foreach (var upgrade in upgradeDatas)
            {
                upgradeInvoker.Subscribe(upgrade, Upgrade);
            }
        }

        protected virtual void OnDestroy()
        {
            foreach (var upgrade in upgradeDatas)
            {
                upgradeInvoker.Unsubscribe(upgrade, Upgrade);
            }
        }

        protected abstract void Upgrade(int currentLevel, UpgradeDataSO data);
    }
}