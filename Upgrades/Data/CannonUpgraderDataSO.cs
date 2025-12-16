using Cannons.StatSystem;
using UnityEngine;

namespace Code.Upgrades.Data
{
    [CreateAssetMenu(fileName = "CannonStatUpgraderData", menuName = "SO/Upgrade/CannonStat", order = 0)]
    public class CannonUpgraderDataSO : UpgradeDataSO
    {
        public CannonStatType statType;
    }
}