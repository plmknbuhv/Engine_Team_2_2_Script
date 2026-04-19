using Code.Units;

namespace Code.Combat.Projectiles
{
    public interface ICanTarget
    {
        public void SetTarget(FriendlyUnit owner, EnemyUnit targetUnit, int damage);
    }
}