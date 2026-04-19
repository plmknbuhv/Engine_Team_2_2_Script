using Cannons;
using Cannons.StatSystem;
using Code.Units;
using UnityEngine;

namespace EventSystem
{
    public static class CannonEvents
    {
        public static readonly CannonShootEvent CannonShootEvent = new CannonShootEvent();
        public static readonly CannonShootExplosionEvent CannonShootExplosionEvent = new CannonShootExplosionEvent();
        public static readonly CannonCheckCanShootEvent CannonCheckCanShootEvent = new CannonCheckCanShootEvent();
        public static readonly CannonChargingEvent CannonChargingEvent = new CannonChargingEvent();
        public static readonly CannonStatUpgradeEvent CannonStatUpgradeEvent = new CannonStatUpgradeEvent();
        public static readonly CannonChangeHealthEvent CannonChangeHealthEvent = new CannonChangeHealthEvent();
        public static readonly CannonAddHealthEvent CannonAddHealthEvent = new CannonAddHealthEvent();
        public static readonly CannonDeathEvent CannonDeathEvent = new CannonDeathEvent();
    }
    
    public class CannonShootEvent : GameEvent
    {
        public Unit unit;

        public CannonShootEvent Initializer(Unit unit)
        {
            this.unit = unit;
            return this;
        }
    }
    
    public class CannonShootExplosionEvent : GameEvent
    {
        public Vector3 endPos;
        public UnitSizeType sizeType;
        
        public CannonShootExplosionEvent Initializer(Vector3 pos, UnitSizeType size)
        {
            endPos = pos;
            sizeType = size;
            return this;
        }
    }
    
    public class CannonCheckCanShootEvent : GameEvent
    {
        public bool canShoot;
        
        public CannonCheckCanShootEvent Initializer(bool isUsed)
        {
            canShoot = isUsed;
            return this;
        }
    }
    
    public class CannonChargingEvent : GameEvent
    {
        public bool isCharging;
        
        public CannonChargingEvent Initializer(bool isUsed)
        {
            isCharging = isUsed;
            return this;
        }
    }
    
    public class CannonStatUpgradeEvent : GameEvent
    {
        public float addValue;
        public CannonStatType statType;

        public CannonStatUpgradeEvent Initializer(CannonStatType type, float value)
        {
            statType = type;
            addValue = value;
            return this;
        }
    }
    
    public class CannonChangeHealthEvent : GameEvent
    {
        public int health;

        public CannonChangeHealthEvent Initializer(int value)
        {
            health = value;
            return this;
        }
    }
    
    public class CannonAddHealthEvent : GameEvent
    {
        public int addHealth;

        public CannonAddHealthEvent Initializer(int add)
        {
            addHealth = add;
            return this;
        }
    }

    public class CannonDeathEvent : GameEvent
    {
    }
}