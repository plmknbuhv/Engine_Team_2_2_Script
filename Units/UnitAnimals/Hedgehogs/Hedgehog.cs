using Code.Entities;
using UnityEngine;

namespace Code.Units.UnitAnimals.Hedgehogs
{
    public class Hedgehog : FriendlyUnit
    {
        private HedgehogDataSO _hedgehogData;
        private EntityVFX _entityVFX;

        protected override void Awake()
        {
            base.Awake();
            _entityVFX = GetCompo<EntityVFX>();
            _hedgehogData = UnitData as HedgehogDataSO;
        }
        
        public override void ApplyDamage(int damage, Entity dealer)
        {
            base.ApplyDamage(damage, dealer);
            // 적 유닛중에 반사하는 유닛이 없다는 가정 하에 이렇게 만듬
            
            Unit dealerUnit = dealer as Unit;
            if (dealerUnit == null) return;
            
            _entityVFX.PlayVFX("ReflectEffect", transform.position, Quaternion.identity);
            int reflectDamage = Mathf.RoundToInt(damage * _hedgehogData.reflectCoefficient);
            dealerUnit.ApplyDamage(reflectDamage, this);
        }
    }
}
