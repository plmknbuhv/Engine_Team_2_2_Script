using Code.Units.Movements;
using Code.Units.UnitStat;
using Code.Units.UnitStatusEffects;
using Code.Util;
using Enemies;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Units.UnitAnimals.Walruses
{
    public class Walrus : FriendlyUnit
    {
        public UnityEvent OnWalrusBite;
        
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color biteModeColor;

        private UnitDetector _detector;
        private UnitStatCompo _statCompo;
        private WalrusDataSO _walrusData;
        private ColorMaskComponent _colorMaskCompo;
        private bool _isChaseBiteTarget;

        protected override void Awake()
        {
            base.Awake();

            _detector = GetCompo<UnitDetector>();
            _statCompo = GetCompo<UnitStatCompo>();
            _colorMaskCompo = GetCompo<ColorMaskComponent>();

            _walrusData = UnitData as WalrusDataSO;
        }

        public override void ResetItem()
        {
            base.ResetItem();

            _colorMaskCompo.SetColor(defaultColor);
        }

        public bool TryGetFreezeEnemy(out Unit target)
        {
            bool IsFreezeTarget(Unit unit) =>
                UnitStatusUtil.IsTargetStatus(unit, StatusEffectType.FREEZE) && unit.CanTargeting &&
                unit.UnitData is EnemyDataSo { enemyType: EnemyType.Common };

            _detector.SetTarget(UnitTeamType.Enemy);
            bool hasFreezeEnemy = _detector.TryGetClosestEnemy(out Unit enemy, UnitData.detectRange, IsFreezeTarget);

            target = enemy;
            return hasFreezeEnemy;
        }

        public void StartChaseToBiteTarget(Unit target)
        {
            bool isNotTarget = target.UnitData is EnemyDataSo { enemyType: EnemyType.Common } == false;
            if (_isChaseBiteTarget || isNotTarget) return;

            _isChaseBiteTarget = true;

            if (TargetUnit != null && TargetUnit is EnemyUnit enemy)
            {
                enemy.TargetUnitList.Remove(this);
            }

            TargetUnit = target;
            print(target);

            if (_statCompo.TryGetStat(UnitStatType.MoveSpeed, out Stat speedStat))
            {
                speedStat.AddModifier(this, _walrusData.speedIncreaseOnBiteTarget);
            }

            _colorMaskCompo.SetColor(biteModeColor);
        }

        public void StopChaseToBiteTarget()
        {
            if (_isChaseBiteTarget == false) return;

            _isChaseBiteTarget = false;

            if (_statCompo.TryGetStat(UnitStatType.MoveSpeed, out Stat speedStat))
            {
                speedStat.RemoveModifier(this);
            }

            _colorMaskCompo.SetColor(defaultColor);
        }
    }
}