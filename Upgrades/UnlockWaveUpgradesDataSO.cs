using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Upgrades
{
    [CreateAssetMenu(fileName = "WaveUnlockData", menuName = "SO/Upgrade/WaveUnlockData", order = 0)]
    public class UnlockWaveUpgradesDataSO : ScriptableObject
    {
        public List<UnlockWaveData> waveGroup;
    }

    [Serializable]
    public struct UnlockWaveData
    {
        public int waveCount;
        public UpgradeGroupSO group;
    }
}