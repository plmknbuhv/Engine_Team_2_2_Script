using System.Collections.Generic;
using Code.Units.UnitDatas;
using Code.Units.UnitStat;
using UnityEngine;

namespace Code.Units.UnitAnimals.SeaHorses
{
    [CreateAssetMenu(fileName = "SeaHorseData", menuName = "SO/Unit/UnitData/SeaHorse", order = 0)]
    public class SeaHorseDataSO : UnitDataSO
    {
        public List<StatAdder> statMultipliers;
        public float buffRange;
    }
}