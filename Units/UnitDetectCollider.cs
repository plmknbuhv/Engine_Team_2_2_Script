using Code.Entities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Code.Units
{
    [RequireComponent(typeof(Collider))]
    public class UnitDetectCollider : MonoBehaviour, IEntityComponent
    {
        public UnityEvent<Unit> OnCollision;
        
        [SerializeField] private UnitTeamType targetType;

        private Entity _entity;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Unit unit) && unit.TeamType == targetType)
            {
                OnCollision?.Invoke(unit);
            }
        }
    }
}