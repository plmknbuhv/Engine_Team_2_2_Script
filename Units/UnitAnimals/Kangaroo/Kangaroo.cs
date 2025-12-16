using Code.Units.UnitStatusEffects;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Units.UnitAnimals.Kangaroo
{
    public class Kangaroo : EnemyUnit
    {
        public UnityEvent OnSkillEvent;
        
        [field:SerializeField] public Rigidbody RigidCompo { get; private set; }
        
        private KangarooDataSO _kangarooData;
        
        private bool _isUseSkill;

        protected override void Awake()
        {
            base.Awake();
            _kangarooData = UnitData as KangarooDataSO;
            Debug.Assert(_kangarooData != null, $"KangarooData data is null : {UnitData}");
        }

        protected override void Update()
        {
            base.Update();
            
            if (_isUseSkill == false && IsDead == false
                && _statusEffect.IsDefaultStatus
                && _followLine.NormalizedTime < 0.5f)
            {
                if (_followLine.NormalizedTime > 0.2f)
                {
                    _isUseSkill = true;
                    ChangeState("SKILL_ATTACK");
                    OnSkillEvent?.Invoke();
                }
            }
        }
                
        public override void ApplyStatusEffect(StatusEffectType statusEffectType, float duration = 0)
        {
            if(_isUseSkill) return;
            
            _statusEffect.ApplyStatusEffect(statusEffectType, duration);
            
            if(statusEffectType == StatusEffectType.DEFAULT) return;
            
            ChangeState(statusEffectType.ToString());
        }
    }
}