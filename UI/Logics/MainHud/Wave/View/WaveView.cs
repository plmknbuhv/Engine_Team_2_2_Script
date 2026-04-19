using System;
using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Logics.MainHud.Wave.View
{
    public class WaveView : BaseView
    {
        [SerializeField] private Slider waveSlider;
        [SerializeField] private TextMeshProUGUI waveCountText;
        [Header("Boss Warning")]
        [SerializeField] private UIElement bossWarningElement;
        [SerializeField] private float bossWarningShowDuration = 2f;

        private string _waveCountFormat;
        private const string HideKey = "hide";

        private void Awake()
        {
            _waveCountFormat = waveCountText.text;
        }

        private void Start()
        {
            bossWarningElement.AddState(HideKey).Forget();
        }
        public void SetWaveCount(int count)
        {
            waveCountText.text = string.Format(_waveCountFormat, count);
        }
        
        public async void ShowBossWarning()
        {
            try
            {
                await bossWarningElement.RemoveState(HideKey);
                await UniTask.WaitForSeconds(bossWarningShowDuration);
                bossWarningElement.AddState(HideKey).Forget();
            }
            catch (Exception e)
            {
                Debug.LogError($"Error showing boss warning: {e.Message}");
            }
        }

        public void SetWaveValue(int maxValue, int currentValue)
        {
            if (maxValue <= 0)
            {
                waveSlider.value = 0;
                return;
            }
            waveSlider.value = 1 - (float)currentValue/maxValue;
        }
    }
}