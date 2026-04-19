using UnityEngine;

namespace Code.Core.Destination
{
    [CreateAssetMenu(fileName = "DestinationFinder", menuName = "SO/DestinationFinder", order = 0)]
    public class DestinationFinderSO : ScriptableObject
    {
        public Transform Destination { get; private set; }

        public void SetTarget(Transform destination)
        {
            Destination = destination;
        }
    }
}