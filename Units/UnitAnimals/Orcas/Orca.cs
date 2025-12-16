using Ami.BroAudio;
using Code.Units.UnitStatusEffects;
using EventSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Units.UnitAnimals.Orcas
{
    public class Orca : EnemyUnit
    {
        public UnityEvent OnSkillEvent;

        [field: SerializeField] public GameEventChannelSO SoundChannel { get; private set; }
        [field: SerializeField] public SoundID SkillSound { get; private set; }

        private UnitStatusEffect _unitStatusEffect;
        private OrcaDataSo _orcaData;
        private float _currentTime;
        private bool _isUseSkill;
        
        protected override void Awake()
        {
            base.Awake();
            _unitStatusEffect = GetCompo<UnitStatusEffect>();
            
            _orcaData = UnitData as OrcaDataSo;
            Debug.Assert(_orcaData != null, $"OrcaData data is null : {UnitData}");
        }

        protected override void Update()
        {
            base.Update();
            
            if (_isUseSkill == false && IsDead == false && _unitStatusEffect.IsDefaultStatus)
            {
                _currentTime += Time.deltaTime;

                if (_orcaData.skillCoolTime <= _currentTime)
                {
                    _isUseSkill = true;
                    ChangeState("SKILL_ATTACK");
                    OnSkillEvent.Invoke();
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