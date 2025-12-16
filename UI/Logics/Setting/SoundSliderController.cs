using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Logics.Setting
{
    public class SoundSliderController : MonoBehaviour
    {
        [field: SerializeField] public SoundSliderType SliderType { get; private set; }
        [SerializeField] private Slider slider; 
        public Action<SoundSliderType, float> OnValueChanged;
        
        public void InitializeValue(float value)
        {
            slider.value = value;
            slider.onValueChanged.AddListener(HandleValueChanged);
        }
        
        private void OnDestroy()
        {
            slider.onValueChanged.RemoveListener(HandleValueChanged);
        }
        
        private void HandleValueChanged(float arg0)
        {
            OnValueChanged?.Invoke(SliderType, arg0);
        }
    }
    public enum SoundSliderType
    {
        MASTER,
        BGM,
        SFX,
        UI
    }
}