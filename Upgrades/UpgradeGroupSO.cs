using System;
using System.Collections.Generic;
using Code.Upgrades.Data;
using UnityEngine;

namespace Code.Upgrades
{
    [CreateAssetMenu(fileName = "UpgradeGroupData", menuName = "SO/Upgrade/UpgradeGroup", order = 0)]
    public class UpgradeGroupSO : ScriptableObject, ICloneable
    {
        public List<UpgradeDataSO> upgradeTargets;
        public object Clone()
        {
            return Instantiate(this);
        }
    }
}       