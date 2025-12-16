using Code.Entities;
using Code.Units;
using UnityEngine;

namespace Code.Util
{
    public abstract class UnitSkillCondition : MonoBehaviour, IEntityComponent
    {
        protected Unit _unit;

        public virtual void Initialize(Entity entity)
        {
            _unit = entity as Unit;
            Debug.Assert(_unit != null, $"Unit is null : {entity.gameObject.name}");
        }

        public abstract bool CanUseSkill { get; protected set; }
        public abstract void ResetCondition();
    }
}