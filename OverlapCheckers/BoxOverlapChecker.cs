using UnityEngine;

namespace Code.OverlapCheckers
{
    public class BoxOverlapChecker : OverlapChecker
    {
        [SerializeField] private Vector3 boxSize;
        
        public bool BoxOverlapCheck()
        {
            bool isOverlap = Physics.CheckBox(checkPoint.position,
                boxSize * 0.5f, checkPoint.rotation, targetMask);
            
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
                
            int count = Physics.OverlapBoxNonAlloc(checkPoint.position, boxSize * 0.5f,
                Results, checkPoint.rotation, targetMask);
            
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
            Gizmos.matrix = checkPoint.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, boxSize);
        }
        #endif
    }
}