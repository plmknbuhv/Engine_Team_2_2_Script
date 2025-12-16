using System.Collections.Generic;
using Code.Units.UnitStat;
using UnityEngine;

namespace Code.Units.UnitAnimals.SeaHorses
{
    public class SeaHorse : WaveRideableUnit
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
        
        private SeaHorseDataSO _seaHorseData;
        private UnitDetector _unitDetector;
        private bool _canBuff;
        
        private HashSet<Unit> _beforeTargets = new HashSet<Unit>();
        private readonly HashSet<Unit> _removedUnit = new HashSet<Unit>();
        private readonly HashSet<Unit> _detectedUnit = new HashSet<Unit>();
        
        private readonly string _buffKey = "SeaHorse";

        protected override void Awake()
        {
            base.Awake();

            _unitDetector = GetCompo<UnitDetector>();
            _seaHorseData = UnitData as SeaHorseDataSO;
            
            OnDeadEvent.AddListener(HandleDead);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            OnDeadEvent.RemoveListener(HandleDead);
        }

        public override void SetUpUnit()
        {
            base.SetUpUnit();

            CanBuff = true;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if(CanBuff == false) return;

            _unitDetector.SetTarget(UnitTeamType.Friendly);
            if (_unitDetector.TryGetUnits(out HashSet<Unit> buffTargets, _seaHorseData.buffRange))
            {
                _removedUnit.Clear();
                _removedUnit.UnionWith(_beforeTargets);
                _removedUnit.ExceptWith(buffTargets);

                _detectedUnit.Clear();
                _detectedUnit.UnionWith(buffTargets);
                _detectedUnit.ExceptWith(_beforeTargets);

                _beforeTargets = new HashSet<Unit>(buffTargets);

                ApplyMultipliers(_removedUnit, true);
                ApplyMultipliers(_detectedUnit, false);
            }
            else
            {
                ApplyMultipliers(_beforeTargets, true);
                _beforeTargets.Clear();
            }
        }

        private void HandleDead()
        {
            ApplyMultipliers(_beforeTargets, true);
        }

        private void ApplyMultipliers(HashSet<Unit> targets, bool isRemove)
        {
            foreach (Unit target in targets)
            {
                if (target == null || target.IsDead) continue;
                
                UnitStatCompo statCompo = target.GetCompo<UnitStatCompo>();

                foreach (StatAdder multiplier in _seaHorseData.statMultipliers)
                {
                    Stat stat = statCompo.GetStat(multiplier.statType);

                    if (isRemove)
                        stat.RemoveModifier(_buffKey);
                    else
                        stat.AddModifier(_buffKey, multiplier.value);
                }
            }
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            
            if(UnitData == null) return;

            SeaHorseDataSO seaHorseData = UnitData as SeaHorseDataSO;
            
            if(seaHorseData == null) return;
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, seaHorseData.buffRange);
        }
    }
}