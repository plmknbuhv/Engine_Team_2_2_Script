using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.UI.Visual.Style
{
    public interface IStyle
    {
        void Initialize(GameObject tar);
        UniTask AddState(string state, int priority = 0);
        UniTask RemoveState(string state);
        UniTask ClearStates();
    }
    public abstract class BaseStyle<T, TStyle> : MonoBehaviour, IStyle
        where T : Component
        where TStyle : BaseStyleData
    {
        [SerializeField] private string defaultState = "default";
        [SerializeField] protected TStyle[] styleDatas;
        protected Dictionary<string, TStyle> styles { get; } = new Dictionary<string, TStyle>();
        protected Dictionary<string, int> states { get; } = new Dictionary<string, int>();

        protected T target;
        
        private string _currentState = "";
        private List<string> _stateHistory = new List<string>();
        
        protected bool _isInitialized = false;
        
        
        private async void Start()
        {
            try
            {
                await UniTask.WaitForEndOfFrame();
                _isInitialized = true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        public virtual void Initialize(GameObject tar)
        {
            target = tar.GetComponent<T>();

            foreach (var styleData in styleDatas)
            {
                if (!styles.ContainsKey(styleData.State))
                    styles.Add(styleData.State, styleData);
            }

            UpdateCurrentState().Forget();
        }

        public async UniTask AddState(string state, int priority = 0)
        {
            if (!styles.Keys.Contains(state)) return;

            if (!states.ContainsKey(state))
            {
                _stateHistory.Add(state);
                states.Add(state, priority);
            }
            else
                states[state] = priority;

            await UpdateCurrentState();
        }

        public async UniTask RemoveState(string state)
        {
            if (states.ContainsKey(state))
            {
                _stateHistory.Remove(state);
                states.Remove(state);
            }

            await UpdateCurrentState();
        }

        public async UniTask ClearStates()
        {
            states.Clear();
            
            await UpdateCurrentState();
        }

        public async UniTask UpdateCurrentState()
        {
            if (target == null) return;
            
            if (states.Count <= 0)
            {
                await ApplyStyle(styles[defaultState]);
                _currentState = defaultState;
                return;
            }

            var highestPriority = int.MinValue;
            var selectedState = defaultState;

            foreach (var state in states.Where(state => state.Value > highestPriority))
            {
                highestPriority = state.Value;
                selectedState = state.Key;
            }

            // if (selectedState == _currentState) return;
            _currentState = selectedState;

            await ApplyStyle(styles[selectedState]);
        }

        protected abstract UniTask ApplyStyle(TStyle currentState);
    }

    [Serializable]
    public abstract class BaseStyleData
    {
        public string State = "default";
        public Ease Ease = Ease.InOutSine;
        public float Duration = 0.3f;
    }
}