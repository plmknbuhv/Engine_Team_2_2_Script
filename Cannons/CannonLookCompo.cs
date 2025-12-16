using GondrLib.Dependencies;
using UnityEngine;

namespace Cannons
{
    public class CannonLookCompo : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 5f;
        [Inject] private CannonInfoCompo _infoCompo;
        
        public void LookAtShootPoint(Vector3 pointPos)
        {
            var bodyTrm = _infoCompo.GetPart("BODY");
            
            Vector3 dir = pointPos - bodyTrm.position;
            var targetLookAt = Quaternion.LookRotation(dir);

            Quaternion targetLocalRotation = Quaternion.Inverse(transform.rotation) * targetLookAt;
            Vector3 localEuler = targetLocalRotation.eulerAngles;
            Quaternion rot = Quaternion.Euler(localEuler.x, 0f, 0f);
            
            //포물선 바라보기
            bodyTrm.localRotation = rot;
        }

        public void BodyLookAtTarget(Vector3 lookAtPos)
        {
            var visualTrm = _infoCompo.GetPart("VISUAL");
            
            Vector3 lookAtPoint = lookAtPos - visualTrm.position;
            
            lookAtPoint.y = 0;
            Quaternion lookAtTarget = Quaternion.LookRotation(lookAtPoint);
            
            //캐논의 방향
            visualTrm.rotation = Quaternion.Slerp(visualTrm.rotation, lookAtTarget, Time.deltaTime * rotationSpeed);
        }
    }
}