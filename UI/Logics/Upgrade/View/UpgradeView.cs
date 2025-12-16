using System;
using System.Collections.Generic;
using Code.UI.Logics.StateMachine;
using Code.UI.Visual;
using Code.Upgrades;
using Code.Upgrades.Data;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Logics.Upgrade.View
{
    public class UpgradeView : BaseView, IUIState
    {
        [field: SerializeField] public UIStateType StateType { get; private set; }

        [Header("Canvas Settings")] [SerializeField]
        private Canvas canvas;

        [SerializeField] private GraphicRaycaster graphicRaycaster;

        [Header("Show Settings")] [SerializeField]
        private UIElement canvasElement;

        [SerializeField] private float showDuration = 0.5f;
        [SerializeField] private float showDelay = 0.1f;
        [SerializeField] private Ease showEase = Ease.OutCubic;
        [SerializeField] private Transform cardShowPoint;
        [SerializeField] private UpgradeCardController[] upgradeCards;
        [SerializeField] private Button rerollButton;
        private TextMeshProUGUI _rerollButtonText;
        private UIElement _rerollButtonElement;

        private const string ShowKey = "show";
        private const string InactiveKey = "inactive";
        private string _rerollFormat;
        
        private bool _isRerolling = false;
        private bool _didSetCards = false;

        public Action<int> OnCardSelected;
        public Action OnRerollSelected;

        private void Awake()
        {
            canvas.enabled = false;
            graphicRaycaster.enabled = false;
            for (int i = 0; i < upgradeCards.Length; i++)
            {
                upgradeCards[i].Init(i);
                upgradeCards[i].OnClickEvent += HandleCardClick;
            }

            rerollButton.onClick.AddListener(OnRerollClicked);
            _rerollButtonText = rerollButton.GetComponentInChildren<TextMeshProUGUI>();
            _rerollFormat = _rerollButtonText.text;
            _rerollButtonElement = rerollButton.GetComponent<UIElement>();
        }

        private void OnDestroy()
        {
            for (int i = 0; i < upgradeCards.Length; i++)
                upgradeCards[i].OnClickEvent -= HandleCardClick;

            rerollButton.onClick.RemoveListener(OnRerollClicked);
        }

        private void OnRerollClicked()
        {
            _isRerolling = true;
            OnRerollSelected?.Invoke();
        }

        private void HandleCardClick(int obj) => OnCardSelected?.Invoke(obj);

        public async UniTask OnEnter()
        {
            Time.timeScale = 0f;
            canvas.enabled = true;
            graphicRaycaster.enabled = false;
            SetRerollButtonInteractable(true);
            
            await UniTask.WaitUntil(() => _didSetCards);
            Debug.Log("Completed setting cards in UpgradeView OnEnter");
            
            List<UniTask> tasks = new List<UniTask>();
            
            tasks.Add(canvasElement.AddState(ShowKey, 2));
            for (int i = 0; i < upgradeCards.Length; i++)
                tasks.Add(upgradeCards[i].ShowCard(cardShowPoint.position, showDuration + showDelay, showEase));
            await UniTask.WhenAll(tasks);

            Debug.Log("Completed showing cards in UpgradeView OnEnter");
            
            _didSetCards = false;
            graphicRaycaster.enabled = true;
        }

        public async UniTask OnExit()
        {
            graphicRaycaster.enabled = false;
            List<UniTask> tasks = new List<UniTask>();
            tasks.Add(canvasElement.RemoveState(ShowKey));
            for (int i = 0; i < upgradeCards.Length; i++)
                tasks.Add(upgradeCards[i].HideCard(cardShowPoint.position, showDuration, showEase));

            await UniTask.WhenAll(tasks);
            canvas.enabled = false;
            Time.timeScale = 1f;
        }

        public async UniTask SetCards(UpgradeDataSO[] datas)
        {
            if (_isRerolling)
            {
                await OnReroll(datas);
                return;
            }
            var tasks = new List<UniTask>();
            for (int i = 0; i < upgradeCards.Length; i++)
            {
                var targetData = datas[i];
                if (i < datas.Length)
                    tasks.Add(upgradeCards[i].SetCardInfo(targetData)); 
            }
            await UniTask.WhenAll(tasks);
            _didSetCards = true;
        }
        
        public async UniTask OnReroll(UpgradeDataSO[] datas)
        {
            graphicRaycaster.enabled = false;
            SetRerollButtonInteractable(false);
            List<UniTask> tasks = new List<UniTask>();
            for (int i = 0; i < upgradeCards.Length; i++)
                tasks.Add(upgradeCards[i].HideCard(cardShowPoint.position, showDuration, showEase));
            await UniTask.WhenAll(tasks);
            tasks.Clear();
            for (int i = 0; i < upgradeCards.Length; i++)
            {
                var targetData = datas[i];
                if (i < datas.Length)
                    tasks.Add(upgradeCards[i].SetCardInfo(targetData));
            }
            await UniTask.WhenAll(tasks);
            tasks.Clear();
            for (int i = 0; i < upgradeCards.Length; i++)
                tasks.Add(upgradeCards[i].ShowCard(cardShowPoint.position, showDuration, showEase));
            await UniTask.WhenAll(tasks);
            graphicRaycaster.enabled = true;
            _isRerolling = false;
            
        }
        
        private void SetRerollButtonInteractable(bool interactable)
        {
            rerollButton.interactable = interactable;
            if (interactable)
            {
                _rerollButtonText.text = string.Format(_rerollFormat, 0);
                _rerollButtonElement.RemoveState(InactiveKey).Forget();
            }
            else
            {
                _rerollButtonText.text = string.Format(_rerollFormat, 1);
                _rerollButtonElement.AddState(InactiveKey, 10).Forget();
            }
        }
    }
}