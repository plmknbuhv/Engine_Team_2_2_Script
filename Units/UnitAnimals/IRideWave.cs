using UnityEngine;

namespace Code.Units.UnitAnimals
{
    public interface IRideWave
    {
        public bool CanRide { get; }
        public void StartRideWave(Transform waveTrm, Quaternion waveRot);
        public void UpdateRideWave(Transform wavePos, Quaternion waveRot);
        public void StopRideWave();
    }
}