using System;
using System.Collections.Generic;
using Code.Entities;
using DG.Tweening;
using Enemies;
using EventSystem;
using Level;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

namespace Code.Units.UnitAnimals.Turtles
{
    public class Turtle : WaveRideableUnit
    {
        public UnityEvent OnRushStartEvent;
        public UnityEvent OnRushAttackEvent;
        public event Action<EnemyUnit> OnEnemyCollisionEvent;
        public bool IsRushing { get; set; }

        [SerializeField] private GameEventChannelSO cannonChannel;
        [SerializeField] private GameEventChannelSO unitChannel;

        private TurtleDataSO _turtleData;
        private FollowLine _followLine;
        private UnitDetectCollider _detectCollider;

        private readonly HashSet<Unit> _flyingUnits = new HashSet<Unit>();

        protected override void Awake()
        {
            base.Awake();

            _turtleData = UnitData as TurtleDataSO;
            Debug.Assert(_turtleData != null, "Turtle data is null");

            _followLine = GetCompo<FollowLine>();

            LevelSpline levelSpline = FindAnyObjectByType<LevelSpline>();
            _followLine.SetUpFollowLine(levelSpline.Splines[0]);
            _followLine.SetDuration(_turtleData.rushSpeed);

            _detectCollider = GetCompo<UnitDetectCollider>();
            _detectCollider.OnCollision.AddListener(HandleEnemyCollision);
            
            unitChannel.AddListener<UnitSpawnEvent>(HandleUnitSpawn);
            cannonChannel.AddListener<CannonShootEvent>(HandleCannonShoot);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _detectCollider.OnCollision.RemoveListener(HandleEnemyCollision);

            unitChannel.RemoveListener<UnitSpawnEvent>(HandleUnitSpawn);
            cannonChannel.RemoveListener<CannonShootEvent>(HandleCannonShoot);
        }

        private void HandleUnitSpawn(UnitSpawnEvent evt)
        {
            if (evt.unit != this)
                _flyingUnits.Remove(evt.unit);
        }

        private void HandleCannonShoot(CannonShootEvent evt)
        {
            if (evt.unit != this)
                _flyingUnits.Add(evt.unit);
        }

        protected override void Update()
        {
            base.Update();

            if (_flyingUnits.Count == 0) return;

            foreach (Unit flyingUnit in _flyingUnits)
            {
                Vector3 flyingUnitPos = flyingUnit.transform.position;
                Vector3 turtlePos = transform.position;

                bool isInDistance = Vector3.Distance(turtlePos, flyingUnitPos) < _turtleData.crushCheckDistance;
                bool isShootToMe = flyingUnit.FlyProgress > _turtleData.needFlyingProgress;

                if (isInDistance && isShootToMe)
                {
                    ChangeState("RUSH_READY");
                    break;
                }
            }
        }

        private void HandleEnemyCollision(Unit unit)
        {
            if (unit is EnemyUnit enemy) OnEnemyCollisionEvent?.Invoke(enemy);
        }

        public void OnRushEnd()
        {
            float yRot = Random.Range(0f, 360f);
            transform.eulerAngles = new Vector3(0, yRot, 0);
        }
    }
}