using EventSystem;
using UnityEngine;

namespace Code.Upgrades.LevelSystem
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO levelChannel;

        [SerializeField] private int maxLevel = 30;
        [SerializeField] private float requiredExp = 20f;
        [SerializeField] private AnimationCurve nextRequiredExpCurve;

        private float _startRequiredExp;
        
        private float _currentExp;
        public float CurrentExp
        {
            get => _currentExp;
            set
            {
                _currentExp = value;

                var changeExpEvt = LevelEvents.ChangeExpEvent.Initializer(value);
                levelChannel.RaiseEvent(changeExpEvt);
                
                while (_currentExp >= requiredExp)
                {
                    _currentExp -= requiredExp;

                    changeExpEvt = LevelEvents.ChangeExpEvent.Initializer(_currentExp);
                    levelChannel.RaiseEvent(changeExpEvt);
                    
                    float maxMultiplierPercent = Mathf.Clamp01((float)CurrentLevel / (float)maxLevel);
                    if (maxMultiplierPercent < 0) maxMultiplierPercent = 0f;
                    
                    requiredExp += (int)(_startRequiredExp * nextRequiredExpCurve.Evaluate(maxMultiplierPercent));
                    
                    var changeRequiredExpEvt = LevelEvents.ChangeRequiredExpEvent.Initializer(requiredExp);
                    levelChannel.RaiseEvent(changeRequiredExpEvt);
                    
                    CurrentLevel++;
                }
            }
        }
        
        private int _currentLevel;
        public int CurrentLevel
        {
            get => _currentLevel;
            set
            {
                _currentLevel = value;

                var changeEvt = LevelEvents.ChangeLevelEvent.Initializer(value);
                levelChannel.RaiseEvent(changeEvt);
            }
        }

        private void Awake()
        {
            _startRequiredExp = requiredExp;
            levelChannel.AddListener<AddExpEvent>(HandleAddExp);
        }

        private void Start()
        {
            levelChannel.RaiseEvent(LevelEvents.ChangeLevelEvent.Initializer(CurrentLevel));
            levelChannel.RaiseEvent(LevelEvents.ChangeExpEvent.Initializer(CurrentExp));
        }

        private void OnDestroy()
        {
            levelChannel.RemoveListener<AddExpEvent>(HandleAddExp);
        }

        private void HandleAddExp(AddExpEvent evt)
        {
            AddExp(evt.addExp);
        }

        public void AddExp(float exp)
        {
            if (exp < 0) return;
            
            CurrentExp += exp;
        }
    }
}