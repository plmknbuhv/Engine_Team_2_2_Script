using System;
using System.Collections.Generic;
using System.Linq;
using Code.UI.Logics.StateMachine;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Settings.InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Logics.Pause.View
{
    public class PauseView : BaseView, IUIState
    {
        [field: SerializeField] public UIStateType StateType { get; private set; }
        [SerializeField] private Canvas canvas;
        [SerializeField] private GraphicRaycaster graphicRaycaster;
        [SerializeField] private UIElement canvasElement;
        [SerializeField] private PlayerInputSO playerInputSO;

        [Header("Show/Hide")] [SerializeField] private float transitionDuration = 0.5f;
        [SerializeField] private float transitionDelay = 0.05f;
        [SerializeField] private Ease hideEase = Ease.InBack;
        [SerializeField] private Ease showEase = Ease.OutBack;
        [SerializeField] private Transform lHideSpot;
        [SerializeField] private Transform rHideSpot;
        private Dictionary<Transform, Vector3> _positionMap;

        [Header("References")] [SerializeField]
        private Button resumeButton;

        [SerializeField] private Button settingButton;
        [SerializeField] private Button quitButton;
        private List<LayoutElement> _layoutElements;

        private const string ShowKey = "show";

        public Action<bool> OnShowingChanged;
        public Action OnResumeButtonClicked;
        public Action OnSettingButtonClicked;
        public Action OnQuitButtonClicked;

        private void Awake()
        {
            if (_positionMap == null)
            {
                _positionMap = new Dictionary<Transform, Vector3>();
                _positionMap[resumeButton.transform] = resumeButton.transform.position;
                _positionMap[settingButton.transform] = settingButton.transform.position;
                _positionMap[quitButton.transform] = quitButton.transform.position;
            }

            _layoutElements = new List<LayoutElement>()
            {
                resumeButton.GetComponent<LayoutElement>(),
                settingButton.GetComponent<LayoutElement>(),
                quitButton.GetComponent<LayoutElement>()
            };

            resumeButton.onClick.AddListener(HandleResumeButtonClicked);
            settingButton.onClick.AddListener(HandleSettingButtonClicked);
            quitButton.onClick.AddListener(HandleQuitButtonClicked);
        }

        private void OnDestroy()
        {
            resumeButton.onClick.RemoveListener(HandleResumeButtonClicked);
            settingButton.onClick.RemoveListener(HandleSettingButtonClicked);
            quitButton.onClick.RemoveListener(HandleQuitButtonClicked);
        }

        private void HandleResumeButtonClicked() => OnResumeButtonClicked?.Invoke();
        private void HandleSettingButtonClicked() => OnSettingButtonClicked?.Invoke();
        private void HandleQuitButtonClicked() => OnQuitButtonClicked?.Invoke();

        public async UniTask OnEnter()
        {
            playerInputSO.SetInputEnabled(false);
            MoveButtonsToHideSpots();
            await UniTask.NextFrame();

            OnShowingChanged?.Invoke(true);
            canvas.enabled = true;
            await UniTask.WhenAll(canvasElement.AddState(ShowKey), ShowButtons());
            graphicRaycaster.enabled = true;
        }

        public async UniTask OnExit()
        {
            graphicRaycaster.enabled = false;
            await UniTask.WhenAll(canvasElement.RemoveState(ShowKey), HideButtons());
            canvas.enabled = false;
            OnShowingChanged?.Invoke(false);
            _layoutElements.ForEach(le => le.ignoreLayout = false);
            playerInputSO.SetInputEnabled(true);
        }

        private async UniTask HideButtons()
        {
            if (_positionMap == null)
            {
                _positionMap = new Dictionary<Transform, Vector3>();
                _positionMap[resumeButton.transform] = resumeButton.transform.position;
                _positionMap[settingButton.transform] = settingButton.transform.position;
                _positionMap[quitButton.transform] = quitButton.transform.position;
            }

            _layoutElements.ForEach(le => le.ignoreLayout = true);
            var posMap = _positionMap.Keys.ToArray();
            for (int i = 0; i < posMap.Length; i++)
            {
                var trm = posMap[i];
                var pos = trm.position;
                pos.x = i % 2 == 0 ? rHideSpot.position.x : lHideSpot.position.x;
                trm.DOMove(pos, transitionDuration).SetEase(hideEase).SetUpdate(true);
                await UniTask.WaitForSeconds(transitionDelay, true);
            }

            await UniTask.WaitForSeconds(transitionDuration - transitionDelay, true);
        }

        private void MoveButtonsToHideSpots()
        {
            if (_positionMap == null)
                _positionMap = new Dictionary<Transform, Vector3>();
            _positionMap[resumeButton.transform] = resumeButton.transform.position;
            _positionMap[settingButton.transform] = settingButton.transform.position;
            _positionMap[quitButton.transform] = quitButton.transform.position;
            _layoutElements.ForEach(le => le.ignoreLayout = true);
            var posMap = _positionMap.Keys.ToArray();
            for (int i = 0; i < posMap.Length; i++)
            {
                var trm = posMap[i];
                var pos = trm.position;
                pos.x = i % 2 == 0 ? rHideSpot.position.x : lHideSpot.position.x;
                trm.position = pos;
            }
        }

        private async UniTask ShowButtons()
        {
            foreach (var trm in _positionMap.Keys)
            {
                var pos = _positionMap[trm];
                trm.DOMove(pos, transitionDuration).SetEase(showEase).SetUpdate(true);
                await UniTask.WaitForSeconds(transitionDelay, true);
            }

            await UniTask.WaitForSeconds(transitionDuration - transitionDelay, true);
            _layoutElements.ForEach(le => le.ignoreLayout = false);
        }
    }
}