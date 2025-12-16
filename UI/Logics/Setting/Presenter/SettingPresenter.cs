using System;
using System.Collections.Generic;
using System.IO;
using Ami.BroAudio;
using Code.UI.Logics.Setting.Model;
using Code.UI.Logics.Setting.View;
using Code.UI.Logics.StateMachine;
using EventSystem;
using Settings.InputSystem;
using UnityEngine;

namespace Code.UI.Logics.Setting.Presenter
{
    public class SettingPresenter : BasePresenter<SettingModel, SettingView>
    {
        private GameEventChannelSO _uiChannel;
        private UIStateType _prevState;
        private DisplaySettingData _tempDisplaySetting;
        private UIInputSO _uiInput;

        public SettingPresenter(SettingModel model, SettingView view, GameEventChannelSO uiChannel, UIStateType prev,
            UIInputSO uiInput) :
            base(model, view)
        {
            InitializeSettings();

            _uiChannel = uiChannel;
            _prevState = prev;
            _uiInput = uiInput;

            view.OnShowingChanged += HandleShowingChanged;
            view.OnApplyButtonClicked += HandleApplyButtonClicked;
            view.OnDisplaySettingReverted += HandleDisplaySettingReverted;
            view.OnDisplaySettingApplied += HandleDisplaySettingApplied;
            view.OnSoundSettingsChanged += HandleSoundSettingsChanged;
            view.OnSave += HandleSave;
        }

        public override void Dispose()
        {
            base.Dispose();
            View.OnShowingChanged -= HandleShowingChanged;
            View.OnApplyButtonClicked -= HandleApplyButtonClicked;
            View.OnDisplaySettingReverted -= HandleDisplaySettingReverted;
            View.OnDisplaySettingApplied -= HandleDisplaySettingApplied;
            View.OnSoundSettingsChanged -= HandleSoundSettingsChanged;
            View.OnSave -= HandleSave;
        }

        private void HandleShowingChanged(bool obj)
        {
            if (obj)
                _uiInput.OnCancelPressed += HandleSave;
            else
                _uiInput.OnCancelPressed -= HandleSave;
        }

        private void InitializeSettings()
        {
            InitializeDisplaySettings();
            InitializeSoundSettings();
        }

        private void InitializeDisplaySettings()
        {
            if (File.Exists(Model.DisplaySettingFilePath))
            {
                var json = File.ReadAllText(Model.DisplaySettingFilePath);
                Model.CurrentDisplaySetting = JsonUtility.FromJson<DisplaySettingData>(json);
            }
            else
            {
                Model.CurrentDisplaySetting = new DisplaySettingData
                {
                    resolution =  ResolutionType.R_1920x1080,
                    isFullscreen = Screen.fullScreen
                };
            }

            ChangeResolution(Model.CurrentDisplaySetting);
            View.SetDisplaySetting(Model.CurrentDisplaySetting);
        }

        private void InitializeSoundSettings()
        {
            if (File.Exists(Model.SoundSettingFilePath))
            {
                var json = File.ReadAllText(Model.SoundSettingFilePath);
                Model.CurrentSoundSetting = JsonUtility.FromJson<SoundSettingData>(json);

                View.SetUpSlider(SoundSliderType.MASTER, Model.CurrentSoundSetting.masterVolume);
                View.SetUpSlider(SoundSliderType.BGM, Model.CurrentSoundSetting.musicVolume);
                View.SetUpSlider(SoundSliderType.SFX, Model.CurrentSoundSetting.sfxVolume);
                View.SetUpSlider(SoundSliderType.UI, Model.CurrentSoundSetting.uiVolume);
                
                BroAudio.SetVolume(BroAudioType.All, Model.CurrentSoundSetting.masterVolume);
                BroAudio.SetVolume(BroAudioType.Music, Model.CurrentSoundSetting.musicVolume);
                BroAudio.SetVolume(BroAudioType.SFX, Model.CurrentSoundSetting.sfxVolume);
                BroAudio.SetVolume(BroAudioType.UI, Model.CurrentSoundSetting.uiVolume);
            }
            else
            {
                Model.CurrentSoundSetting = new SoundSettingData
                {
                    masterVolume = 1f,
                    musicVolume = 1f,
                    sfxVolume = 1f,
                    uiVolume = 1f
                };
                
                BroAudio.SetVolume(BroAudioType.All, 1f);
                BroAudio.SetVolume(BroAudioType.Music, 1f);
                BroAudio.SetVolume(BroAudioType.SFX, 1f);
                BroAudio.SetVolume(BroAudioType.UI, 1f);

                foreach (SoundSliderType sliderType in Enum.GetValues(typeof(SoundSliderType)))
                {
                    View.SetUpSlider(sliderType, 1f);
                }
            }
        }

        private void HandleApplyButtonClicked(DisplaySettingData obj)
        {
            _tempDisplaySetting = obj;
            ChangeResolution(_tempDisplaySetting);
            View.ShowDisplayApplyConfirmation();
        }

        private void HandleDisplaySettingReverted()
        {
            ChangeResolution(Model.CurrentDisplaySetting);
            View.SetDisplaySetting(Model.CurrentDisplaySetting);
        }

        private void HandleDisplaySettingApplied() => Model.CurrentDisplaySetting = _tempDisplaySetting;

        private void HandleSoundSettingsChanged(SoundSliderType key, float value)
        {
            switch (key)
            {
                case SoundSliderType.MASTER:
                    Model.CurrentSoundSetting.masterVolume = value;
                    BroAudio.SetVolume(BroAudioType.All, value);
                    break;
                case SoundSliderType.BGM:
                    Model.CurrentSoundSetting.musicVolume = value;
                    BroAudio.SetVolume(BroAudioType.Music, value);
                    break;
                case SoundSliderType.SFX:
                    Model.CurrentSoundSetting.sfxVolume = value;
                    BroAudio.SetVolume(BroAudioType.SFX, value);
                    break;
                case SoundSliderType.UI:
                    Model.CurrentSoundSetting.uiVolume = value;
                    BroAudio.SetVolume(BroAudioType.UI, value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(key), key, null);
            }
        }

        private void HandleSave()
        {
            var displayJson = JsonUtility.ToJson(Model.CurrentDisplaySetting);
            File.WriteAllText(Model.DisplaySettingFilePath, displayJson);

            var soundJson = JsonUtility.ToJson(Model.CurrentSoundSetting);
            File.WriteAllText(Model.SoundSettingFilePath, soundJson);

            _uiChannel.RaiseEvent(UIEvents.UIStateChangeEvent.Initializer(_prevState));
        }

        protected override void OnModelChanged()
        {
        }

        private void ChangeResolution(DisplaySettingData targetData)
        {
            int width, height;
            switch (targetData.resolution)
            {
                case ResolutionType.R_1920x1080:
                    width = 1920;
                    height = 1080;
                    break;
                case ResolutionType.R_1920x1200:
                    width = 1920;
                    height = 1200;
                    break;
                case ResolutionType.R_2560x1080:
                    width = 2560;
                    height = 1080;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Screen.SetResolution(width, height, targetData.isFullscreen);
        }

        private void HandleSoundSettingModified()
        {
            // Implement sound setting application logic here........
            // Damn we dont have audio manager yet :,(
        }
    }
}