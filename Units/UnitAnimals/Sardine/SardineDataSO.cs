using System;
using System.Collections.Generic;
using Code.Units.UnitDatas;
using Code.Units.UnitStat;
using UnityEngine;

namespace Code.Units.UnitAnimals.Sardine
{
    [CreateAssetMenu(fileName = "SardineData", menuName = "SO/Unit/UnitData/Sardine", order = 0)]
    public class SardineDataSO : UnitDataSO
    {
        public int defaultLinkCnt = 3;
        public int maxLinkCount = 5;
        public List<StatAdder> statAdders; //연결된 정어리 한마리 당 증가하는 스탯
        public List<Color> linkColors;
        public Vector3 scaleAdder;
        public float sardineDetectRange;
    }
}