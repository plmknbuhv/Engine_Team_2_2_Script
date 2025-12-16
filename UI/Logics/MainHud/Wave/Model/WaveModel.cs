using UnityEngine;

namespace Code.UI.Logics.MainHud.Wave.Model
{
    public class WaveModel : BaseModel
    {
        private int _currentWave;
        public int CurrentWave
        {
            get => _currentWave;
            set
            {
                if (_currentWave == value) return;
                _currentWave = value;
                NotifyChanged();
            }
        }

        private int _maxMaxEnemyCount;
        public int MaxEnemyCount
        {
            get => _maxMaxEnemyCount;
            set
            {
                _maxMaxEnemyCount = value;
                NotifyChanged();
            }
        }
        
        private int _enemyCount;
        public int EnemyCount
        {
            get => _enemyCount;
            set
            {
                _enemyCount = value;
                NotifyChanged();
            }
        }
    }
}