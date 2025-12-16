using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Logics.MainHud.Stat.View
{
    public class StatView : BaseView
    {
        [SerializeField] private float fillDuration = 0.5f;
        [SerializeField] private Image expFill;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI expText;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Slider energySlider;
        [SerializeField] private TextMeshProUGUI energyText;

        public void SetExpFill(float current, float max)
        {
            float fillAmount = current / max;
            // expBarFill.fillAmount = fillAmount;
            DOVirtual.Float(expFill.fillAmount, fillAmount, fillDuration, value => expFill.fillAmount = value);
            expText.text = $"{Mathf.FloorToInt(current)}/{Mathf.FloorToInt(max)}";
        }

        public void SetLevel(int level) => levelText.text = $"{level}";

        public void SetHealth(int current, int max = -1)
        {
            if (max != -1)
                healthSlider.maxValue = max;
            // healthSlider.value = current;
            DOVirtual.Float(healthSlider.value, current, fillDuration, value => healthSlider.value = value);
            healthText.text = $"{current}/{healthSlider.maxValue}";
        }

        public void SetEnergy(int current, int max = -1)
        {
            if (max != -1)
                energySlider.maxValue = max;
            // energySlider.value = current;
            DOVirtual.Float(energySlider.value, current, fillDuration, value => energySlider.value = value);
            energyText.text = $"{current}/{energySlider.maxValue}";
        }
    }
}