using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EventSystem;
using UnityEngine;

namespace Code.UI.Logics.StateMachine
{
    public class UIStateMachine : MonoBehaviour
    {
        [SerializeField] private UIStateType initialStateType = UIStateType.None;
        [SerializeField] private GameEventChannelSO uiEventChannel;

        private Dictionary<UIStateType, IUIState> _stateDictionary = new Dictionary<UIStateType, IUIState>();
        private IUIState _previousState;
        private IUIState _currentState;

        private bool _isChangingState = false;

        private void Start()
        {
            uiEventChannel.AddListener<UIStateChangeEvent>(HandleUIStateChangeEvent);

            var states = GetComponentsInChildren<IUIState>(true);
            foreach (var state in states)
            {
                _stateDictionary[state.StateType] = state;
                if (state.StateType == initialStateType)
                {
                    _currentState = state;
                    state.OnEnter().Forget();
                }
                else
                {
                    state.OnExit().Forget();
                }
            }
        }

        private void OnDestroy()
        {
            uiEventChannel.RemoveListener<UIStateChangeEvent>(HandleUIStateChangeEvent);
        }

        private async void HandleUIStateChangeEvent(UIStateChangeEvent obj)
        {
            if (_isChangingState) return;

            _isChangingState = true;
            var newStateType = obj.NewStateType;

            if (obj.NewStateType == UIStateType.None)
                newStateType = _previousState.StateType;

            if (_currentState.StateType is not (UIStateType.Pause or UIStateType.Setting))
            {
                _previousState = _currentState;
            }

            // List<UniTask> tasks = new List<UniTask>();
            if (_currentState != null && _currentState.StateType != newStateType)
            {
                // tasks.Add(_currentState.OnExit());
                await _currentState.OnExit();
            }

            if (_stateDictionary.TryGetValue(newStateType, out var newState) && newState != _currentState)
            {
                _currentState = newState;
                // tasks.Add(newState.OnEnter());
                await newState.OnEnter();
            }

            // await UniTask.WhenAll(tasks);
            _isChangingState = false;
        }
    }
}