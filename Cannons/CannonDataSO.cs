using UnityEngine;

namespace Cannons
{
    [CreateAssetMenu(fileName = "CannonData", menuName = "SO/Cannon/Data", order = 0)]
    public class CannonDataSO : ScriptableObject
    {
        public float coolTime;
        [Space] public AnimationCurve shootSpeedCurve;
    }
}