using Code.UI.Logics.MainHud.Wave.Model;
using Code.UI.Logics.MainHud.Wave.View;
using Enemies;
using EventSystem;
using UnityEngine;

namespace Code.UI.Logics.MainHud.Wave.Presenter
{
    public class WavePresenter : BasePresenter<WaveModel, WaveView>
    {
        private GameEventChannelSO _waveChannel;
        private GameEventChannelSO _enemyChannel;
        private EnemyManager _enemyManager;
        public WavePresenter(WaveModel model, WaveView view, GameEventChannelSO waveChannel, GameEventChannelSO enemyChannel, EnemyManager enemyManager) : base(model, view)
        {
            _waveChannel = waveChannel;
            _enemyManager = enemyManager;
            _enemyChannel = enemyChannel;
            
            _waveChannel.AddListener<StartWaveEvent>(OnWaveStartEvent);
            _waveChannel.AddListener<StartBossWaveEvent>(OnBossWaveEvent);
            
            _enemyChannel.AddListener<EnemyListChangedEvent>(HandleEnemyListChangedEvent);
        }

        public override void Dispose()
        {
            base.Dispose();
            _waveChannel.RemoveListener<StartWaveEvent>(OnWaveStartEvent);
            _waveChannel.RemoveListener<StartBossWaveEvent>(OnBossWaveEvent);
            _enemyChannel.RemoveListener<EnemyListChangedEvent>(HandleEnemyListChangedEvent);
        }

        private void HandleEnemyListChangedEvent(EnemyListChangedEvent obj)
        {
            if (obj.enemyRegistry == EnemyRegistry.Remove)
                Model.EnemyCount --;
        }

        private void OnBossWaveEvent(StartBossWaveEvent obj)
        {
            View.ShowBossWarning();
        }

        private void OnWaveStartEvent(StartWaveEvent obj)
        {
            Model.CurrentWave = obj.waveCount;
            Model.EnemyCount = obj.allEnemyCount;
            Model.MaxEnemyCount = obj.allEnemyCount;
        }

        protected override void OnModelChanged()
        {
            View.SetWaveCount(Model.CurrentWave);
            View.SetWaveValue(Model.MaxEnemyCount, Model.EnemyCount);
        }
    }
}