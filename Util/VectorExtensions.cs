using UnityEngine;

namespace Code.Util
{
    public static class VectorExtensions
    {
        public static bool Approximately(this Vector3 a, Vector3 b)
        {
            return Mathf.Approximately(a.x, b.x) &&
                   Mathf.Approximately(a.y, b.y) &&
                   Mathf.Approximately(a.z, b.z);
        }
        
        public static Vector3 RemoveY(this Vector3 a) => new Vector3(a.x, 0, a.z);
    }
}