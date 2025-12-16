using System;
using System.Collections.Generic;
using System.Linq;
using Code.Entities;
using UnityEngine;

namespace Code.Units
{
    public class UnitDetector : MonoBehaviour, IEntityComponent
    {
        public float yThreshold = 1f;
        [SerializeField] private bool findEnemy = true;
        [SerializeField] private int detectCount = 5;

        private Collider[] _colliders;
        private Unit _unit;
        private UnitTeamType _teamType;

        public void Initialize(Entity entity)
        {
            _unit = entity as Unit;
            Debug.Assert(_unit != null, "유닛이 아니당당당");

            if (findEnemy)
            {
                _teamType = _unit.TeamType;
            }
            else
            {
                SetTarget(_unit.TeamType);
            }

            _colliders = new Collider[detectCount]; // 설마 5명 이상이 근처에 있을까 싶어서 이렇게 함
        }

        public void SetTarget(UnitTeamType targetType)
        {
            _teamType = targetType switch
            {
                UnitTeamType.Enemy => UnitTeamType.Friendly,
                UnitTeamType.Friendly => UnitTeamType.Enemy,
                _ => UnitTeamType.None
            };
        }

        public bool TryGetClosestEnemy<T>(out T enemy, float detectRange, Func<T, bool> additionalCondition, UnitTeamType targetType = UnitTeamType.None) where T : Unit
        {
            enemy = null;

            if (TryGetUnits(out HashSet<T> units, detectRange, targetType, additionalCondition) && units.Count > 0)
            {
                float closestDistance = float.MaxValue;

                foreach (T unit in units)
                {
                    float distance = Vector3.Distance(unit.transform.position, _unit.transform.position);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        enemy = unit;
                    }
                }

                return enemy != null;
            }

            return false;
        }

        public bool TryGetUnits<T>(out HashSet<T> units, float detectRange, UnitTeamType targetType = UnitTeamType.None, Func<T, bool> additionalCondition = null)
            where T : Unit
        {
            if(targetType != UnitTeamType.None) SetTarget(targetType);
                
            //debug code
            for (int i = 0; i < detectCount; i++)
                _colliders[i] = null;
            
            const string unitLayer = "Unit";
            units = new HashSet<T>();

            int cnt = Physics.OverlapSphereNonAlloc(
                _unit.transform.position, detectRange, _colliders, LayerMask.GetMask(unitLayer));
            
            if (cnt > 0)
            {
                foreach (Collider coll in _colliders[0..cnt])
                {
                    if (coll.TryGetComponent(out T unit))
                    {
                        bool isNotTarget = unit.TeamType == _teamType || unit == _unit || unit.IsDead;
                        bool isInCondition = additionalCondition == null || additionalCondition(unit);
                        if (isNotTarget || isInCondition == false) continue;

                        Vector3 targetPos = coll.transform.position;
                        Vector3 unitPos = _unit.transform.position;

                        bool isInYRange = Mathf.Abs(targetPos.y - unitPos.y) <= yThreshold;
                        if (isInYRange == false) continue;

                        units.Add(unit);
                    }
                }
            }
            
            return units.Count > 0;
        }

        public bool TryGetNonBattleEnemy<T>(out T enemy, float detectRange = 3.0f, UnitTeamType targetType = UnitTeamType.None,
            Func<T, bool> additionalCondition = null) where T : Unit
        {
            return TryGetClosestEnemy(out enemy, 
                detectRange,
                target => !target.IsInBattle && (additionalCondition?.Invoke(target) ?? true),
                targetType);
        }

        public bool TryGetBattleEnemy<T>(out T enemy, float detectRange = 3.0f, UnitTeamType targetType = UnitTeamType.None,
            Func<T, bool> additionalCondition = null) where T : Unit
        {
            return TryGetClosestEnemy(out enemy, 
                detectRange,
                target => target.IsInBattle && (additionalCondition?.Invoke(target) ?? true),
                targetType);
        }
    }
}