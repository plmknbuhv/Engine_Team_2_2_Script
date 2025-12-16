using EventSystem;
using UnityEngine;

namespace Cannons
{
    public class CannonUsedChecker : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO cannonChannel;
        [SerializeField] private GameEventChannelSO slotChannel;
        [SerializeField] private int wantUseCount = 3;
        
        private int _currentUseCount;
        private int _canRemoveUnitCount;

        public int CanRemoveUnitCount
        {
            get => _canRemoveUnitCount;
            set
            {
                var prevValue = _canRemoveUnitCount;
                _canRemoveUnitCount = value;
                if (prevValue != _canRemoveUnitCount)
                {
                    var removeCountEvt = SlotEvents.CanRemoveCountChangeEvent.Initializer(_canRemoveUnitCount, _currentUseCount);
                    slotChannel.RaiseEvent(removeCountEvt);
                }
            }
        }
        public bool CanRemoveUnit => CanRemoveUnitCount > 0;
        
        private void Awake()
        {
            cannonChannel.AddListener<CannonShootEvent>(HandleUseCannon);
            slotChannel.AddListener<SlotRemoveEvent>(HandleUseRemoveSlot);
        }

        private void OnDestroy()
        {
            cannonChannel.RemoveListener<CannonShootEvent>(HandleUseCannon);
            slotChannel.RemoveListener<SlotRemoveEvent>(HandleUseRemoveSlot);
        }

        private void HandleUseCannon(CannonShootEvent evt)
        {
            _currentUseCount++;
            
            if( _currentUseCount >= wantUseCount)
            {
                CanRemoveUnitCount++;
                _currentUseCount = 0;
            }

            var removeCountEvt = SlotEvents.CanRemoveCountChangeEvent.Initializer(_canRemoveUnitCount, _currentUseCount);
            slotChannel.RaiseEvent(removeCountEvt);
        }
        
        private void HandleUseRemoveSlot(SlotRemoveEvent evt)
        {
            CanRemoveUnitCount--;
        }
    }
}