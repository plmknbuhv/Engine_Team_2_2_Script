using UnityEngine;

namespace Code.OverlapCheckers
{
    public class SphereOverlapChecker : OverlapChecker
    {
        [SerializeField] private float radius;

        public bool ShereOverlapCheck()
        {
            bool isOverlap = Physics.CheckSphere(checkPoint.position,
                radius, targetMask);
            
            if (isOverlap)
            {
                OnTargetEnterEvent.Invoke();
            }
            
            return isOverlap;
        }

        public GameObject[] GetOverlapData()
        {
            for (int i = 0; i < Results.Length; i++)
                Results[i] = null;
            
            int count = Physics.OverlapSphereNonAlloc(checkPoint.position, radius,
                Results, targetMask);
            
            GameObject[] targets = new GameObject[count];

            for (int i = 0; i < count; i++)
            {
                targets[i] = Results[i].gameObject;
            }

            return targets;
        }
        
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(checkPoint == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(checkPoint.position, radius);
        }
        #endif
    }
}