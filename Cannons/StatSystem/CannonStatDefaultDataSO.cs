using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cannons.StatSystem
{
    [CreateAssetMenu(fileName = "CannonStatData", menuName = "SO/Cannon/Stat", order = 0)]
    public class CannonStatDefaultDataSO : ScriptableObject
    {
        public List<CannonStatData> statSettings;
    }

    [Serializable]
    public struct CannonStatData
    {
        public float value;
        public CannonStatType statType;
    }
}