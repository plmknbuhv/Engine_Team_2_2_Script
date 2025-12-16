using GondrLib.ObjectPool.RunTime;

namespace Code.Units.UnitStatusEffects
{
    public interface IStatusEffect
    {
        public void ApplyStatusEffect(StatusEffectType statusEffectType, float duration);
    }
}