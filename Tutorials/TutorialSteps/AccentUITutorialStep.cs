using DG.Tweening;
using Settings.InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Tutorials.TutorialSteps
{
    public class AccentUITutorialStep : TutorialStep
    {
        [SerializeField] private UIInputSO uiInput;
        [SerializeField] private Image accentImage;

        private void Awake()
        {
            var newAlphaColor = accentImage.color;
            newAlphaColor.a = 0f;
            accentImage.color = newAlphaColor;
        }

        public override void Enter()
        {
            base.Enter();
            uiInput.OnClickPressed += HandleClickUI;
            
            var newAlphaColor = accentImage.color;
            newAlphaColor.a = 0.75f;
            accentImage.DOColor(newAlphaColor, 0.2f).SetEase(Ease.OutSine);
        }

        private void HandleClickUI()
        {
            NextStep();
        }
        
        public override void End()
        {
            var newAlphaColor = accentImage.color;
            newAlphaColor.a = 0f;
            accentImage.DOColor(newAlphaColor, 0.2f).SetEase(Ease.InQuad);
            uiInput.OnClickPressed -= HandleClickUI;
            base.End();
        }
    }
}