using System;
using System.Collections.Generic;
using Code.Entities;
using Code.Units.UnitDatas;
using UnityEngine;

namespace Code.Units.UnitStat
{
    public class UnitStatCompo : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private UnitStatType debugType;
        [SerializeField] private bool isDebugKey = true;
        
        private Unit _unit;
        private Dictionary<UnitStatType, Stat> _stats;
        
        public void Initialize(Entity entity)
        {
            _stats = new Dictionary<UnitStatType, Stat>();
            
            _unit = entity as Unit;
            Debug.Assert(_unit != null, $"Unit is null : {entity.gameObject.name}");
            
            foreach (UnitStatType statType in Enum.GetValues(typeof(UnitStatType)))
            {
                _stats.Add(statType, new Stat(statType));
            }
        }

        public bool TryGetStat(UnitStatType statType, out Stat value) => _stats.TryGetValue(statType, out value);
        public Stat GetStat(UnitStatType statType) => _stats[statType];
        public void ResetStat(UnitStatType statType) => _stats[statType].ClearModifier();
        public void ResetStat()
        {
            foreach (Stat stat in _stats.Values)
            {
                stat.ClearModifier();
            }
        }

        [ContextMenu("Debug stat")]
        public void DebugStat()
        {
            if (_stats.TryGetValue(debugType, out Stat targetStat))
            {
                Debug.Log($"{debugType} stat of {_unit.gameObject.name} : {targetStat.Value}");

                if (isDebugKey)
                {
                    Debug.Log("========== Modify List ==========");

                    int cnt = 0;
                    
                    foreach (KeyValuePair<object, float> pair in targetStat.ModifyPair)
                    {
                        cnt++;
                        Debug.Log($"{cnt} : Modify Key - {pair.Key}, Value - {pair.Value}");
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Debug fail : not found key {debugType}");
            }
        }
    }
}