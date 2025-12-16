using Code.Units.Movements;
using Code.Units.UnitAnimals;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Units
{
    public class WaveRideableUnit : FriendlyUnit, IRideWave
    {
        [SerializeField] private float positionDeltaMultiplier;
        public bool IsRideWave { get; private set; }
        public bool CanRide => Mathf.Approximately(FlyProgress, 1f);
        
        private int _originAvoidancePriority;
        private Transform _prevParent;

        protected override void Awake()
        {
            base.Awake();
            
            _originAvoidancePriority = _navAgent.avoidancePriority;
        }


        public virtual void StartRideWave(Transform waveTrm, Quaternion waveRot)
        {
            IsRideWave = true;

            _prevParent = transform.parent;
            transform.parent = waveTrm;
            
            _navAgent.updatePosition = false;
            _navAgent.updateRotation = false;
            
            _navAgent.avoidancePriority = 0;
            ChangeState("RIDE_WAVE");
        }

        public virtual void UpdateRideWave(Transform waveTrm, Quaternion waveRot)
        {
        }

        public virtual void StopRideWave()
        {
            IsRideWave = false;

            transform.parent = _prevParent;
            
            _navAgent.updatePosition = true;
            _navAgent.updateRotation = true;
            _navAgent.Warp(transform.position);
            
            _navAgent.avoidancePriority = _originAvoidancePriority;

            ChangeState("IDLE");
        }
    }
}