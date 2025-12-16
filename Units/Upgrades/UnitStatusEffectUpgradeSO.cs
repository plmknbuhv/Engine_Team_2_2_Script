using Code.Units.UnitStatusEffects;
using Code.Upgrades.Data;
using UnityEngine;

namespace Code.Units.Upgrades
{
    [CreateAssetMenu(fileName = "UnitStatusUpgradeData", menuName = "SO/Unit/Upgrade/StatusEffectUpgrade", order = 0)]
    public class UnitStatusEffectUpgradeSO : UpgradeDataSO
    {
        public UnitTeamType targetTeam;
        public StatusEffectType effectType;
        public float duration; //초 단위
    }
}