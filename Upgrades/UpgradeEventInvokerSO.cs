using System;
using System.Collections.Generic;
using Code.Upgrades.Data;
using UnityEngine;

namespace Code.Upgrades
{
    [CreateAssetMenu(fileName = "UpgradeInvoker", menuName = "SO/Upgrade/EventInvoker", order = 0)]
    public class UpgradeEventInvokerSO : ScriptableObject
    {
        ///UpgradeDataSO의 id를 Key값으로 받는다.
        private Dictionary<UpgradeDataSO, Action<int, UpgradeDataSO>> _eventDict;

        public void Initialize()
        {
            _eventDict = new Dictionary<UpgradeDataSO, Action<int, UpgradeDataSO>>();
        }

        public void RaiseEvent(UpgradeDataSO data, int level)
        {
            if (_eventDict.TryGetValue(data, out Action<int, UpgradeDataSO> action))
            {
                action?.Invoke(level, data);
            }
            else
                Debug.LogWarning($"ID:{data.upgradeID} event not found");
        }

        public void Subscribe(UpgradeDataSO data, Action<int, UpgradeDataSO> action)
        {
            if (!_eventDict.TryAdd(data, action))
            {
                _eventDict[data] += action;
            }
        }

        public void Unsubscribe(UpgradeDataSO data, Action<int, UpgradeDataSO> action)
        {
            if (_eventDict.TryGetValue(data, out Action<int, UpgradeDataSO> handler))
            {
                handler -= action;
                if (handler == null)
                {
                    _eventDict.Remove(data);
                    return;
                }
                
                _eventDict[data] = handler;
            }
        }
    }
}