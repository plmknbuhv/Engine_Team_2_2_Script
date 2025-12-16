using System;
using System.Collections.Generic;
using Code.Units.UnitDatas;
using Code.Upgrades.Data;
using UnityEngine;

namespace Code.Units.Upgrades
{
    [CreateAssetMenu(fileName = "Unit upgrade", menuName = "SO/Unit/Upgrade/List", order = 0)]
    public class UnitUpgradeSO : UpgradeDataSO
    {
        public UnitPerkUpgradeSO perkUpgrades;
    }
}