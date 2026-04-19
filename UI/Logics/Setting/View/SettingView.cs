using System;
using System.Collections.Generic;
using Code.UI.Logics.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Logics.Setting.View
{
    public abstract class SettingView : BaseView, IUIState
    {
        [field: SerializeField]public UIStateType StateType { get; private set; } = UIStateType.Setting;
        [Header("Setting Controllers")] [SerializeField]
        private DisplaySettingController displaySettingController;

        [SerializeField] private SoundSliderController[] soundSliderController;
        [SerializeField] private Button saveButton;
        [SerializeField] private DisplayConfirmationController displayConfirmationController;

        private Dictionary<SoundSliderType, SoundSliderController> _soundSliderControllers;

        public Action<bool> OnShowingChanged;
        public Action<DisplaySettingData> OnApplyButtonClicked;
        public Action OnDisplaySettingReverted;
        public Action OnDisplaySettingApplied;
        public Action<SoundSliderType, float> OnSoundSettingsChanged;
        public Action OnSave;

        protected virtual void Awake()
        {
            displaySettingController.OnApply += HandleDisplaySettingApplied;
            displaySettingController.OnRevert += HandleDisplayApplyReverted;

            displayConfirmationController.OnConfirm += HandleDisplayApplyConfirmation;
            displayConfirmationController.OnRevert += HandleDisplayApplyReverted;

            if (_soundSliderControllers == null)
            {
                _soundSliderControllers = new Dictionary<SoundSliderType, SoundSliderController>();
                foreach (var soundSlider in soundSliderController)
                {
                    soundSlider.OnValueChanged += HandleSoundSliderChanged;
                    _soundSliderControllers[soundSlider.SliderType] = soundSlider;
                }
            }

            saveButton.onClick.AddListener(HandleSaveButtonClicked);
        }

        private void OnDestroy()
        {
            displaySettingController.OnApply -= HandleDisplaySettingApplied;
            displaySettingController.OnRevert -= HandleDisplayApplyReverted;

            displayConfirmationController.OnConfirm -= HandleDisplayApplyConfirmation;
            displayConfirmationController.OnRevert -= HandleDisplayApplyReverted;

            foreach (var soundSlider in soundSliderController)
            {
                soundSlider.OnValueChanged -= HandleSoundSliderChanged;
            }

            saveButton.onClick.RemoveListener(HandleSaveButtonClicked);
        }

        private void HandleDisplaySettingApplied(DisplaySettingData obj) => OnApplyButtonClicked?.Invoke(obj);
        private void HandleDisplayApplyConfirmation() => OnDisplaySettingApplied?.Invoke();
        private void HandleDisplayApplyReverted() => OnDisplaySettingReverted?.Invoke();

        private void HandleSoundSliderChanged(SoundSliderType arg1, float arg2) =>
            OnSoundSettingsChanged?.Invoke(arg1, arg2);

        private void HandleSaveButtonClicked() => OnSave?.Invoke();

        public void SetUpSlider(SoundSliderType sliderType, float value)
        {
            if (_soundSliderControllers == null)
            {
                _soundSliderControllers = new Dictionary<SoundSliderType, SoundSliderController>();
                foreach (var soundSlider in soundSliderController)
                {
                    soundSlider.OnValueChanged += HandleSoundSliderChanged;
                    _soundSliderControllers[soundSlider.SliderType] = soundSlider;
                }
            }

            if (_soundSliderControllers.TryGetValue(sliderType, out var controller))
            {
                controller.InitializeValue(value);
            }
        }

        public void SetDisplaySetting(DisplaySettingData settingData)
        {
            displaySettingController.SetDisplaySetting(settingData);
        }

        public void ShowDisplayApplyConfirmation()
        {
            displayConfirmationController.Show();
        }

        public virtual async UniTask OnEnter()
        {
            OnShowingChanged?.Invoke(true);
            await UniTask.CompletedTask;
        }

        public virtual async UniTask OnExit()
        {
            OnShowingChanged?.Invoke(false);
            await UniTask.CompletedTask;
        }
    }
}