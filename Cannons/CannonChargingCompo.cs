using Ami.BroAudio;
using Cannons.StatSystem;
using EventSystem;
using GondrLib.Dependencies;
using Settings.InputSystem;
using UnityEngine;

namespace Cannons
{
    public class CannonChargingCompo : MonoBehaviour
    {
        [Header("Events")] [SerializeField] private GameEventChannelSO cannonChannel;
        [SerializeField] private GameEventChannelSO parabolaChannel;
        [SerializeField] private GameEventChannelSO levelChannel;
        [SerializeField] private GameEventChannelSO soundChannel;

        [Header("Datas")] [SerializeField] private PlayerInputSO inputData;
        [SerializeField] private CannonDataSO cannonData;

        [Header("Charging")] [SerializeField] private float addChargingPower;
        [SerializeField] private float minChargingSize;
        [SerializeField] private float maxChargingSize;
        [SerializeField] private float clampMinYPos;
        [SerializeField] private LayerMask groundLayer;

        [Header("Sounds")] [SerializeField] private SoundID chargingSoundData;
        [SerializeField] private string chargingSoundKey = "CannonCharging";

        [Inject] private CannonInfoCompo _infoCompo;
        [Inject] private CannonStatCompo _statCompo;

        private float _chargingPowerValue;

        public float ChargingPower
        {
            get => _chargingPowerValue;
            set
            {
                float temp = Mathf.Clamp(value, minChargingSize, maxChargingSize);
                if (Mathf.Approximately(_chargingPowerValue, temp)) return;
                
                _chargingPowerValue = temp;

                if (maxChargingSize > _chargingPowerValue) return;
                
                var chargingSoundStopEvt = SoundEvents.StopLongSFXSoundEvent.Initializer(chargingSoundKey);
                soundChannel.RaiseEvent(chargingSoundStopEvt);
            }
        }

        private float _currentCoolTime;
        private bool _canCharging;
        private bool _canShoot;
        private float _groundCheckDistance;

        //shoot setting
        private Vector3[] _parabolaPoints;
        private Vector3 _shootDirection;

        private void Awake()
        {
            //초기화
            inputData.OnShootReleased += HandleShootReleased;
            inputData.OnShootPressed += HandleChargingStart;

            parabolaChannel.AddListener<ParabolaChangeEvent>(HandleParabolaChanged);
            levelChannel.AddListener<ChangeLevelEvent>(HandleCancelDueToLevelUp);
        }

        private void Start()
        {
            _groundCheckDistance = Camera.main.farClipPlane;
        }

        private void OnDestroy()
        {
            inputData.OnShootPressed -= HandleChargingStart;
            inputData.OnShootReleased -= HandleShootReleased;

            parabolaChannel.RemoveListener<ParabolaChangeEvent>(HandleParabolaChanged);
            levelChannel.RemoveListener<ChangeLevelEvent>(HandleCancelDueToLevelUp);
        }

        private void Update()
        {
            CoolTimeTick();
            AddChargingPower();
        }

        private void HandleChargingStart()
        {
            if (!_canShoot) return;

            _canCharging = true;

            _shootDirection = inputData.GetMouseWorldPosition() - transform.position;
            _shootDirection.y = 0;
            _shootDirection = _shootDirection.normalized;

            var chargingSoundEvt = SoundEvents.PlayLongSFXSoundEvent.Initializer(chargingSoundData, chargingSoundKey);
            soundChannel.RaiseEvent(chargingSoundEvt);

            var chargingEvt = CannonEvents.CannonChargingEvent.Initializer(_canCharging);
            cannonChannel.RaiseEvent(chargingEvt);
        }

        private void HandleShootReleased()
        {
            if (!_canCharging) return;

            if (_canShoot)
            {
                var shootEvt = ParabolaEvents.ShootParabolaEvent.Initializer(_parabolaPoints);
                parabolaChannel.RaiseEvent(shootEvt);
            }

            CancelCharging();
        }

        private void RequestMakeParabola()
        {
            Vector3 startPos = _infoCompo.GetPart("MUZZLE").position;
            Vector3 endPos = _shootDirection * ChargingPower;

            //땅에 도착 위치 확인
            endPos.y = _infoCompo.GetPart("CANNON").position.y + 1;

            Physics.Raycast(endPos, Vector3.down, out var hitInfo, _groundCheckDistance, groundLayer);
            endPos.y = Mathf.Clamp(hitInfo.point.y, clampMinYPos, _groundCheckDistance);

            var evt = ParabolaEvents.RequestMakeParabolaEvent.Initializer(startPos, endPos);
            parabolaChannel.RaiseEvent(evt);
        }

        private void CancelCharging()
        {
            _canShoot = false;
            _canCharging = false;

            ChargingPower = 0;
            _currentCoolTime = 0;

            var chargingSoundStopEvt = SoundEvents.StopLongSFXSoundEvent.Initializer(chargingSoundKey);
            soundChannel.RaiseEvent(chargingSoundStopEvt);
            
            var evt = CannonEvents.CannonChargingEvent.Initializer(_canCharging);
            cannonChannel.RaiseEvent(evt);
        }

        private void CoolTimeTick()
        {
            if (!_statCompo) return;

            _currentCoolTime += Time.deltaTime;

            if (cannonData.coolTime <= _currentCoolTime)
            {
                _canShoot = true;
            }
        }

        private void AddChargingPower()
        {
            if (!_canCharging) return;

            ChargingPower += addChargingPower * Time.deltaTime;
            RequestMakeParabola();
        }

        private void HandleParabolaChanged(ParabolaChangeEvent evt)
        {
            _parabolaPoints = evt.points;
            _canShoot = evt.canEnd;
        }

        private void HandleCancelDueToLevelUp(ChangeLevelEvent evt)
        {
            CancelCharging();
        }


#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(Vector3.zero, maxChargingSize);
        }

#endif
    }
}