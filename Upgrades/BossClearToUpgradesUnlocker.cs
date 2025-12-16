using System;
using System.Collections.Generic;
using EventSystem;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Upgrades
{
    public class BossClearToUpgradesUnlocker : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO upgradeChannel;
        [SerializeField] private GameEventChannelSO waveChannel;
        [SerializeField] private UnlockWaveUpgradesDataSO data;

        private Dictionary<int, UpgradeGroupSO> _groupDict;

        private void Awake()
        {
            _groupDict = new Dictionary<int, UpgradeGroupSO>();

            data.waveGroup.ForEach(d =>
            {
                if (!_groupDict.TryAdd(d.waveCount, (UpgradeGroupSO)d.group.Clone()))
                {
                    foreach (var addData in d.group.upgradeTargets)
                    {
                        _groupDict[d.waveCount].upgradeTargets.Add(addData);
                    }
                }
            });

            waveChannel.AddListener<BossWaveCompleteEvent>(HandleBossComplete);
        }

        private void OnDestroy()
        {
            waveChannel.RemoveListener<BossWaveCompleteEvent>(HandleBossComplete);
        }

        private void HandleBossComplete(BossWaveCompleteEvent evt)
        {
            if (_groupDict.TryGetValue(evt.bossWaveCount, out var group))
            {
                foreach (var upgrader in group.upgradeTargets)
                {
                    var unlockEvt = UpgradeEvents.UnlockUpgradeEvent.Initializer(upgrader);
                    upgradeChannel.RaiseEvent(unlockEvt);
                }
            }
            else
            {
                Debug.Log($"{evt.bossWaveCount}Boss unlock upgrader is null");
            }
        }
    }
}