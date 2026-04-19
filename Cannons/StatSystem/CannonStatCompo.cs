using System.Collections.Generic;
using EventSystem;
using GondrLib.Dependencies;
using UnityEngine;

namespace Cannons.StatSystem
{
    [Provide]
    public class CannonStatCompo : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private GameEventChannelSO cannonChannel;
        [SerializeField] private CannonStatDefaultDataSO defaultData;

        private Dictionary<CannonStatType, float> _statDict;
        
        private void Awake()
        {
            _statDict = new Dictionary<CannonStatType, float>();
            defaultData.statSettings.ForEach(data => _statDict.Add(data.statType, data.value));
            
            cannonChannel.AddListener<CannonStatUpgradeEvent>(HandleStatUpgrade);
        }

        private void OnDestroy()
        {
            cannonChannel.RemoveListener<CannonStatUpgradeEvent>(HandleStatUpgrade);
        }

        private void HandleStatUpgrade(CannonStatUpgradeEvent evt)
        {
            AddStat(evt.statType, evt.addValue);
        }

        public void AddStat(CannonStatType statType, float value)
        {
            if (_statDict.TryGetValue(statType, out float statValue))
            {
                 float newValue = statValue + value;
                 if(newValue < 0) newValue = 0.1f;
                 
                 _statDict[statType] = newValue;
            }
            else
            {
                _statDict.Add(statType, value);
            }
        }

        public float GetStatValue(CannonStatType statType)
        {
            return _statDict[statType];
        }
    }
}