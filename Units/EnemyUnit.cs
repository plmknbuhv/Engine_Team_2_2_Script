using System.Collections.Generic;
using Enemies;
using EventSystem;
using UnityEngine;

namespace Code.Units
{
    public class EnemyUnit : Unit
    {
        public override Unit TargetUnit
        {
            get
            {
                if (TargetUnitList.Count == 0)
                {
                    return null;
                }
                
                return TargetUnitList[0];
            }
            set { }
        }
        
        [field: SerializeField] public GameEventChannelSO EnemyChannel { get; private set; }
        [field: SerializeField] public GameEventChannelSO LevelChannel { get; private set; }
        [field: SerializeField] public bool IsBoss { get; private set; }
        
        public List<FriendlyUnit> TargetUnitList { get; private set; } // 이거 풀링 할거면 리스트 초기화 해야한다 제발
        public EnemyDataSo EnemyData { get; private set; }
        
        public Collider Collider { get; set; }
        
        public Quaternion TargetRotation { get; set; }
        
        protected FollowLine _followLine;
        
        private float _skillTime;

        protected override void Awake()
        {
            base.Awake();
            
            EnemyData = UnitData as EnemyDataSo;
            
            Collider =  GetComponent<Collider>();
            TargetUnitList = new List<FriendlyUnit>();
            _followLine = GetCompo<FollowLine>();
        }

        protected override void Start()
        {
            base.Start();
            CanTargeting = true;
        }

        protected override void Update()
        {
            base.Update();

            if(IsDead) return;
            
            if (transform.position.x < -100 || transform.position.x > 100
                                            || transform.position.y < -100 || transform.position.y > 100
                                            || transform.position.z < -100 || transform.position.z > 100)
            {
                const string dead = "DEAD";
                ChangeState(dead);
            }

        }

        private void OnEnable()
        {
            if (_followLine == null || _followLine.SplineAnimate == null)
            {
                Debug.LogWarning("FollowLine이 없거나 SplineAnimate가 설정되지 않았습니다.");
                return;
            }
            
            _followLine.SplineAnimate.Completed += CompletePathHandle;
        }

        private void OnDisable()
        {
            if (_followLine == null || _followLine.SplineAnimate == null)
            {
                Debug.LogWarning("FollowLine이 없거나 SplineAnimate가 설정되지 않았습니다.");
                return;
            }
            
            _followLine.SplineAnimate.Completed -= CompletePathHandle;
        }
        private void CompletePathHandle()
        {
            _followLine.PauseMove();
            _followLine.SetNormalizedTime(0.99f);
            
            EnemyChannel.RaiseEvent(EnemyEvents.EnemyPathCompleteEvent.Initializer(EnemyData.enemyType));
            EnemyChannel.RaiseEvent(EnemyEvents.EnemyListChangedEvent.Initializer(this, EnemyRegistry.Remove));
            
            const string dead = "DEAD";
            ChangeState(dead);
        }

        public override void SetUpUnit()
        {
            base.SetUpUnit();
            _followLine.SplineAnimate.Restart(false);
            _followLine.SetDuration(UnitData.moveSpeed);
            
            IsInBattle = false;
            
            UnitCounter.AddUnit(UnitTeamType.Enemy, this);
            
            _stateMachine.CanChangeState = true;
            Collider.enabled = true;
            
            const string walk = "WALK";
            ChangeState(walk);
            
            OnSetUpEvent.Invoke();
        }
        
        public override void ResetItem()
        {
            SetUpUnit();
        }
        
        // 아군이 적군한테 시비거는 함수
        public void MarkAttackTarget(FriendlyUnit dealer) 
        {
            if(TargetUnitList.Contains(dealer)) return;
            
            const string idle = "IDLE";
            
            IsInBattle = true;
            
            TargetUnitList.Add(dealer);
            
            ChangeState(idle);
        }

        public void StartBattle()
        {
            const string attack = "ATTACK";
            ChangeState(attack);
        }
    }
}