using Code.Units.Movements;
using Code.Util;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Units.UnitAnimals.MiniBears
{
    public class MiniBear : FriendlyUnit
    {
        public bool IsInit { get; private set; }

        private NavMeshAgent _navMeshAgent;
        private Tween _initTween;

        protected override void Awake()
        {
            base.Awake();

            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void Initialize(Vector3 startPos, Vector3 endPos, Quaternion rotation, Color color, float initDuration)
        {
            transform.SetPositionAndRotation(startPos, rotation);
            _navMeshAgent.enabled = true;

            ColorMaskComponent colorMaskCompo = GetCompo<ColorMaskComponent>();
            colorMaskCompo.SetColor(color);

            _initTween?.Complete();
            
            IsInit = false;
            
            MoveTo(endPos, initDuration);

            SetUpUnit();
        }

        private async void MoveTo(Vector3 endPos, float initDuration)
        {
            NavMovement movement = GetCompo<NavMovement>();
            movement.SetDestination(endPos);

            await Awaitable.WaitForSecondsAsync(initDuration);

            IsInit = true;
        }
    }
}