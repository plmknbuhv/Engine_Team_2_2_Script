using EventSystem;
using UnityEngine;

namespace Cannons.Parabolas
{
    public class ParabolaCalculator : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO parabolaChannel;
        [SerializeField, Range(10, 100)] private int pointCount = 50;

        [SerializeField] private float shootAngle = 45f;

        [Header("Check")] [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float checkOffsetHeight = 0.5f;
        [SerializeField] private float checkDistance = 1f;

        private Vector3[] _points;

        private void Awake()
        {
            _points = new Vector3[pointCount];

            parabolaChannel.AddListener<RequestMakeParabolaEvent>(HandleMakeParabola);
        }

        private void OnDestroy()
        {
            parabolaChannel.RemoveListener<RequestMakeParabolaEvent>(HandleMakeParabola);
        }

        public Vector3[] GetParabolaPoints(Vector3 startPos, Vector3 endPos, out bool canEnd)
        {
            //포물선을 그릴수 있는지 확인
            var checkPos = endPos;
            checkPos.y += checkOffsetHeight;

            Vector3 checkDir = (startPos - checkPos);
            checkDir.y = 0;
            checkDir.Normalize();

            canEnd = !Physics.Raycast(checkPos, checkDir, out var hitInfo, checkDistance,
                groundLayer);

            // 1. 변위 계산
            Vector3 displacement = endPos - startPos;

            // 수평,수직 변위 분리
            Vector3 displacementHorizontal = new Vector3(displacement.x, 0, displacement.z);
            float horizontalDistance = displacementHorizontal.magnitude;
            float verticalDisplacement = displacement.y;

            // 2. 물리 변수 준비
            float gravityY = Physics.gravity.y;
            float radian = shootAngle * Mathf.Deg2Rad;
            float tanAngle = Mathf.Tan(radian);

            float timeSqrTerm = (verticalDisplacement - horizontalDistance * tanAngle) / (0.5f * gravityY);

            //예외 처리 timeSqrTerm가 0보다 작으면 endPos에 도달할 수 없다
            if (timeSqrTerm < 0f)
            {
                Debug.LogWarning($"{shootAngle}도에서 {endPos}는 도착 할 수 없는 위치입니다.");
                canEnd = false;
                return _points;
            }

            float timeOfFlight = Mathf.Sqrt(timeSqrTerm);

            // [예외 처리] 시간이 유효하지 않은 경우
            if (float.IsNaN(timeOfFlight) || float.IsInfinity(timeOfFlight))
            {
                Debug.LogWarning("[Parabola] Failed to calculate a valid time of flight.");
                canEnd = false;
                return _points;
            }

            //간격 계산
            float timeStep = timeOfFlight / (pointCount - 1);

            //초기 속도 계산
            Vector3 initVelocity = (displacement / timeOfFlight) - (Physics.gravity * (0.5f * timeOfFlight));

            for (int i = 0; i < pointCount; i++)
            {
                float t = i * timeStep;

                // 위치 계산
                Vector3 point = startPos + initVelocity * t + Physics.gravity * (0.5f * t * t);

                _points[i] = point;
            }

            _points[pointCount - 1] = endPos;

            return _points;
        }

        private void HandleMakeParabola(RequestMakeParabolaEvent evt)
        {
            GetParabolaPoints(evt.startPos, evt.endPos, out var canEnd);

            var changeEvt = ParabolaEvents.ParabolaChangeEvent.Initializer(_points, canEnd);
            parabolaChannel.RaiseEvent(changeEvt);
        }
    }
}