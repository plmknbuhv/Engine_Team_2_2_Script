using DG.Tweening;
using UnityEngine;

namespace Code.Units.UnitAnimals.Monkeys
{
    public class Monkey : FriendlyUnit
    { 
        [field:SerializeField] public Transform MuzzleTrm { get; private set; }
        [field:SerializeField] public Vector3 RidePos { get; private set; }
        [field:SerializeField] public Vector3 RideRot { get; private set; }

        public Vector3 rideStartPos;
        
        public bool IsSpinning => GetCurrentState() is MonkeySpinState;

        public void ComeDown()
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            transform.DOMove(rideStartPos, 0.2f);
            
            ChangeState("IDLE");
        }
    }
}
