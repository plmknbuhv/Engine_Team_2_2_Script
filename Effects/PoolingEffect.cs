using System;
using DG.Tweening;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Effects
{
    public class PoolingEffect : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;
        
        private Pool _myPool;
        [SerializeField] private GameObject effectObject;
        [SerializeField] private bool useDuration;
        [SerializeField] private float duration;
        private IPlayableVFX _playableVFX;

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
            _playableVFX = effectObject.GetComponent<IPlayableVFX>();
            Debug.Assert(_playableVFX != null, $"effect object must have IPlayableVFX component");
        }

        public void ResetItem()
        {
            _playableVFX.StopVFX();
        }
        
        public async void PlayVFX(Vector3 position, Quaternion rotation)
        {
            _playableVFX.PlayVFX(position, rotation);

            if (useDuration)
            {
                await Awaitable.WaitForSecondsAsync(duration);

                StopPoolEffect();
            }
        }

        public void SetDuration(float duration)
        {
            this.duration = duration;
        }

        public async void StopPoolEffect()
        {
            try
            {
                _playableVFX.StopVFX();
            
                await Awaitable.WaitForSecondsAsync(2.5f);
                
                transform.SetParent(null);
                _myPool.Push(this);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private void OnValidate()
        {
            if (effectObject == null) return;
            _playableVFX = effectObject.GetComponent<IPlayableVFX>();
            if (_playableVFX == null)
            {
                effectObject = null;
                Debug.LogError($"effect object must have IPlayableVFX component");
            }
        }
    }
}