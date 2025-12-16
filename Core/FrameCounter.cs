using UnityEngine;

namespace Code.Core
{
    public class FrameCounter : MonoBehaviour
    {
        [SerializeField, Range(30, 144)] private int frame;

        private void Awake()
        {
            Application.targetFrameRate = frame;
        }
    }
}