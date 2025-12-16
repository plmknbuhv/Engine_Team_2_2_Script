using Code.Units;
using UnityEngine;

namespace Code.Combat.Projectiles
{
    public class Egg : Projectile, ICanTarget
    {
        [SerializeField] private AnimationCurve moveCurve;
        [SerializeField] private float moveTime = 1.8f;
        
        private FriendlyUnit _owner;
        private EnemyUnit _targetUnit;
        private float _timer;
        private int _damage;
        
        public void SetTarget(FriendlyUnit owner, EnemyUnit targetUnit, int damage)
        {
            _owner = owner;
            _targetUnit = targetUnit;
            _damage = damage;
            IsDead = false;
            _timer = 0;
        }

        private void Update()
        {
            if (_targetUnit == null && _targetUnit.IsDead) Dead(); // 타겟 사라지면 걍 자살
            if (IsDead) return;

            if (_timer < moveTime)
            {
                float t = _timer / moveTime;
                Vector3 position = Vector3.Lerp(_owner.transform.position, _targetUnit.transform.position, t);
                position.y += moveCurve.Evaluate(t);
                transform.position = position;
                _timer += Time.deltaTime;
            }
            else // 도착했으면
            {
                _targetUnit.ApplyDamage(_damage, _owner);
                Dead();
            }
        }

        private void Dead()
        {
            IsDead = true;
            _pool.Push(this);
        }
    }
}