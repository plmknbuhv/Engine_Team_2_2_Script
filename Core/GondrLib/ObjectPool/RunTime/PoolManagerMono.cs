using GondrLib.Dependencies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Work.Common.Core.GondrLib.ObjectPool.RunTime
{
    [DefaultExecutionOrder(-10),Provide]
    public class PoolManagerMono : MonoBehaviour,IDependencyProvider
    {
        [SerializeField] private PoolManagerSO poolManager;
        private static PoolManagerMono _instance;
        public static PoolManagerMono Instance => _instance;

        private void Awake()
        {
            if (_instance != null)
                Debug.LogWarning("Poolmanager is already existing");

            else
                _instance = this;
            poolManager.InitializePool(transform);
        }

        public T Pop<T>(PoolItemSO item) where T : IPoolable
        {
            return (T)poolManager.Pop(item);
        }
        
        public void Push(IPoolable item)
        {
            poolManager.Push(item);
        }
    }
}