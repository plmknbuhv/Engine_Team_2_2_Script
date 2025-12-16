using System.Collections.Generic;
using UnityEngine;

namespace Code.Units.UnitStat
{
    /// <summary>
    /// 그 버프나 디버프 같은거 할려고 만듬
    /// 아니 근데 스탯 종류별로 이벤트 나누기엔 비 효율 적일 것 같아서 걍 이렇게 만듬
    /// </summary>
    [CreateAssetMenu(fileName = "UnitStatMultiplyManager", menuName = "SO/Unit/Stat/UnitStatMultiplyManager", order = 0)]
    public class UnitStatMultiplyManagerSO : ScriptableObject 
    {
        public List<UnitStatMultiplySO> statMultiplyList;

        public void AddStatMultiply(object key, float value, UnitTeamType teamType, UnitStatType statTypeType) 
            => GetStatMultiply(teamType, statTypeType).AddModifier(key, value);

        public void RemoveStatMultiply(object key, UnitTeamType teamType, UnitStatType statTypeType) 
            => GetStatMultiply(teamType, statTypeType).RemoveModifier(key);

        public UnitStatMultiplySO GetStatMultiply(UnitTeamType teamType, UnitStatType statTypeType)
        {
            foreach (var statMultiply in statMultiplyList)
            {
                if (statMultiply.teamType == teamType && statMultiply.statTypeType == statTypeType)
                    return statMultiply;
            }
            
            Debug.LogError("찾으려는 StatMultiply가 없음");
            return null;
        }
    }
}