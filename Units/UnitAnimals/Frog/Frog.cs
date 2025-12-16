using Code.Units.UnitStatusEffects;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Units.UnitAnimals.Frog
{
    public class Frog : EnemyUnit
    {
        public UnityEvent OnSkillEvent;
        
        [field:SerializeField] public Rigidbody RigidCompo { get; private set; }
        
        private FrogDataSO _frogData;
        
        private float _currentTime;
        private bool _isUseSkill;
        
        protected override void Awake()
        {
            base.Awake();
            _frogData = UnitData as FrogDataSO;
            Debug.Assert(_frogData != null, $"FrogData data is null : {UnitData}");
        }

        protected override void Update()
        {
            base.Update();
            
            if (IsInBattle == false && _isUseSkill == false && IsDead == false
                && _statusEffect.IsDefaultStatus
                && _followLine.NormalizedTime < 0.7f)
            {
                _currentTime += Time.deltaTime;

                if (_frogData.skillCoolTime <= _currentTime)
                {
                    _isUseSkill = true;
                    ChangeState("SKILL_ATTACK");
                    OnSkillEvent?.Invoke();
                }
            }
            else if(IsInBattle == false)
            {
                ResetTimer();
            }
        }
        
        public override void ApplyStatusEffect(StatusEffectType statusEffectType, float duration = 0)
        {
            if(_isUseSkill) return;
            
            _statusEffect.ApplyStatusEffect(statusEffectType, duration);
            
            if(statusEffectType == StatusEffectType.DEFAULT) return;
            
            ChangeState(statusEffectType.ToString());
        }

        public void ResetTimer()
        {
            _currentTime = 0;
            _isUseSkill = false;
        }
    }
}