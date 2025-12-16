using Code.Units.UnitStatusEffects;
using DG.Tweening;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Units.UnitAnimals.Monkeys
{
    public class BananaPeel : MonoBehaviour, IPoolable
    {
        [field:SerializeField] public PoolItemSO PoolItem { get; private set; }
        [SerializeField] private LayerMask whatIsGround;

        private bool _isCanUse;
        private Pool _pool;
        
        public GameObject GameObject => gameObject;
        
        public void SetUpPool(Pool pool)
        {
            _pool = pool;
            _isCanUse = true;
        }
        
        public void Fall()
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, whatIsGround)) // 바닥이 있으면
            {
                transform.DOMove(hitInfo.point, 0.6f).OnComplete(() => _isCanUse = true);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (_isCanUse == false) return;
            
            EnemyUnit enemy = other.GetComponent<EnemyUnit>();
            if (enemy)
            {
                UnitStatusEffect statusEffect = enemy.GetCompo<UnitStatusEffect>();
                statusEffect.ApplyStatusEffect(StatusEffectType.STUN, 1);

                _isCanUse = false;
                _pool.Push(this);
            }
        }

        public void ResetItem()
        {
            _isCanUse = false;
        }
    }
}
