using System;
using System.Collections.Generic;
using Code.Units.UnitDatas;
using EventSystem;
using GondrLib.Dependencies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cannons.Slots
{
    [DefaultExecutionOrder(-1)]
    public class AutoRandomSlotSetter : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO slotChannel;
        [SerializeField] private UnitDataSO[] defaultUnits; 
        [Inject] private CannonSlot _slot;
        
        private Dictionary<float, UnitDataSO> _slotDataDict;
        private float _totalPercent;
        
        private void Awake()
        {
            _slotDataDict = new Dictionary<float, UnitDataSO>();

            foreach (var unit in defaultUnits)
            {
                AddUnit(unit);
            }
            
            _slot.OnSlotUsedEvent += HandleAddRandomUnit;
            slotChannel.AddListener<UnitUnlockToSlotEvent>(HandleUnlockUnit);
        }

        private void Start()
        {
            for (int i = 0; i < _slot.MaxSlotSize; i++)
            {
                HandleAddRandomUnit();
            }
        }

        private void OnDestroy()
        {
            _slot.OnSlotUsedEvent -= HandleAddRandomUnit;
            slotChannel.RemoveListener<UnitUnlockToSlotEvent>(HandleUnlockUnit);
        }

        private void HandleAddRandomUnit()
        {
            var unitData = GetRandomUnitData();
            
            _slot.AddSlot(unitData);
        }
        
        private UnitDataSO GetRandomUnitData()
        {
            float randomValue = Random.Range(0f, _totalPercent);

            foreach (var (percent, data) in _slotDataDict)
            {
                if (randomValue <= percent)
                {
                    return data;
                }
            }
            
            Debug.LogError("random slot data is null!");
            return null;
        }

        private void AddUnit(UnitDataSO data)
        {
            _totalPercent += (int)data.classType;
            _slotDataDict.Add(_totalPercent, data);
        }
        
        private void HandleUnlockUnit(UnitUnlockToSlotEvent evt)
        {
            AddUnit(evt.unitData);
        }
    }
}