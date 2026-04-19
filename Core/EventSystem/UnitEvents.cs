using Code.Units;
using Code.Units.Upgrades;

namespace EventSystem
{
    public static class UnitEvents
    {
        public static UnitSpawnEvent UnitSpawnEvent = new UnitSpawnEvent();
        public static UnitDeadEvent UnitDeadEvent = new UnitDeadEvent();
    }

    public class UnitSpawnEvent : GameEvent
    {
        public Unit unit;

        public UnitSpawnEvent Initializer(Unit unit)
        {
            this.unit = unit;
            return this;
        }
    }
    
    public class UnitDeadEvent : GameEvent
    {
        public Unit unit;

        public UnitDeadEvent Initializer(Unit unit)
        {
            this.unit = unit;
            return this;
        }
    }

    public class UnitUpgradeEvent : GameEvent
    {
        public UnitUpgradeSO unitUpgradeSO;
    }
}