using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Combat.Projectiles
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        [field:SerializeField] public PoolItemSO PoolItem { get; set; }

        protected Pool _pool;
        
        public GameObject GameObject => gameObject;
        protected bool IsDead { get; set; }
        
        public virtual void SetUpPool(Pool pool)
        {
            _pool = pool;
        }

        public virtual void ResetItem()
        {
            
        }
    }
}