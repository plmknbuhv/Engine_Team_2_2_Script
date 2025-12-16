using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Logics.Setting
{
    public class DisplaySettingController : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Toggle windowedToggle;
        [SerializeField] private Button applyButton;
        [SerializeField] private Button cancelButton;
        
        public Action<DisplaySettingData> OnApply;
        public Action OnRevert;

        private void Awake()
        {
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggleChanged);
            windowedToggle.onValueChanged.AddListener(OnWindowedToggleChanged);
            applyButton.onClick.AddListener(OnApplyButtonClicked);
            cancelButton.onClick.AddListener(OnCancelButtonClicked);
        }
        private void OnDestroy()
        {
            fullscreenToggle.onValueChanged.RemoveListener(OnFullscreenToggleChanged);
            windowedToggle.onValueChanged.RemoveListener(OnWindowedToggleChanged);
            applyButton.onClick.RemoveListener(OnApplyButtonClicked);
            cancelButton.onClick.RemoveListener(OnCancelButtonClicked);
        }
        private void OnFullscreenToggleChanged(bool isOn)
        {
            windowedToggle.isOn = !isOn;
        }
        private void OnWindowedToggleChanged(bool isOn)
        {
            fullscreenToggle.isOn = !isOn;
        }
        private void OnApplyButtonClicked()
        {
            ResolutionType selectedResolution = (ResolutionType)resolutionDropdown.value;
            bool isFullscreen = fullscreenToggle.isOn;
            
            DisplaySettingData settingData = new DisplaySettingData
            {
                resolution = selectedResolution,
                isFullscreen = isFullscreen
            };
            
            OnApply?.Invoke(settingData);
        }
        private void OnCancelButtonClicked() => OnRevert?.Invoke();
        
        public void SetDisplaySetting(DisplaySettingData settingData)
        {
            resolutionDropdown.value = (int)settingData.resolution;
            fullscreenToggle.isOn = settingData.isFullscreen;
            windowedToggle.isOn = !settingData.isFullscreen;
        }
    }

    public struct DisplaySettingData
    {
        public ResolutionType resolution;
        public bool isFullscreen;
    }
    public enum ResolutionType
    {
        R_1920x1080,
        R_1920x1200,
        R_2560x1080,
        End
    }
}