using System.Collections;
using Cannons.Slots;
using Cannons.StatSystem;
using Code.Units;
using DG.Tweening;
using EventSystem;
using GondrLib.Dependencies;
using Settings.InputSystem;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Cannons
{
    public class CannonShooter : MonoBehaviour
    {
        public UnityEvent OnShootEvent;

        #region Serialize variable

        [SerializeField] private GameEventChannelSO cannonChannel;
        [SerializeField] private GameEventChannelSO parabolaChannel;
        [SerializeField] private GameEventChannelSO soundChannel;
        [SerializeField] private PlayerInputSO inputData;
        [SerializeField] private CannonDataSO cannonData;

        #endregion
        
        [Inject] private CannonSlot _slot;
        [Inject] private CannonInfoCompo _infoCompo;
        [Inject] private CannonStatCompo _statCompo;

        private Sequence _shootSeq;

        private Vector3 _bodyLocalOriginPos;

        private void Awake()
        {
            //초기화
            parabolaChannel.AddListener<ShootParabolaEvent>(HandleShootEvent);
        }

        private void Start()
        {
            _bodyLocalOriginPos = _infoCompo.GetPart("BODY").transform.localPosition;
        }

        private void OnDestroy()
        {
            parabolaChannel.RemoveListener<ShootParabolaEvent>(HandleShootEvent);
        }
        
        private void HandleShootEvent(ShootParabolaEvent evt)
        {
            Shoot(evt.points);
        }

        private void Shoot(Vector3[] points)
        {
            var unit = _slot.SpawnUnit();
            if (!unit) return;

            //소환 유닛 위치 설정
            unit.transform.position = points[0];

            ShootSequenceSession(unit, points, _statCompo.GetStatValue(CannonStatType.SHOOT_POWER));

            //발사 이벤트
            OnShootEvent?.Invoke();
            var evt = CannonEvents.CannonShootEvent.Initializer(unit);
            cannonChannel.RaiseEvent(evt);
        }

        private Sequence ShootSequenceSession(Unit target, Vector3[] points, float speed)
        {
            _shootSeq = DOTween.Sequence();
            points = (Vector3[])points.Clone();
            var body = _infoCompo.GetPart("BODY");
            
            _shootSeq.Append(body.DOLocalMove(-(Vector3.up * 0.3f) + body.localPosition, 0.1f)
                .SetEase(Ease.InBack));
            _shootSeq.AppendCallback(() => StartCoroutine(ShootCoroutine(target, points, speed)));
            _shootSeq.Append(body.DOLocalMove((Vector3.up * 0.3f) + body.localPosition, 0.1f)
                .SetEase(Ease.InBack));
            _shootSeq.AppendCallback(() => body.transform.localPosition = _bodyLocalOriginPos);
            
            return _shootSeq;
        }

        private IEnumerator ShootCoroutine(Unit target, Vector3[] points, float speed)
        {
            int currentIndex = 0;

            target.ChangeState("FLYING");

            float totalDistance = 0f;
            for (int i = 1; i < points.Length; i++)
                totalDistance += Vector3.Distance(points[i - 1], points[i]);

            float currentDistance = 0f;
            
            //날아가는 부분
            while (currentIndex < points.Length)
            {
                float time = (float)currentIndex / (float)points.Length;
                float multiplierForce = cannonData.shootSpeedCurve.Evaluate(time) + 0.1f;

                Vector3 prevPos = target.transform.position;
                target.transform.position =
                    Vector3.MoveTowards(target.transform.position, points[currentIndex],
                        Time.deltaTime * speed * multiplierForce);

                currentDistance += Vector3.Distance(prevPos, target.transform.position);
                target.SetFlyProgress(currentDistance / totalDistance);
                
                if (ApproximatelyVector(target.transform.position, points[currentIndex])) currentIndex++;
                yield return null;
            }

            //유닛 초기 설정
            target.SetUpUnit();
            target.SetFlyProgress(1f);
            
            //유닛 랜덤 위치 바라보기
            var lookAtPos = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
            target.transform.rotation = lookAtPos;
            
            soundChannel.RaiseEvent(SoundEvents.PlaySFXSoundEvent.Initializer(target.UnitData.signatureSoundData));

            //폭팔 이벤트
            cannonChannel.RaiseEvent(CannonEvents.CannonShootExplosionEvent.Initializer(points[^1], target.UnitData.sizeType));
        }

        private bool ApproximatelyVector(Vector2 from, Vector2 to)
        {
            return Mathf.Approximately(from.x, to.x) && Mathf.Approximately(from.y, to.y);
        }
    }
}