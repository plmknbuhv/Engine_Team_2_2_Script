using Settings.InputSystem;
using UnityEngine;

namespace Code.Tutorials.TutorialSteps
{
    public class OnlyMessageTutorialStep : TutorialStep
    {
        [SerializeField] private UIInputSO uiInput;

        public override void Enter()
        {
            base.Enter();
            uiInput.OnClickPressed += HandleClickUI;
        }

        private void HandleClickUI()
        {
            NextStep();
        }
        
        public override void End()
        {
            uiInput.OnClickPressed -= HandleClickUI;
            base.End();
        }
    }
}