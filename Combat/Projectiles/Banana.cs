using Code.Units;
using Code.Units.UnitAnimals.Monkeys;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Combat.Projectiles
{
    public class Banana : Projectile, ICanTarget
    {
        [SerializeField] private AnimationCurve moveCurve;
        [SerializeField] private float moveTime = 1.8f;
        [SerializeField] private float throwPercent = 0.3f;
        [SerializeField] private float throwMinTime, throwMaxTime;
        [SerializeField] private PoolItemSO bananaPeel;
        
        private FriendlyUnit _owner;
        private EnemyUnit _targetUnit;
        private float _timer;
        private float _throwTime;  
        private int _damage;
        private bool _isThrownBanana;
        
        public void SetTarget(FriendlyUnit owner, EnemyUnit targetUnit, int damage)
        {
            transform.rotation = Quaternion.Euler(
                Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180));
            _owner = owner;
            _targetUnit = targetUnit;
            _damage = damage;
            IsDead = false;
            _isThrownBanana = false;
            _throwTime = Random.Range(throwMinTime, throwMaxTime);
            _timer = 0; 
        }
        
        private void Update()
        {
            if (_targetUnit == null || _targetUnit.IsDead) Dead(); // 타겟 사라지면 걍 자살
            if (IsDead) return;

            if (_timer < moveTime)
            {
                float t = _timer / moveTime;
                
                Vector3 position = Vector3.Lerp(_owner.transform.position, _targetUnit.transform.position, t);
                position.y += moveCurve.Evaluate(t);
                transform.position = position;
                _timer += Time.deltaTime;
                
                if (_isThrownBanana == false && t >= _throwTime) // 바나나 타임이면 바나나 던질게
                {
                    _isThrownBanana = true;
                    if (Random.value >= throwPercent)
                        return;
                    
                    BananaPeel banana = PoolManagerMono.Instance.Pop<BananaPeel>(bananaPeel);
                    banana.transform.position = transform.position;
                    banana.Fall();
                }
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
