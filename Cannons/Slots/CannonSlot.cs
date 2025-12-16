using System;
using Code.CostSystem;
using Code.Units;
using Code.Units.UnitDatas;
using EventSystem;
using GondrLib.Dependencies;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Cannons.Slots
{
    [Provide]
        
    public class CannonSlot : MonoBehaviour, IDependencyProvider
    {
        #region Events session

        public event Action<Unit> OnCannonShootEvent;
        public event Action OnSlotUsedEvent;
        public event Action<UnitDataSO> OnUnitAddEvent;

        #endregion
        
        public CircularQueue<UnitDataSO> UnitQueue { get; private set; } //나중에 Unit정보를 담도록 변경

        [Header("Channel")]
        [SerializeField] private GameEventChannelSO synergyChannel;
        [SerializeField] private GameEventChannelSO unitChannel;
        [SerializeField] private GameEventChannelSO slotChannel;
        [SerializeField] private GameEventChannelSO costChannel;
        [SerializeField] private GameEventChannelSO cannonChannel;
        
        [Header("Settings")]
        [field: SerializeField] public int MaxSlotSize { get; private set; } = 9;
        [SerializeField] private UnitDataSO nextElementData; //Test

        [Inject] private CostManager costManager;
        [Inject] private PoolManagerMono _poolM;
        
        private void Awake()
        {
            UnitQueue = new CircularQueue<UnitDataSO>(MaxSlotSize);
            
            costChannel.AddListener<ChangeCostAmountEvent>(HandleCostChanged);
            slotChannel.AddListener<SlotRemoveEvent>(HandleRemoveSlot);
        }

        private void OnDestroy()
        {
            costChannel.RemoveListener<ChangeCostAmountEvent>(HandleCostChanged);
            slotChannel.RemoveListener<SlotRemoveEvent>(HandleRemoveSlot);
        }

        public Unit SpawnUnit(Vector3 spawnPosition = default)
        {
            if (UnitQueue.IsEmpty) return null;
            
            //나중에 변경할 예정
            var unitData = CheckNextUnit();
            
            if (!costManager.CheckUseCost(unitData.requiredCost)) return null;

            PopUnit();
            var unit = _poolM.Pop<Unit>(unitData.poolItem);
            unit.transform.position = spawnPosition;
            
            costManager.SubCost(unitData.requiredCost);
            
            OnSlotUsedEvent?.Invoke();
            OnCannonShootEvent?.Invoke(unit);
            
            return unit;
        }
        
        private void HandleRemoveSlot(SlotRemoveEvent evt)
        {
            RemoveSlotUnit(evt.slotIndex);
        }
        
        private void HandleCostChanged(ChangeCostAmountEvent evt)
        {
            bool canShoot = CheckRequiredUnitCost(evt.amount);
            var checkEvt = CannonEvents.CannonCheckCanShootEvent.Initializer(canShoot);
            cannonChannel.RaiseEvent(checkEvt);
        }
        
        /// <summary>
        /// 슬롯에 유닛을 추가한다.
        /// </summary>
        public void AddSlot(UnitDataSO unitData)
        {
            if (UnitQueue.IsFull)
            {
                Debug.LogWarning("Slots are full");
                return;
            }
            
            UnitQueue.Enqueue(unitData);
            
            OnUnitAddEvent?.Invoke(unitData);
            slotChannel.RaiseEvent(SlotEvents.SlotAddEvent.Initializer(unitData));
        }
        
        /// <summary>
        /// 사용할 유닛을 슬롯에서 제거하고 반환한다.
        /// </summary>
        /// <returns></returns>
        public UnitDataSO PopUnit()
        {
            if (UnitQueue.IsEmpty)
            {
                Debug.LogWarning("Slots are empty");
                return null;
            }
            
            return UnitQueue.Dequeue();
        }

        /// <summary>
        /// 다음 유닛을 확인한다.
        /// </summary>
        public UnitDataSO CheckNextUnit()
        {
            if (UnitQueue.IsEmpty)
            {
                Debug.LogWarning("Slots are empty");
                return null;
            }
            
            return UnitQueue.Peek();
        }

        public void RemoveSlotUnit(int idx)
        {
            if (UnitQueue.IsEmpty)
            {
                Debug.LogWarning("Slots are empty");
                return;
            }
            
            UnitQueue.RemoveIdx(idx);
            OnSlotUsedEvent?.Invoke();
        }
        
        ///유닛을 소환할 수 있는 만큼의 코스트가 있는지 확인
        private bool CheckRequiredUnitCost(int cost)
        {
            var data = CheckNextUnit();

            if (!data) return false;
            return data.requiredCost <= cost;
        }
        
        [ContextMenu("Add Unit Element")]
        public void AddUnitElement()
        {
            AddSlot(nextElementData);
        }
    }
}