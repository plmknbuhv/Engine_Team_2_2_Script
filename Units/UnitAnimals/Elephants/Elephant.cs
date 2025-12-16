using Code.Combat;
using Code.Units.UnitStat;
using Code.Units.UnitStatusEffects;
using Code.Util;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Units.UnitAnimals.Elephants
{
    public class Elephant : EnemyUnit
    {
        public UnityEvent OnSkillEvent;
        
        [SerializeField] private Material material;
        [SerializeField] [ColorUsage(false, false)] private Color colorDefault;
        [SerializeField] [ColorUsage(false, false)] private Color colorAnger;
        
        private UnitStatCompo _unitStatCompo;
        private UnitStatusEffect _unitStatusEffect;
        private ElephantDataSo _elephantData;
        private UnitHealth _unitHealth;
        private ColorMaskComponent _colorMaskCompo;
        
        public bool IsFirstUltimate { get; set; }
        
        private float _currentTime;
        
        private bool _isUseSkill;
        private bool _isUseUltimate;

        protected override void Awake()
        {
            base.Awake();
            _unitHealth = GetCompo<UnitHealth>();
            _unitStatCompo = GetCompo<UnitStatCompo>();
            _unitStatusEffect = GetCompo<UnitStatusEffect>();
            _colorMaskCompo = GetCompo<ColorMaskComponent>();
            
            _elephantData = UnitData as ElephantDataSo;
            
            Debug.Assert(_elephantData != null, $"ElephantData data is null : {UnitData}");
            
            _colorMaskCompo.SetColor(colorDefault);
        }
        
        public override void SetUpUnit()
        {
            base.SetUpUnit();
            _colorMaskCompo.SetColor(colorDefault);
            _isUseUltimate = false;
            IsFirstUltimate = false;
            
            /*if(_unitStatCompo.TryGetStat(UnitStatType.MoveSpeed, out var speedStat))
                speedStat.RemoveModifier(this);*/
            if(_unitStatCompo.TryGetStat(UnitStatType.AttackDelay, out var delayStat))
                delayStat.RemoveModifier(this);
            if(_unitStatCompo.TryGetStat(UnitStatType.Damage, out var damageStat))
                damageStat.RemoveModifier(this);
        }

        protected override void Update()
        {
            base.Update();
            
            if (_isUseUltimate == false && 50 >= (float)_unitHealth.CurrentHealth / _elephantData.health * 100)
            {
                _isUseUltimate = true;
                IsFirstUltimate = true;
                ResetTimer();
                _currentTime = _elephantData.skillCoolTime;
                _colorMaskCompo.SetColor(colorAnger);
                
                /*if(_unitStatCompo.TryGetStat(UnitStatType.MoveSpeed, out var speedStat))
                    speedStat.AddModifier(this, _elephantData.angerModeMoveSpeed);*/
                if(_unitStatCompo.TryGetStat(UnitStatType.AttackDelay, out var delayStat))
                    delayStat.AddModifier(this, _elephantData.angerModeAttackSpeed);
                if(_unitStatCompo.TryGetStat(UnitStatType.Damage, out var damageStat))
                    damageStat.AddModifier(this, _elephantData.angerModeMoveSpeed);
            }
            
            if (IsInBattle && _isUseSkill == false && IsDead == false && _unitStatusEffect.IsDefaultStatus)
            {
                _currentTime += Time.deltaTime;

                if (_elephantData.skillCoolTime <= _currentTime)
                {
                    _isUseSkill = true;
                    ChangeState("SKILL_ATTACK");
                    OnSkillEvent.Invoke();
                }
            }
            else if(IsInBattle == false)
            {
                ResetTimer();
            }
        }

        public void ResetTimer()
        {
            _currentTime = 0;
            _isUseSkill = false;
            
            if(TargetUnitList.Count == 0)
                IsInBattle = false;
        }
    }
}