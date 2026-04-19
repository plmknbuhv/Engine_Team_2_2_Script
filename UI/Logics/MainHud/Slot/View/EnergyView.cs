using Code.UI.Visual;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Logics.MainHud.Slot.View
{
    public class EnergyView : BaseView
    {
        [SerializeField] private Image fillerImage;
        [SerializeField] private TextMeshProUGUI energyText;
        [SerializeField] private UIElement uiElement;
        [Header("Fill Animation Settings")]
        [SerializeField] private float fillDuration = 0.1f;
        [SerializeField] private Ease ease = Ease.OutCirc;
        private Tweener _valueTweener;
        private int _maxValue;
        
        private const string MaxValueKey = "max";
        private const string ConsumeValueKey = "consume";

        public void InitializeView(int max)
        {
            _maxValue = max;
            energyText.text = "0";
            fillerImage.fillAmount = 0f;
            
            SetValue(0);
        }

        public void SetValue(int value)
        {
            if (value >= _maxValue)
            {
                uiElement.AddState(MaxValueKey).Forget();
            }
            energyText.text = (Mathf.Clamp(value, 0, 99)).ToString();
            SetFiller((float)value / _maxValue);
        }

        private void SetFiller(float fillAmount)
        {
            DOTween.Kill(_valueTweener);
            var startValue = fillerImage.fillAmount;
            _valueTweener = DOVirtual.Float(startValue, fillAmount, fillDuration,
                value => fillerImage.fillAmount = value).SetEase(ease);
        }

        public void ConsumeEnergy()
        {
            uiElement.PlayFeedback(ConsumeValueKey);
            uiElement.RemoveState(MaxValueKey).Forget();
        }
    }
}