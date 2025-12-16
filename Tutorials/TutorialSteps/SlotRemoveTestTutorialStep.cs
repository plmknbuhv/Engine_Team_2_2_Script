using EventSystem;
using Settings.InputSystem;
using UnityEngine;

namespace Code.Tutorials.TutorialSteps
{
    public class SlotRemoveTestTutorialStep : TutorialStep
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private GameEventChannelSO slotChannel;
        [SerializeField] private int testCount = 2;

        private int _currentTestCount;
        
        public override void Enter()
        {
            base.Enter();
            
            slotChannel.AddListener<SlotRemoveEvent>(HandleSlotRemove);
            playerInput.SetInputEnabled(true);
            playerInput.SetEnableInputAction(true, UseInputType.REMOVE);
        }

        private void HandleSlotRemove(SlotRemoveEvent evt)
        {
            _currentTestCount++;

            if (_currentTestCount >= testCount)
            {
                NextStep();
            }
        }

        public override void End()
        {
            slotChannel.RemoveListener<SlotRemoveEvent>(HandleSlotRemove);
            
            playerInput.SetEnableInputAction(false, UseInputType.REMOVE);
            playerInput.SetInputEnabled(false);
            
            base.End();
        }
    }
}