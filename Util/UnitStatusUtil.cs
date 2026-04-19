using Code.Units;
using Code.Units.UnitStatusEffects;
using UnityEngine;

namespace Code.Util
{
    public static class UnitStatusUtil
    {
        public static bool IsTargetStatus(Unit unit, StatusEffectType statusType)
        {
            bool isTargetStatus = false;

            if (TryGetUnitStatus(unit, out UnitStatusEffect statusEffect))
            {
                isTargetStatus = statusEffect.CurrentStatusEffectData.statusEffectType == statusType;
                Debug.Log(statusEffect.CurrentStatusEffectData.statusEffectType);
            }

            return isTargetStatus;
        }

        private static bool TryGetUnitStatus(Unit unit, out UnitStatusEffect statusEffect)
        {
            statusEffect = unit.GetCompo<UnitStatusEffect>();

            return statusEffect != null && statusEffect.CurrentStatusEffectData != null;
        }
    }
}