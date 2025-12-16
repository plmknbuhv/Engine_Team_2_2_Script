using System;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Logics.Setting
{
    public class DisplayConfirmationController : MonoBehaviour
    {
        [SerializeField] private float countdownTime = 10f;
        [SerializeField] private Canvas confirmationCanvas;
        [SerializeField] private GraphicRaycaster graphicRaycaster;
        [SerializeField] private UIElement confirmElement;

        [Header("References")] [SerializeField]
        private TextMeshProUGUI countdownText;

        [SerializeField] private Button confirmButton;
        [SerializeField] private Button revertButton;
        public Action OnConfirm;
        public Action OnRevert;

        private string _timerFormat;

        private const string ConfirmKey = "confirm";

        private float _countdownTimer = 0;

        private void Awake()
        {
            confirmButton.onClick.AddListener(HandleConfirmClicked);
            revertButton.onClick.AddListener(HandleRevertClicked);

            _timerFormat = countdownText.text;
        }

        private void OnDestroy()
        {
            confirmButton.onClick.RemoveListener(HandleConfirmClicked);
            revertButton.onClick.RemoveListener(HandleRevertClicked);
        }

        private void HandleConfirmClicked()
        {
            OnConfirm?.Invoke();
            Hide();
        }

        private void HandleRevertClicked()
        {
            RevertSettings();
        }


        public async void Show()
        {
            confirmationCanvas.enabled = true;
            _countdownTimer = countdownTime;
            await confirmElement.AddState(ConfirmKey, 5);
            graphicRaycaster.enabled = true;
            Timer();
        }

        private async void Timer()
        {
            while (_countdownTimer > 0)
            {
                countdownText.text = string.Format(_timerFormat, Mathf.CeilToInt(_countdownTimer));
                await UniTask.WaitForSeconds(1, true);
                _countdownTimer -= 1f;
            }

            RevertSettings();
        }

        private void RevertSettings()
        {
            OnRevert?.Invoke();
            _countdownTimer = 0;
            Hide();
        }


        private async void Hide()
        {
            if (graphicRaycaster)
                graphicRaycaster.enabled = false;
            await confirmElement.RemoveState(ConfirmKey);
            confirmationCanvas.enabled = false;
        }
    }
}