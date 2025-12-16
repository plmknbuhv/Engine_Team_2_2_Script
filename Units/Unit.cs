using Code.Combat;
using Code.Entities;
using Code.Entities.FSM;
using Code.Units.Movements;
using Code.Units.State;
using Code.Units.UnitDatas;
using Code.Units.UnitStat;
using Code.Units.UnitStatusEffects;
using Code.Units.Upgrades;
using EventSystem;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Units
{
    public abstract class Unit : Entity, IDamageable, IPoolable, IStatusEffect, ICanKnockback
    {
        [field: SerializeField] public UnitDataSO UnitData { get; private set; }
        [field: SerializeField] public PoolItemSO PoolItem { get; set; }
        [field: SerializeField] public UnitCounterSO UnitCounter { get; set; }
        [field: SerializeField] public GameEventChannelSO UnitChannel { get; set; }

        [field: SerializeField] public bool CanTargeting { get; set; }

        [SerializeField] private StateDataSO[] states;
        protected EntityStateMachine _stateMachine;
        protected UnitStatusEffect _statusEffect;
        private UnitHealth _health;
        private Movement _movement;
        private Collider _collider;
        public GameObject GameObject => gameObject;
        public UnitTeamType TeamType { get; private set; }
        public abstract Unit TargetUnit { get; set; }
        public Pool Pool { get; private set; }
        public bool IsInBattle { get; set; }
        public float FlyProgress { get; private set; }

        public bool CanChangeState
        {
            get => _stateMachine.CanChangeState;
            set => _stateMachine.CanChangeState = value;
        }

        public UnityEvent OnSetUpEvent;

        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new EntityStateMachine(this, states);
            TeamType = UnitData.teamType;

            _statusEffect = GetCompo<UnitStatusEffect>();
            _health = GetCompo<UnitHealth>();
            _movement = GetCompo<Movement>();

            _collider = GetComponent<Collider>();
        }

        public virtual void SetUpUnit()
        {
            IsDead = false;
            _health.SetUpHealth(UnitData.health);

            OnSetUpEvent?.Invoke();
        }

        protected virtual void Update()
        {
            _stateMachine.UpdateStateMachine();
        }

        protected virtual void FixedUpdate()
        {
            _stateMachine.FixedUpdateStateMachine();
        }

        public virtual void ApplyDamage(int damage, Entity dealer)
        {
            _health.ApplyDamage(damage);

            if (IsDead && GetCurrentState() is not UnitDeadState)
            {
                UnitChannel.RaiseEvent(UnitEvents.UnitDeadEvent.Initializer(this));
                ChangeState("DEAD", true);
                _stateMachine.CanChangeState = false;
            }
        }

        public virtual void ApplyStatusEffect(StatusEffectType statusEffectType, float duration = 0)
        {
            _statusEffect.ApplyStatusEffect(statusEffectType, duration);

            if (statusEffectType == StatusEffectType.DEFAULT) return;

            ChangeState(statusEffectType.ToString());
        }

        public void SetUpPool(Pool pool)
        {
            Pool = pool;
        }

        public void SetFlyProgress(float progress) => FlyProgress = progress;

        public virtual void ResetItem()
        {
            UnitStatCompo statCompo = GetCompo<UnitStatCompo>();
            statCompo.ResetStat();

            UnitUpgradeCompo upgradeCompo = GetCompo<UnitUpgradeCompo>();
            upgradeCompo?.RollbackUpgrade(0);

            _stateMachine.CanChangeState = true;

            const string idle = "IDLE";
            ChangeState(idle);
        }

        public void ChangeState(string newStateName, bool forced = false)
            => _stateMachine.ChangeState(newStateName, forced);

        public UnitState GetCurrentState() => _stateMachine.CurrentState as UnitState;

        public virtual void UpgradeUnit(int currentLevel)
        {
            UnitUpgradeCompo upgradeCompo = GetCompo<UnitUpgradeCompo>();

            if (upgradeCompo == null) return;

            if (upgradeCompo.UpgradeCnt <= currentLevel)
                upgradeCompo.Upgrade(currentLevel);
            else
                upgradeCompo.RollbackUpgrade(currentLevel);
        }

        public void Knockback(Vector3 direction, float force, Entity dealer)
        {
            _movement.Knockback(direction, force, dealer);
        }

        public Vector3 GetClosestPoint(Vector3 position)
        {
            return _collider.ClosestPoint(position);
        }

        [ContextMenu("Print Current State")]
        public void PrintCurrentState()
        {
            print($"Current State : {_stateMachine.CurrentState}");
        }

        protected virtual void OnDrawGizmosSelected()
        {
            if (UnitData == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, UnitData.detectRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, UnitData.attackRange);
        }
    }
}