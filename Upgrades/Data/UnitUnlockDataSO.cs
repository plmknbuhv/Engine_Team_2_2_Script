using Code.Units.UnitDatas;
using UnityEngine;

namespace Code.Upgrades.Data
{
    [CreateAssetMenu(fileName = "UnitUnlockData", menuName = "SO/Upgrade/UnitUnlock", order = 0)]
    public class UnitUnlockDataSO : UpgradeDataSO
    {
        public UnitDataSO unitData;
    }
}