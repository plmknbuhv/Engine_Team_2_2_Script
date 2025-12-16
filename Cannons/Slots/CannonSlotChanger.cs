using System;
using Code.CostSystem;
using EventSystem;
using GondrLib.Dependencies;
using Settings.InputSystem;
using UnityEngine;

namespace Cannons.Slots
{
    public class CannonSlotChanger : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO inputData;
        [SerializeField] private float delayTime = 0.5f;
        [SerializeField] private GameEventChannelSO slotChannel;
        [SerializeField] private int needCost = 1;
        
        [Inject] private CostManager costM;
        
        private float _nextTime;
        private bool _canRemoveSlot;
        
        private void Awake()
        {
            inputData.OnRemovePressed += HandleSlotChange;
            _nextTime = Time.time + delayTime;
        }

        private void OnDestroy()
        {
            inputData.OnRemovePressed -= HandleSlotChange;
        }

        private void Update()
        {
            if (Time.time >= _nextTime)
            {
                _nextTime = Time.time + delayTime;
                _canRemoveSlot = true;
            }
        }

        private void HandleSlotChange()
        {
            if (!_canRemoveSlot) return;
            if (!costM.CheckUseCost(needCost)) return;
            
            var evt = SlotEvents.SlotRemoveEvent.Initializer(0);
            slotChannel.RaiseEvent(evt);
            
            costM.AddCost(-needCost);
            
            _canRemoveSlot = false;
        }
    }
}