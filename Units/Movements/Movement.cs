using Code.Entities;
using Code.Units.UnitStat;
using UnityEngine;

namespace Code.Units.Movements
{
    public abstract class Movement : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        protected Unit _unit;
        protected Stat _moveSpeedStat;
        protected float _baseSpeed;
        
        public virtual void Initialize(Entity entity)
        {
            _unit = entity as Unit;
            Debug.Assert(_unit != null, "이거 엔티티가 유닛이 아님");
            Debug.Assert(_unit.UnitData != null, "유닛에 UnitDataSo가 없습니다.");
            
            _baseSpeed = _unit.UnitData.moveSpeed;
        }

        public virtual void AfterInitialize()
        {
            _moveSpeedStat = _unit.GetCompo<UnitStatCompo>().GetStat(UnitStatType.MoveSpeed);
            _moveSpeedStat.OnValueChanged += HandleChangeMoveStat;
        }

        protected abstract void HandleChangeMoveStat(Stat stat, float currentValue, float prevValue);

        public virtual void LookAtTarget(Vector3 target, bool isSmooth = true)
        {
            Vector3 direction = (target - transform.position).normalized;
            direction.y = 0;
            
            if (isSmooth)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.LookRotation(direction), Time.deltaTime * 8f);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
        
        private void OnDestroy()
        {
            _moveSpeedStat.OnValueChanged -= HandleChangeMoveStat;
        }
        
        public abstract void Knockback(Vector3 direction, float force, Entity dealer);
    }
}