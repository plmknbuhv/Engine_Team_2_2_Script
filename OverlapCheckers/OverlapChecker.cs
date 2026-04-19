using UnityEngine;
using UnityEngine.Events;

namespace Code.OverlapCheckers
{
    public class OverlapChecker : MonoBehaviour
    {
        public UnityEvent OnTargetEnterEvent;
        
        [SerializeField] protected Transform checkPoint;
        [SerializeField] protected LayerMask targetMask;
        [SerializeField] protected int maxResultCount;
        
        protected Collider[] Results;
        private void Awake()
        {
            Results = new Collider[maxResultCount];
        }
    }
}