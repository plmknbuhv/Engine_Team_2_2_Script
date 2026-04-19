using Settings.InputSystem;
using Unity.Cinemachine;
using UnityEngine;

namespace Code.Tutorials.TutorialSteps
{
    public class CameraZoomTestTutorialStep : TutorialStep
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private float wantMinZoomSize = 40f;
        private CinemachineCamera _camera;

        private void Awake()
        {
            _camera = FindAnyObjectByType<CinemachineCamera>();
        }

        public override void Enter()
        {
            base.Enter();
            playerInput.SetInputEnabled(true);
            playerInput.SetEnableInputAction(true, UseInputType.SCROLL);
        }

        public override void UpdateStep()
        {
            base.UpdateStep();
            if (_camera.Lens.FieldOfView <= wantMinZoomSize)
            {
                NextStep();
            }
        }

        public override void End()
        {
            playerInput.SetEnableInputAction(false, UseInputType.SCROLL);
            playerInput.SetInputEnabled(false);
            base.End();
        }
    }
}