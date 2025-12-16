using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Units
{
    [CreateAssetMenu(fileName = "UnitCounterSO", menuName = "SO/Unit/UnitCounterSO", order = 0)]
    public class UnitCounterSO : ScriptableObject
    {
        private List<Unit> _friendlyUnits;
        private List<Unit> _enemyUnits;
        public int FriendlyUnitCounter => _friendlyUnits.Count;
        public int EnemyUnitCounter => _enemyUnits.Count;

        private void OnEnable()
        {
            _enemyUnits = new List<Unit>();
            _friendlyUnits = new List<Unit>();
        }

        public void AddUnit(UnitTeamType teamType, Unit unit)
        {
            if (teamType == UnitTeamType.Friendly)
                _friendlyUnits.Add(unit);
            else
                _enemyUnits.Add(unit);
        }
        public void RemoveUnit(UnitTeamType teamType, Unit unit)
        {
            if (teamType == UnitTeamType.Friendly)
                _friendlyUnits.Remove(unit);
            else
                _enemyUnits.Remove(unit);
        }
    }
}