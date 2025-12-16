using System;
using Code.Entities;
using EventSystem;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Units
{
    public class FriendlyUnit : Unit
    {
        [SerializeField] private bool isSubUnit;

        protected NavMeshAgent _navAgent;

        public override Unit TargetUnit
        {
            get => _targetUnit;
            set
            {
                if (_targetUnit != null && _targetUnit is EnemyUnit enemy)
                {
                    enemy.TargetUnitList.Remove(this);
                }

                _targetUnit = value;
            }
        }
        public bool IsFalling { get; private set; }

        private Unit _targetUnit;
        
        protected override void Awake()
        {
            base.Awake();
            
            _navAgent = GetComponent<NavMeshAgent>();

            UnitChannel.AddListener<UnitDeadEvent>(HandleUnitDead);
        }

        protected virtual void OnDestroy()
        {
            UnitChannel.RemoveListener<UnitDeadEvent>(HandleUnitDead);
        }

        [ContextMenu("TestSetupUnit")]
        public override void SetUpUnit()
        {
            base.SetUpUnit();
            
            ChangeState("IDLE");
            
            if (isSubUnit == false)
            {
                UnitChannel.RaiseEvent(UnitEvents.UnitSpawnEvent.Initializer(this));
                UnitCounter.AddUnit(UnitTeamType.Friendly, this);
            }

            IsFalling = false;
        }

        public override void ResetItem()
        {
            base.ResetItem();
            
            IsFalling = true;
        }

        public void DestroyUnit()
        {
            if(IsDead) return;
            
            IsDead = true;
            OnDeadEvent?.Invoke();
            
            DeadUnit();
            Pool.Push(this);
        }

        public void DeadUnit()
        {
            if (TargetUnit != null)
            {
                EnemyUnit enemyUnit = TargetUnit as EnemyUnit;
                Debug.Assert(enemyUnit != null, "적을 가지고 있긴 한데 적이 아님");

                if (enemyUnit != null) // 만약에 싸우던 애가 있으면 걔 타겟에서 나를 빼
                {
                    enemyUnit.TargetUnitList.Remove(this);
                    TargetUnit = null;
                }
            }

            UnitCounter.RemoveUnit(UnitTeamType.Friendly, this);
        }

        private void HandleUnitDead(UnitDeadEvent evt)
        {
            if (TargetUnit == evt.unit) TargetUnit = null;
        }
        
        public void SetActiveNavAgent(bool isActive) => _navAgent.enabled = isActive;
    }
}