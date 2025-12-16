using System;
using System.Collections.Generic;
using Code.Effects;
using Code.Units.UnitStat;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Units.UnitAnimals.Skunks
{
    public class Skunk : FriendlyUnit
    {
        private UnitDetector _detector;
        private SkunkDataSO _skunkData;
        private Dictionary<Unit, PoolingEffect> _debuffingUnitDict;
        
        protected override void Awake()
        {
            base.Awake();
            _detector = GetCompo<UnitDetector>();
            _debuffingUnitDict = new Dictionary<Unit, PoolingEffect>();
            _skunkData = UnitData as SkunkDataSO;
        }
        
        protected override void Update()
        {
            base.Update();
        
            List<Unit> toRemoveUnit = new();
            foreach (var unit in _debuffingUnitDict.Keys)
            {
                float distance = Vector3.Distance(transform.position, unit.transform.position);
                    
                if (distance > _skunkData.slowRange + 0.2f) // 만약에 거리를 벗어났으면
                {
                    toRemoveUnit.Add(unit);
                }
            }

            foreach (var unit in toRemoveUnit)
            {
                unit.GetCompo<UnitStatCompo>().GetStat(UnitStatType.MoveSpeed)
                    .RemoveModifier(this);
                _debuffingUnitDict[unit].StopPoolEffect();
                _debuffingUnitDict.Remove(unit);
            }
            
            // 일단 근처 적들 전부 찾기
            if (!_detector.TryGetUnits(out HashSet<Unit> units, _skunkData.slowRange, UnitTeamType.Enemy)) 
                return;
            
            foreach (var unit in units)
            {
                if (_debuffingUnitDict.ContainsKey(unit)) continue; // 만약 이미 있는애면 스킵
        
                PoolingEffect effect = PoolManagerMono.Instance.Pop<PoolingEffect>(_skunkData.debuffEffectItem);
                effect.transform.SetParent(unit.transform); // 적한테 이펙트 고정함

                float effectYOffset = 0f;
                float effectSize = 0f;

                switch (unit.UnitData.sizeType)
                {
                    case UnitSizeType.SMALL:
                        effectYOffset = 1.25f;
                        effectSize = 1.3f;
                        break;
                    case UnitSizeType.MIDDLE:
                        effectYOffset = 2.75f;
                        effectSize = 2f;
                        break;
                    case UnitSizeType.LARGE:
                        effectYOffset = 3.75f;
                        effectSize = 2.5f;
                        break;
                }
                
                effect.transform.localScale *= effectSize;
                effect.PlayVFX(unit.transform.position + Vector3.up * effectYOffset, Quaternion.identity);
                _debuffingUnitDict.Add(unit, effect);
                unit.GetCompo<UnitStatCompo>().GetStat(UnitStatType.MoveSpeed)
                    .AddModifier(this, _skunkData.slowCoefficient);
            }
        }
    }
}