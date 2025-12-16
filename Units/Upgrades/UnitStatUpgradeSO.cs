using Code.Units.UnitStat;
using Code.Upgrades.Data;
using UnityEngine;

namespace Code.Units.Upgrades
{
    [CreateAssetMenu(fileName = "Unit stat upgrade data", menuName = "SO/Unit/Upgrade/StatUpgrade", order = 0)]
    public class UnitStatUpgradeSO : UpgradeDataSO
    {
        public UnitTeamType targetTeam;
        public UnitStatType statType;
        public float addValue; //계수 추가치
    }
}