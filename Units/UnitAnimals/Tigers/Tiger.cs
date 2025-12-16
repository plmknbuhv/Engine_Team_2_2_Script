using Code.Units.State.Friendly;
using Code.Units.UnitAnimals.Pigs;
using Code.Units.UnitStat;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Units.UnitAnimals.Tigers
{
    public class Tiger : FriendlyUnit
    {
        public UnityEvent OnEat;
        
        private UnitStatCompo _unitStatCompo;
        private TigerDataSO _tigerData;
        private UnitDetector _detector;
        private float _originalSize;
        private int _maxEatCount;
        
        public int GlowCount { get; private set; }
        public bool CanEat { get; set; } = true;

        public int MaxEatCount
        {
            get => _maxEatCount;
            set => _maxEatCount = Mathf.Clamp(value, _tigerData.defaultGrowCount, _tigerData.maxGlowCount);
        }
        public Pig EatingTarget { get; set; }

        protected override void Awake()
        {
            base.Awake();

            _originalSize = transform.localScale.x;
            _unitStatCompo = GetCompo<UnitStatCompo>();
            _detector = GetCompo<UnitDetector>();
            _tigerData = UnitData as TigerDataSO;
            MaxEatCount = _tigerData!.defaultGrowCount;
            
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

            CanEat = true;
        }

        public override void ResetItem()
        {
            base.ResetItem();

            Reset();
            CanEat = false;
        }

        private void HandleDead() => CanEat = false;

        public void GlowStat()
        {
            GlowCount++;
            float glowSize = transform.localScale.x + _tigerData.glowSize;
            transform.localScale = Vector3.one * glowSize;  
            
            _unitStatCompo.GetStat(UnitStatType.Damage).AddModifier(this, _tigerData.glowDamage * GlowCount, true);
            Debug.Log("성장");
        }

        protected override void FixedUpdate()
        {
            if (GlowCount < MaxEatCount &&
                _detector.TryGetClosestEnemy<Pig>(out var pig, _tigerData.eatingRange, null, UnitTeamType.Friendly))
            {
                if (pig.GetCurrentState() is UnitFlyingState) return;
                if (pig.IsEaten || CanEat == false || GetCurrentState() is FriendlyAttackState) return;

                if (TargetUnit != null && TargetUnit is EnemyUnit enemy)
                {
                    enemy.TargetUnitList.Remove(this);
                    enemy.TargetUnit = null;
                }
                
                pig.IsEaten = true;
                EatingTarget = pig;
                ChangeState("EAT");
            }
            
            base.FixedUpdate();
        }

        private void Reset()
        {
            GlowCount = 0;
            transform.localScale = Vector3.one * _originalSize;
            _unitStatCompo.GetStat(UnitStatType.Damage).RemoveModifier(this);
        }
    }
}