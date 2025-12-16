using System;
using Code.Entities;
using UnityEngine;
using UnityEngine.Events;

namespace Code.OverlapCheckers
{
    public enum CastType
    {
        Ray,
        Box,
        Sphere,
    }
    
    public class CastChecker : MonoBehaviour, IEntityComponent
    {
        public UnityEvent OnTargetEnterEvent;
        
        [SerializeField] private Transform checkPoint;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private float castLength;
        [SerializeField] private int maxCount;
        [Space]
        [SerializeField] private Vector3 boxSize;
        [SerializeField] private float radius;
        [Space]
        [SerializeField] private bool showBoxCast;
        [SerializeField] private bool showSphereCast;
        
        private RaycastHit[] _results;
        
        public void Initialize(Entity entity)
        {
            _results = new RaycastHit[maxCount];
        }
        
        public bool CastCheck(CastType castType = CastType.Ray)
        {
            bool isOverlap;

            switch (castType)
            {
                case CastType.Box:
                    isOverlap = Physics.BoxCast(checkPoint.position, boxSize * 0.5f, transform.forward, Quaternion.identity, targetMask);
                    break; 
                case CastType.Sphere:
                    Ray ray = new Ray(checkPoint.position, transform.forward);  
                    isOverlap = Physics.SphereCast(ray, radius,castLength, targetMask);
                    break;
                default: //CastType.Ray
                    isOverlap = Physics.Raycast(checkPoint.position, transform.forward, castLength, targetMask);
                    break;
            }

            if (isOverlap)
            {
                OnTargetEnterEvent.Invoke();
            }
            
            return isOverlap;
        }
        
        public GameObject[] GetCastData(CastType castType = CastType.Ray)
        {
            int count = 0;
            
            switch (castType)
            {
                case CastType.Box:
                    count = Physics.BoxCastNonAlloc(checkPoint.position, boxSize * 0.5f,
                        transform.forward, _results, Quaternion.identity, castLength, targetMask);
                    break;
                case CastType.Sphere:
                    count = Physics.SphereCastNonAlloc(checkPoint.position, radius,
                        transform.forward, _results, castLength, targetMask);
                    break;
                default: //CastType.Ray
                    count = Physics.RaycastNonAlloc(checkPoint.position, transform.forward, _results, castLength, targetMask);
                    break;
            }

            GameObject[] targets = new GameObject[count];

            for (int i = 0; i < count; i++)
            {
                targets[i] = _results[i].collider.gameObject;
            }

            if (count > 0)
            {
                Array.Sort(targets, (a, b) =>
                    Vector3.Distance(checkPoint.position, a.transform.position)
                        .CompareTo(Vector3.Distance(transform.position, b.transform.position)));
            }

            return targets;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(checkPoint == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(checkPoint.position, transform.forward * castLength);

            if (showBoxCast)
            {
                Gizmos.color = Color.yellow;
                Gizmos.matrix = checkPoint.localToWorldMatrix;
                Gizmos.DrawWireCube(Vector3.forward * castLength, boxSize);
            }
            
            if (showSphereCast)
            {
                Gizmos.color = Color.yellow;
                Gizmos.matrix = checkPoint.localToWorldMatrix;
                Gizmos.DrawWireSphere(Vector3.forward * castLength, radius);
            }
        }
#endif
    }
}