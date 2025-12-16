using Code.Effects;
using Code.Entities;
using UnityEngine;
using Work.Common.Core.GondrLib.ObjectPool.RunTime;

namespace Code.Units.UnitAnimals.SnappingTurtle
{
    public class SnappingTurtle : EnemyUnit
    {
        private SnappingTurtleDataSo _turtleData;
        private float _currentTime;
        private bool _isShield;
        
        protected override void Awake()
        {
            base.Awake();
            _turtleData = UnitData as SnappingTurtleDataSo;
        }

        private void LateUpdate()
        {
            _currentTime += Time.deltaTime;
            
            if (_turtleData.duration <= _currentTime && _isShield)
            {
                _isShield = false;
                _currentTime = 0;
                return;
            }

            if (_turtleData.creationDelay <= _currentTime && _isShield == false)
            {
                _isShield = true;
                PlayEffect();
                _currentTime = 0;
            }
        }
        
        private void PlayEffect()
        {
            PoolingEffect skillEffect = PoolManagerMono.Instance.Pop<PoolingEffect>(_turtleData.skillEffect);
            skillEffect.transform.SetParent(transform);
            skillEffect.PlayVFX(transform.position, Quaternion.identity);
        }

        public override void ApplyDamage(int damage, Entity dealer)
        {
            if(_isShield) return;
            base.ApplyDamage(damage, dealer);
        }
    }
}