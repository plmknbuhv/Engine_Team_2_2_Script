using UnityEngine;

namespace Code.Core.Destination
{
    [DefaultExecutionOrder(-1)] // 미리 위치 넣어줌
    public class DestinationManager : MonoBehaviour
    {
        [SerializeField] private Transform destination;
        [SerializeField] private DestinationFinderSO destinationFinder; // 적들이 가야 할 곳
        
        private void Awake()
        {
            destinationFinder.SetTarget(destination);
        }
    }
}