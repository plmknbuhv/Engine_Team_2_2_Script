using System;
using System.Collections.Generic;
using Code.Units;
using EventSystem;
using GondrLib.Dependencies;
using Level;
using UnityEngine;
using UnityEngine.Events;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;
using Random = UnityEngine.Random;

namespace Enemies
{
    [Provide]
    public class EnemyWaveSpawner : MonoBehaviour
    {
        public UnityEvent onSpawnWave;
        public UnityEvent onBossSpawnWave;
        
        [SerializeField] private GameEventChannelSO enemyChannel;
        [SerializeField] private GameEventChannelSO waveChannel;
        
        [Inject] private PoolManagerMono _poolManager;
        [Inject] private EnemyManager _enemyManager;
        [Inject] private LevelSpline _levelSpline;
        
        [Tooltip("한 웨이브에서 나올 적 종류와 갯수를 설정하는 리스트")]
        [SerializeField] private List<WaveDataSo> waveGroup;
        [Space]
        [SerializeField] private float splineOffsetX;

        [SerializeField] private bool loop;

        private float _waveDelay;
        private float _currentWaveDelay;
        
        private int _currentBossWave;
        private int _currentWave;

        private bool _isBossWave;
        private bool _isAllWaveClear;

        private void Awake()
        {
            enemyChannel.AddListener<BossDeathEvent>(BossWaveComplete);
        }
        

        private void OnDestroy()
        {
            enemyChannel.RemoveListener<BossDeathEvent>(BossWaveComplete);
        }

        private void Update()
        {
                
            if (waveGroup.Count <= _currentWave)
            {
                if (_enemyManager.CurrentEnemies.Count == 0 && !_isAllWaveClear && !_isBossWave)
                {
                    _isAllWaveClear = true;
                    Debug.Log($"모든 웨이브 클리어");
                    waveChannel.RaiseEvent(WaveEvents.AllWaveCompleteEvent.Initializer());
                }

                return;
            }
            
            _currentWaveDelay += Time.deltaTime;
            
            if(_enemyManager.CurrentEnemies.Count > 0
               || _waveDelay > _currentWaveDelay) return;
            
            SpawnWave();
        }

        private void BossWaveComplete(BossDeathEvent evt)
        {
            waveChannel.RaiseEvent(WaveEvents.BossWaveCompleteEvents.Initializer(_currentBossWave));
            _isBossWave = false;
        }

        [ContextMenu("Reset Wave")]
        private void ResetWave()
        {
            _currentWave = 0;
            _currentBossWave = 0;
            
            _waveDelay = 0;
            _currentWaveDelay = 0;
            
            _isBossWave = false;
            _isAllWaveClear = false;
            Debug.Log($"웨이브 다시 시작");
        }
        
        private async void SpawnWave()
        {
            int currentWaveIdx = _currentWave;
            _currentWave++;
            
            _waveDelay = 2f;
            _currentWaveDelay = 0;
            
            bool isBossWave = waveGroup[currentWaveIdx].isBossWave;
            
            _isBossWave = isBossWave;
            
            waveChannel.RaiseEvent(WaveEvents.StartWaveEvent
                .Initializer(_currentWave, waveGroup[currentWaveIdx].allEnemyCount));

            if (isBossWave)
            {
                waveChannel.RaiseEvent(WaveEvents.StartBossWaveEvent.Initializer());
                onBossSpawnWave.Invoke();
                _currentBossWave++;
            }
            else
            {
                onSpawnWave?.Invoke();
            }
            
            Debug.Log($"웨이브 : {_currentWave}");
            
            foreach (var enemyData in waveGroup[currentWaveIdx].waveEnemyGroup)
            {
                try
                {
                    for(int i = 0; i < enemyData.count; i++)
                    {
                        float randDelay = Random.Range(enemyData.minSpawnDelay,
                            enemyData.maxSpawnDelay);
                            
                        await Awaitable.WaitForSecondsAsync(randDelay, destroyCancellationToken);
                            
                        Unit enemy = _poolManager.Pop<Unit>(enemyData.enemyDataSo.poolItem);
                        EnemyUnit enemyCompo = enemy as EnemyUnit;

                        if (enemy  == null || enemyCompo == null)
                        {
                            Debug.Assert(false, "EnemyDataSo가 없습니다.");
                        }
                        
                        FollowLine followLine = enemyCompo.GetCompo<FollowLine>();

                        float offsetX = 0;
                        
                        if (enemyCompo.IsBoss == false)
                        {
                            offsetX = Random.Range(-splineOffsetX, splineOffsetX);
                        }
                            
                        followLine.SetUpFollowLine(_levelSpline.Splines[0], offsetX);
                        
                        enemyChannel.RaiseEvent(EnemyEvents.EnemyListChangedEvent.Initializer(enemyCompo, EnemyRegistry.Add));
                        enemyChannel.RaiseEvent(EnemyEvents.EnemySpawnEvent.Initializer(enemyCompo));
                    }
                }
                catch (OperationCanceledException ex)
                {
                    Debug.Log("웨이브 취소");
                    Debug.Log(ex);
                }
            }
        }
    }
}