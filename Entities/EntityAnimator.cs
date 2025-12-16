using UnityEngine;
using UnityEngine.Events;

namespace Code.Entities
{
    public class EntityAnimator : MonoBehaviour, IEntityComponent
    {
        protected Animator _animator;
        public UnityEvent<Vector3, Quaternion> OnAnimatorMoveEvent;
        
        private Entity _entity;

        public bool ApplyRootMotion
        {
            get => _animator.applyRootMotion;
            set => _animator.applyRootMotion = value;
        }

        public void Initialize(Entity entity)
        {
            _animator = GetComponent<Animator>();
            _entity = entity;
        }

        public void SetParam(int hash, float value, float dampTime) 
            => _animator.SetFloat(hash, value, dampTime, Time.deltaTime);
        
        public void SetParam(int hash, float value) => _animator.SetFloat(hash, value);
        public void SetParam(int hash, int value) => _animator.SetInteger(hash, value);
        public void SetParam(int hash, bool value) => _animator.SetBool(hash, value);
        public void SetParam(int hash) => _animator.SetTrigger(hash);

        public void StartPlayAnimator() => _animator.StopPlayback();
        public void StopPlayAnimator() =>  _animator.StartPlayback();

        public void OffAnimator()
        {
            _animator.enabled = false;
        }

        private void OnAnimatorMove()
        {
            OnAnimatorMoveEvent?.Invoke(_animator.deltaPosition, _animator.deltaRotation);
        }
    }
}