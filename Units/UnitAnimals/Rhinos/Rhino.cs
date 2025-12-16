using Code.Units.UnitStat;
using Code.Units.UnitStatusEffects;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Units.UnitAnimals.Rhinos
{
    public class Rhino : EnemyUnit
    {
        public UnityEvent OnSkillEvent;
        
        private UnitStatusEffect _unitStatusEffect;
        private UnitStatCompo _unitStatCompo;
        private RhinoDataSo _rhinoData;
        private float _currentTime;
        private bool _isUseSkill;
        
        protected override void Awake()
        {
            base.Awake();
            _unitStatusEffect = GetCompo<UnitStatusEffect>();
            _unitStatCompo = GetCompo<UnitStatCompo>();
            
            _unitStatCompo.GetStat(UnitStatType.MoveSpeed).CanApplyStat = false;
            
            _rhinoData = UnitData as RhinoDataSo;
            Debug.Assert(_rhinoData != null, $"RhinoData data is null : {UnitData}");
        }

        protected override void Update()
        {
            base.Update();
            
            if(TargetUnitList.Count == 0)
                IsInBattle = false;
            
            if (_isUseSkill == false && IsInBattle == false && IsDead == false && _unitStatusEffect.IsDefaultStatus)
            {
                _currentTime += Time.deltaTime;

                if (_rhinoData.skillCoolTime <= _currentTime)
                {
                    _isUseSkill = true;
                    ChangeState("SKILL_ATTACK");
                    OnSkillEvent?.Invoke();
                }
            }
            else
            {
                ResetTimer();
            }
        }

        public void ResetTimer()
        {
            _currentTime = 0;
            _isUseSkill = false;
        }
    }
}