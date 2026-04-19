using UnityEngine;

namespace Code.Util
{
    public class TimerCondition : UnitSkillCondition
    {
        private float _lastTime;
        private float _delayTime;
        private bool _isActiveTimer;
        
        public override bool CanUseSkill { get; protected set; }

        public void StartTimer(float delayTime)
        {
            _delayTime = delayTime;
            _lastTime = Time.time;
            CanUseSkill = false;
            _isActiveTimer = true;
        }

        private void Update()
        {
            if (_isActiveTimer == false) return;

            if (Time.time >= _lastTime + _delayTime)
            {
                CanUseSkill = true;
                _isActiveTimer = false;
            }
        }

        public override void ResetCondition()
        {
            _isActiveTimer = false;
            CanUseSkill = false;
        }
    }
}