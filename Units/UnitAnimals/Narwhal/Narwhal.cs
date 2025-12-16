using System.Collections.Generic;
using Code.Units.UnitStat;
using UnityEngine;

namespace Code.Units.UnitAnimals.Narwhal
{
    public class Narwhal : EnemyUnit
    {
        
        public bool CanBuff
        {
            get => _canBuff;
            set
            {
                _canBuff = value;
                
                if(_canBuff)
                    buffParticle.Play();
                else
                    buffParticle.Stop();
            }
        }
        
        [SerializeField] private ParticleSystem buffParticle;
        
        private HashSet<Unit> _beforeTargets = new HashSet<Unit>();
        
        private NarwhalDataSO _narwhalData;
        private UnitDetector _unitDetector;
        
        private bool _canBuff;
        

        protected override void Awake()
        {
            base.Awake();

            _unitDetector = GetCompo<UnitDetector>();
            _canBuff = true;

            _narwhalData = UnitData as NarwhalDataSO;
            
            OnDeadEvent.AddListener(DeadHandle);
        }

        private void OnDestroy()
        {
            OnDeadEvent.RemoveListener(DeadHandle);
        }

        private void DeadHandle()
        {
            var targets = new List<Unit>(_beforeTargets);
            foreach (var beforeTarget in targets)
            {
                RemoveModifier(beforeTarget);
            }
            
            _beforeTargets.Clear();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if(CanBuff == false) return;
            
            if (_unitDetector.TryGetUnits(out HashSet<Unit> units, _narwhalData.skillRange, UnitTeamType.Enemy))
            {
                foreach (Unit unit in units)
                    if(_beforeTargets.Contains(unit) == false)
                        AddModifier(unit);
                
                var beforetargets = new List<Unit>(_beforeTargets);
                foreach (var beforeTarget in beforetargets)
                    if (units.Contains(beforeTarget) == false)
                        RemoveModifier(beforeTarget);
                
                return;
            }
            
            if(_beforeTargets.Count == 0) return;
            
            var targets = new List<Unit>(_beforeTargets);
            foreach (var beforeTarget in targets)
                RemoveModifier(beforeTarget);
        }

        private void RemoveModifier(Unit beforeTarget)
        {
            UnitStatCompo statCompo = beforeTarget.GetCompo<UnitStatCompo>();
            
            if (statCompo.TryGetStat(UnitStatType.MoveSpeed, out Stat moveStat))
                moveStat.RemoveModifier(this);
                    
            if (statCompo.TryGetStat(UnitStatType.AttackDelay, out Stat attackStat))
                moveStat.RemoveModifier(this);
                    
            if (statCompo.TryGetStat(UnitStatType.Damage, out Stat damageStat))
                moveStat.RemoveModifier(this);
            
            _beforeTargets.Remove(beforeTarget);
        }
        
        private void AddModifier(Unit unit)
        {
            if(unit is EnemyUnit { IsBoss: true }) return;
            
            UnitStatCompo statCompo = unit.GetCompo<UnitStatCompo>();
            
            if (statCompo.TryGetStat(UnitStatType.MoveSpeed, out Stat moveStat))
                moveStat.AddModifier(this, _narwhalData.moveSpeedStatMultiplier);
                    
            if (statCompo.TryGetStat(UnitStatType.AttackDelay, out Stat attackStat))
                attackStat.AddModifier(this, _narwhalData.attackDelayStatMultiplier);
                    
            if (statCompo.TryGetStat(UnitStatType.Damage, out Stat damageStat))
                damageStat.AddModifier(this, _narwhalData.damageStatMultiplier);

            _beforeTargets.Add(unit);
        }
    }
}