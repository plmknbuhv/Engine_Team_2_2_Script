using Code.Units;
using Code.Units.UnitDatas;

namespace EventSystem
{
    public static class SlotEvents
    {
        public static readonly SlotRemoveEvent SlotRemoveEvent = new SlotRemoveEvent();
        public static readonly SlotAddEvent SlotAddEvent = new SlotAddEvent();
        public static readonly UnitUnlockToSlotEvent UnitUnlockToSlotEvent = new UnitUnlockToSlotEvent();
        public static readonly CanRemoveCountChangeEvent CanRemoveCountChangeEvent = new CanRemoveCountChangeEvent();
    }

    public class SlotRemoveEvent : GameEvent
    {
        public int slotIndex;
        
        public SlotRemoveEvent Initializer(int idx)
        {
            slotIndex = idx;
            return this;
        }
    }
    public class SlotAddEvent : GameEvent
    {
        public UnitDataSO unitData;
        
        public SlotAddEvent Initializer(UnitDataSO data)
        {
            unitData = data;
            return this;
        }
    }
    
    public class UnitUnlockToSlotEvent : GameEvent
    {
        public UnitDataSO unitData;
        
        public UnitUnlockToSlotEvent Initializer(UnitDataSO data)
        {
            unitData = data;
            return this;
        }
    }

    public class CanRemoveCountChangeEvent : GameEvent
    {
        public int canRemoveCount;
        public int usedUnitCount;
        
        public CanRemoveCountChangeEvent Initializer(int removeCount, int unitCount)
        {
            canRemoveCount = removeCount;
            usedUnitCount = unitCount;
            return this;
        }
    }
}