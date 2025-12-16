using EventSystem;
using Settings.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Tutorials.TutorialSteps
{
    public class CannonChargingTestTutorialStep : TutorialStep
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private GameEventChannelSO cannonChannel;
        [SerializeField] private int testCount;

        private int _currentTestCount;
        
        public override void Enter()
        {
            base.Enter();
            
            cannonChannel.AddListener<CannonShootEvent>(HandleCannonShootTest);
            playerInput.SetInputEnabled(true);
            playerInput.SetEnableInputAction(true, UseInputType.POINTER | UseInputType.SHOOT);
        }

        private void HandleCannonShootTest(CannonShootEvent evt)
        {
            _currentTestCount++;
            
            if (testCount <= _currentTestCount)
                NextStep();
        }

        public override void End()
        {
            cannonChannel.RemoveListener<CannonShootEvent>(HandleCannonShootTest);
            
            playerInput.SetEnableInputAction(false, UseInputType.POINTER | UseInputType.SHOOT);
            playerInput.SetInputEnabled(false);
            base.End();
        }
    }
}