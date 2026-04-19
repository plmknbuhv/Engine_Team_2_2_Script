using Code.Entities;

namespace Code.Combat
{
    public interface IDamageable
    {
        public void ApplyDamage(int damage, Entity dealer);
    }
}