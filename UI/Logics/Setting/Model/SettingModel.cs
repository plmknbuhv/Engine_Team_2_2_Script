using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.UI.Logics.Setting.Model
{
    public class SettingModel : BaseModel
    {
        private DisplaySettingData _currentDisplaySetting;
        public DisplaySettingData CurrentDisplaySetting { 
            get=> _currentDisplaySetting;
            set
            {
                _currentDisplaySetting = value;
                NotifyChanged();
            }
        }
        
        private SoundSettingData _currentSoundSetting;
        public SoundSettingData CurrentSoundSetting
        {
            get => _currentSoundSetting;
            set
            {
                _currentSoundSetting = value;
                NotifyChanged();
            }
        }
        
        private string _displaySettingFileName = "display_settings.json";
        public string DisplaySettingFilePath => $"{Application.persistentDataPath}/{_displaySettingFileName}";
        
        private string _soundSettingFileName = "sound_settings.json";
        public string SoundSettingFilePath => $"{Application.persistentDataPath}/{_soundSettingFileName}";

        public static void DeleteJsonData()
        {
            string displayPath = $"{Application.persistentDataPath}/display_settings.json";
            if (System.IO.File.Exists(displayPath))
            {
                System.IO.File.Delete(displayPath);
            }
            string soundPath = $"{Application.persistentDataPath}/sound_settings.json";
            if (System.IO.File.Exists(soundPath))
            {
                System.IO.File.Delete(soundPath);
            }
        }
    }
}