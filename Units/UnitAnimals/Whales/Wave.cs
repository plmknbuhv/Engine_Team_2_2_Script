using System.Collections;
using System.Collections.Generic;
using Code.Entities;
using Code.Units.UnitStat;
using DG.Tweening;
using Enemies;
using GondrLib.ObjectPool.RunTime;
using Level;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

namespace Code.Units.UnitAnimals.Whales
{
    public class Wave : MonoBehaviour, IPoolable
    {
        public UnityEvent OnWaveSpawn;

        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;

        [SerializeField] private FollowLine followLine;
        [SerializeField] private ParticleSystem waveParticle;
        [SerializeField] private UnitTeamType enemyType;
        [SerializeField] private UnitTeamType teamType;
        [SerializeField] private float disappearDuration;
        [SerializeField] private float disappearThreshold;

        private readonly HashSet<IRideWave> _waveRideables = new HashSet<IRideWave>();
        private Entity _dealer;
        private LevelSpline levelSpline;
        private Pool _myPool;
        private int _damage;
        private float _damageMultiplier;
        private Coroutine _waveCoroutine;
        private Tween _disappearTween;
        private float _speed;
        private bool _isDead;
        private Vector3 _originScale;

        private void Awake()
        {
            levelSpline = FindAnyObjectByType<LevelSpline>();

            followLine.Initialize(null);
            followLine.SetUpFollowLine(levelSpline.Splines[0]);
            _originScale = transform.localScale;
        }

        private void OnDestroy()
        {
            if (_waveCoroutine != null)
                StopCoroutine(_waveCoroutine);
            
            _disappearTween?.Kill();
        }

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void Initialize(Entity dealer, int damage, float waveSpeed, float damageMultiplier)
        {
            waveParticle.Play();

            _dealer = dealer;
            _damage = damage;
            _speed = waveSpeed;
            _damageMultiplier = damageMultiplier;

            followLine.SetDuration(_speed);
            followLine.PlayMove(true);

            _waveCoroutine = StartCoroutine(WaveCoroutine());

            OnWaveSpawn?.Invoke();
        }

        private void Update()
        {
            foreach (IRideWave waveRideable in _waveRideables)
            {
                waveRideable.UpdateRideWave(transform, transform.rotation);
            }
        }

        private IEnumerator WaveCoroutine()
        {
            yield return null;
            yield return new WaitUntil(() => followLine.SplineAnimate.NormalizedTime <= disappearThreshold);

            foreach (IRideWave waveRideable in _waveRideables)
            {
                waveRideable.StopRideWave();
            }
            
            _waveRideables.Clear();
            
            followLine.PauseMove();
            Disappear();
        }

        private void Disappear()
        {
            _isDead = true;
            
            _disappearTween = transform.DOScale(0, disappearDuration).OnComplete(() =>
            {
                transform.position = Vector3.zero;
                _myPool.Push(this);
            });
        }

        public void ResetItem()
        {
            _isDead = false;
            waveParticle.Stop();

            if (_waveCoroutine != null)
                StopCoroutine(_waveCoroutine);

            SplineAnimate splineAnimate = followLine.SplineAnimate;
            splineAnimate.Loop = SplineAnimate.LoopMode.Once;
            transform.localScale = _originScale;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_isDead) return;
            
            if (other.TryGetComponent(out Unit unit))
            {
                if (unit.TeamType == enemyType)
                {
                    unit.ApplyDamage(_damage, _dealer);
                }
                else if (unit.TeamType == teamType && unit is IRideWave waveRideable && waveRideable.CanRide)
                {
                    _waveRideables.Add(waveRideable);
                    waveRideable.StartRideWave(transform, transform.rotation);
                    
                    float unitDamageMultiplier = unit.GetCompo<UnitStatCompo>().GetStat(UnitStatType.Damage).Value;
                    float unitDamage = unit.UnitData.damage * unitDamageMultiplier;
                    print($"{unit.name} damage : {unitDamage}");
                    
                    _damage += (int)(unitDamage * _damageMultiplier);
                }
            }
        }
    }
}