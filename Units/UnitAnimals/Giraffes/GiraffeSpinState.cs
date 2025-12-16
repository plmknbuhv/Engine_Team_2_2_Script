using System.Collections.Generic;
using Code.Entities;
using Code.Units.State;
using Code.Util;
using UnityEngine;

namespace Code.Units.UnitAnimals.Giraffes
{
    public class GiraffeSpinState : UnitState
    {
        private Giraffe _giraffe;
        private GiraffeDataSO _giraffeData;
        private UnitDetector _detector;
        private TimerCondition _timerCondition;
        private float _startSpinTime;
        private const string spinEffect = "GiraffeSpinEffect";

        public GiraffeSpinState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _giraffe = _unit as Giraffe;
            _detector = _unit.GetCompo<UnitDetector>();
            _giraffeData = _unit.UnitData as GiraffeDataSO;
            _timerCondition = _unit.GetCompo<TimerCondition>();
        }

        public override void Enter()
        {
            base.Enter();

            _startSpinTime = Time.time;
            _giraffe.EntityVFX.PlayVFX(spinEffect, _unit.transform.position, Quaternion.identity);
            _timerCondition.StartTimer(_giraffeData.spinAttackDelay);
        }

        public override void Update()
        {
            base.Update();

            if (Time.time >= _startSpinTime + _giraffeData.spinDuration)
            {
                _giraffe.GetOffMonkey();
                _unit.ChangeState("IDLE");
                return;
            }

            SpinAttack();
        }

        private void SpinAttack()
        {
            if (_timerCondition.CanUseSkill == false) return;

            _timerCondition.StartTimer(_giraffeData.spinAttackDelay);

            if (_detector.TryGetUnits(out HashSet<Unit> units, _giraffeData.spinRange, UnitTeamType.Enemy))
            {
                foreach (var unit in units)
                    unit.ApplyDamage(_giraffeData.spinAttackDamage, _giraffe);
            }
        }

        public override void Exit()
        {
            base.Exit();

            _giraffe.EntityVFX.StopVFX(spinEffect);
            _timerCondition.ResetCondition();
        }
    }
}