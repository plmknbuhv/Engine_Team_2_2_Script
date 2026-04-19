using System;
using DG.Tweening;
using Settings.InputSystem;
using Unity.Cinemachine;
using UnityEngine;

namespace Code.Cameras
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera cinemaCamera;
        [SerializeField] private CinemachineSplineDolly splineDolly;
        [SerializeField] private PlayerInputSO inputData;
        [SerializeField] private Transform viewPoint;

        [SerializeField] private float moveSpeed = 2f;

        [Header("Zoom")] [SerializeField] private float minZoomSize;
        [SerializeField] private float maxZoomSize;
        [SerializeField] private float zoomScale = 5;
        [SerializeField] private float defaultViewValue = 0.5f;
        
        private Vector3 defaultViewPosition;
        private Vector3 _currentViewPos;
        private float prevZoomValue;
        private bool _canZoomIn;
        

        private void Awake()
        {
            defaultViewPosition = viewPoint.position;

            inputData.OnScrollValueChanged += HandleScrollValueChanged;
            inputData.OnMoveDirectionValueChanged += HandleMoveValueChanged;
        }

        private void Start()
        {
            _canZoomIn = true;
            prevZoomValue = -1;
            
            splineDolly.CameraPosition = defaultViewValue;
        }

        private void OnDestroy()
        {
            inputData.OnScrollValueChanged -= HandleScrollValueChanged;
            inputData.OnMoveDirectionValueChanged -= HandleMoveValueChanged;
        }

        private void Update()
        {
            float moveDir = inputData.MoveDir;
            splineDolly.CameraPosition += -moveDir * moveSpeed * Time.deltaTime;
        }

        private void HandleMoveValueChanged(bool isPress)
        {
            _canZoomIn = !isPress;
            
            if (isPress)
            {
                viewPoint.DOMove(defaultViewPosition, 0.4f);
                DOTween.To(
                    () => cinemaCamera.Lens.FieldOfView,
                    x => cinemaCamera.Lens.FieldOfView = x,
                    maxZoomSize, 1f).SetEase(Ease.OutCubic);
                prevZoomValue = -1;
            }
        }
        
        private void HandleScrollValueChanged(float value)
        {
            if (!_canZoomIn) return;
            
            if (!Mathf.Approximately(Mathf.Sign(value), Mathf.Sign(prevZoomValue)))
            {
                if (value > 0)
                {
                    Vector3 lookPoint = inputData.GetMouseWorldPosition();
                    viewPoint.DOMove(lookPoint, 0.2f);
                }
                
            }
            
            float currentZoomValue = cinemaCamera.Lens.FieldOfView;
            currentZoomValue = Mathf.Clamp(currentZoomValue - value * zoomScale, minZoomSize, maxZoomSize);

            DOTween.To(
                () => cinemaCamera.Lens.FieldOfView,
                x => cinemaCamera.Lens.FieldOfView = x,
                currentZoomValue, 1f).SetEase(Ease.OutCubic);

            if (currentZoomValue >= maxZoomSize) viewPoint.DOMove(defaultViewPosition, 0.2f);

            prevZoomValue = value;
        }
    }
}