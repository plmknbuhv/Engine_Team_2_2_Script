using System;
using EventSystem;
using GondrLib.Dependencies;
using Settings.InputSystem;
using UnityEngine;

namespace Cannons
{
    public class CannonAimCompo : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO cannonChannel;
        [SerializeField] private GameEventChannelSO parabolaChannel;
        [SerializeField] private PlayerInputSO inputData;
        [SerializeField] private CannonLookCompo lookCompo;

        private bool _isCharging;

        private void Awake()
        {
            parabolaChannel.AddListener<ParabolaChangeEvent>(HandleParabolaChanged);
            cannonChannel.AddListener<CannonChargingEvent>(HandleCannonCharging);
        }

        private void OnDestroy()
        {
            parabolaChannel.RemoveListener<ParabolaChangeEvent>(HandleParabolaChanged);
            cannonChannel.RemoveListener<CannonChargingEvent>(HandleCannonCharging);
        }

        private void Update()
        {
            Vector3 mousePos = inputData.GetMouseWorldPosition();

            if(!_isCharging)
                lookCompo.BodyLookAtTarget(mousePos);
        }

        private void HandleParabolaChanged(ParabolaChangeEvent evt)
        {
            if (!inputData) return;

            Vector3[] points = evt.points;
            Vector3 pointPos = points[points.Length / 10];
            
            lookCompo.LookAtShootPoint(pointPos);
        }
        
        private void HandleCannonCharging(CannonChargingEvent evt)
        {
            _isCharging = evt.isCharging;
        }
    }
}