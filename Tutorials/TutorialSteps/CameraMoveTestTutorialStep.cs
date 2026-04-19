using Settings.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Tutorials.TutorialSteps
{
    public class CameraMoveTestTutorialStep : TutorialStep
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private int moveCount = 5; 
        private int _currentMoveCount;
        
        
        public override void Enter()
        {
            base.Enter();
            playerInput.SetInputEnabled(true);
            playerInput.SetEnableInputAction(true, UseInputType.MOVE);
            
            playerInput.OnMoveDirectionValueChanged += HandleMoveTest;
        }

        private void HandleMoveTest(bool isPress)
        {
            if (!isPress)
            {
                _currentMoveCount++;

                if (_currentMoveCount >= moveCount)
                {
                    NextStep();
                }
            }
        }

        public override void End()
        {
            playerInput.OnMoveDirectionValueChanged -= HandleMoveTest;
            playerInput.SetEnableInputAction(false, UseInputType.MOVE);
            playerInput.SetInputEnabled(false);
            base.End();
        }
    }
}