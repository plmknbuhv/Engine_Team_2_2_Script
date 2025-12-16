using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Code.UI.Logics.StateMachine;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.UI.Logics.Title.View
{
    public class TitleView : BaseView, IUIState

    {
        [Header("UI State")]
        [field: SerializeField]
        public UIStateType StateType { get; private set; }

        [SerializeField] private UIElement canvasElement;
        [SerializeField] private Canvas canvas;
        [SerializeField] private GraphicRaycaster graphicRaycaster;

        [Header("EffectSettings")] [SerializeField]
        private Transform hidePosition;

        [SerializeField] private Ease hideEase = Ease.InBack;
        [SerializeField] private Ease showEase = Ease.OutBack;
        [SerializeField] private float duration = .1f;
        [SerializeField] private float delay = .1f;
        [SerializeField] private Transform[] elements;

        [Header("Buttons")] [SerializeField] private Button startButton;
        [SerializeField] private Button settingButton;
        [SerializeField] private Button exitButton;

        private Dictionary<Transform, Vector3> _positionMap;
        
        public bool IsTransitionEnded { get; set; } = false;
        private bool _isHided = false;

        public Action<TitleButtonType> OnButtonClicked;

        #region Awake & OnDestroy

        private void Awake()
        {
            _positionMap = new Dictionary<Transform, Vector3>();
            startButton.onClick.AddListener(() => OnButtonClicked?.Invoke(TitleButtonType.Start));
            settingButton.onClick.AddListener(() => OnButtonClicked?.Invoke(TitleButtonType.Setting));
            exitButton.onClick.AddListener(() => OnButtonClicked?.Invoke(TitleButtonType.Exit));
        }

        private void OnDestroy()
        {
            startButton.onClick.RemoveAllListeners();
            settingButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
        }

        #endregion

        public async UniTask OnEnter()
        {
            canvas.enabled = true;
            await ShowElements();
            graphicRaycaster.enabled = true;
        }

        public async UniTask OnExit()
        {
            graphicRaycaster.enabled = false;
            await HideElements();
            canvas.enabled = false;
        }

        public async void HandleStartButtonClicked(int sceneIndex)
        {
            try
            {
                HideElements().Forget();
                await UniTask.WaitUntil(() =>_isHided && IsTransitionEnded); //나중에 바꾸기 
                await UniTask.NextFrame();
                Debug.Log("Loading Main Scene...");
                await SceneManager.LoadSceneAsync(sceneIndex);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading Main Scene: {e.Message}");
            }
        }

        private async UniTask HideElements()
        {
            graphicRaycaster.enabled = false;

            Vector3 pos;

            foreach (var element in elements)
                _positionMap[element] = element.position;
            _positionMap[startButton.transform] = startButton.transform.position;
            _positionMap[settingButton.transform] = settingButton.transform.position;
            _positionMap[exitButton.transform] = exitButton.transform.position;
            
            foreach (var trm in _positionMap.Keys)
            {
                pos = _positionMap[trm];
                pos.x = hidePosition.position.x;
                trm.DOMove(pos, duration).SetEase(hideEase).SetUpdate(true);
                await UniTask.WaitForSeconds(delay, true);
            }
            await UniTask.WaitForSeconds(duration - delay, true);
            
            _isHided = true;
        }
        
        private async UniTask ShowElements()
        {
            Vector3 pos;

            foreach (var trm in _positionMap.Keys)
            {
                pos = _positionMap[trm];
                trm.DOMove(pos, duration).SetEase(showEase).SetUpdate(true);
                await UniTask.WaitForSeconds(delay, true);
            }
            await UniTask.WaitForSeconds(duration - delay, true);
        }
    }
}