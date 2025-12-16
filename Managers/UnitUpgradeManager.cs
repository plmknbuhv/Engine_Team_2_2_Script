using System.Collections.Generic;
using Code.Units;
using Code.Units.UnitStat;
using Code.Units.Upgrades;
using Code.Upgrades;
using Code.Upgrades.Data;
using EventSystem;
using UnityEngine;

namespace Code.Managers
{
    public class UnitUpgradeManager : Upgrader
    {
        [SerializeField] private GameEventChannelSO unitChannel;
        [SerializeField] private GameEventChannelSO enemyChannel;
        //Test
        [SerializeField] private UpgradeDataSO testUpgrade;
        
        private readonly Dictionary<UnitUpgradeSO, int> _upgradePairs = new Dictionary<UnitUpgradeSO, int>();
        private readonly Dictionary<UnitStatUpgradeSO, int> _statPairs = new Dictionary<UnitStatUpgradeSO, int>();
        private readonly HashSet<Unit> _aliveUnits = new HashSet<Unit>();

        protected override void Awake()
        {
            base.Awake();

            InitUpgrades();
            
            unitChannel.AddListener<UnitSpawnEvent>(HandleUnitSpawn);
            unitChannel.AddListener<UnitDeadEvent>(HandleUnitDead);
            
            enemyChannel.AddListener<EnemySpawnEvent>(HandleEnemySpawn);
            enemyChannel.AddListener<EnemyDeathEvent>(HandleEnemyDead);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            unitChannel.RemoveListener<UnitSpawnEvent>(HandleUnitSpawn);
            unitChannel.RemoveListener<UnitDeadEvent>(HandleUnitDead);
            
            enemyChannel.RemoveListener<EnemySpawnEvent>(HandleEnemySpawn);
            enemyChannel.RemoveListener<EnemyDeathEvent>(HandleEnemyDead);
        }

        private void InitUpgrades()
        {
            foreach (UpgradeDataSO upgradeData in upgradeDatas)
            {
                if(upgradeData is UnitUpgradeSO unitUpgrade)
                    _upgradePairs.Add(unitUpgrade, 0);
                else if(upgradeData is UnitStatUpgradeSO statUpgrade)
                    _statPairs.TryAdd(statUpgrade, 0);
            }
        }

        #region Unit spawn

        private void HandleUnitSpawn(UnitSpawnEvent evt)
        {
            UnitUpgradeOnSpawn(evt);
            UnitStatUpgradeOnSpawn(evt);
        }

        private void HandleUnitDead(UnitDeadEvent evt)
        {
            _aliveUnits.Remove(evt.unit);
        }

        #endregion

        #region Enemy spawn

        private void HandleEnemySpawn(EnemySpawnEvent evt)
        {
            _aliveUnits.Add(evt.enemy);
        }
        
        private void HandleEnemyDead(EnemyDeathEvent evt)
        {
            _aliveUnits.Remove(evt.enemy);
        }

        #endregion

        protected override void Upgrade(int currentLevel, UpgradeDataSO data)
        {
            if (data is UnitUpgradeSO unitUpgrade && _upgradePairs.ContainsKey(unitUpgrade)) //유닛 업그레이드
            {
                UnitUpgrade(unitUpgrade, currentLevel);
            }
            else if (data is UnitStatUpgradeSO statUpgrade && _statPairs.ContainsKey(statUpgrade)) //유닛 스탯 업그레이드
            {
                UnitStatUpgrade(statUpgrade, currentLevel);
            }
            else if (data is UnitStatusEffectUpgradeSO statusUpgrade)
            {
                UnitStatusEffectUpgrade(statusUpgrade);
            }
        }

        #region Unit Upgrade

        private void UnitUpgradeOnSpawn(UnitSpawnEvent evt)
        {
            ApplyUnitUpgrade(evt.unit);
        }

        private void UnitUpgrade(UnitUpgradeSO unitUpgrade, int currentLevel)
        {
            _upgradePairs[unitUpgrade] = currentLevel;
            
            foreach (Unit aliveUnit in _aliveUnits)
            {
                ApplyUnitUpgrade(aliveUnit);
            }
        }

        private void ApplyUnitUpgrade(Unit unit)
        {
            UnitUpgradeSO unitUpgradeData = unit.UnitData.unitUpgradeData;
            
            if(unitUpgradeData == null || unit.UnitData.unitUpgradeData != unitUpgradeData) return;
            
            if ( _upgradePairs.TryGetValue(unitUpgradeData, out int level)) //유닛 업그레이드
            {
                unit.UpgradeUnit(level);
            }
        }

        #endregion

        #region Unit Stat Upgrade

        private void UnitStatUpgradeOnSpawn(UnitSpawnEvent evt)
        {
            UnitStatCompo statCompo = evt.unit.GetCompo<UnitStatCompo>();
            
            foreach (KeyValuePair<UnitStatUpgradeSO, int> statPair in _statPairs) //유닛 스탯 업그레이드
            {
                ApplyStatUpgrade(evt.unit, statCompo, statPair.Key);
            }

            _aliveUnits.Add(evt.unit);
        }

        private void UnitStatUpgrade(UnitStatUpgradeSO statUpgrade, int currentLevel)
        {
            _statPairs[statUpgrade] = currentLevel;

            foreach (Unit aliveUnit in _aliveUnits)
            {
                UnitStatCompo statCompo = aliveUnit.GetCompo<UnitStatCompo>();
                
                ApplyStatUpgrade(aliveUnit, statCompo, statUpgrade);
            }
        }

        private void ApplyStatUpgrade(Unit targetUnit, UnitStatCompo statCompo, UnitStatUpgradeSO upgrade)
        {
            bool hasStat = statCompo.TryGetStat(upgrade.statType, out Stat stat);
            bool isTeamUnit = targetUnit.TeamType == upgrade.targetTeam;
                
            if (hasStat && isTeamUnit)
            {
                stat.RemoveModifier(this);
                float addAmount = upgrade.addValue * _statPairs[upgrade];
                stat.AddModifier(this, addAmount);
            }
        }

        #endregion

        #region Unit Status Effect

        private void UnitStatusEffectUpgrade(UnitStatusEffectUpgradeSO statusUpgrade)
        {
            foreach (Unit aliveUnit in _aliveUnits)
            {
                ApplyStatusEffect(aliveUnit, statusUpgrade);
            }
        }
        
        private void ApplyStatusEffect(Unit targetUnit, UnitStatusEffectUpgradeSO upgrade)
        {
            if (targetUnit.TeamType == upgrade.targetTeam)
            {
                targetUnit.ApplyStatusEffect(upgrade.effectType, upgrade.duration);
            }
        }

        #endregion

        [ContextMenu("Test Upgrade")]
        public void TestUpgrade()
        {
            if (testUpgrade is UnitUpgradeSO unitUpgrade)
            {
                int level = _upgradePairs[unitUpgrade] + 1;
                Upgrade(level, unitUpgrade);
            }
            else if (testUpgrade is UnitStatUpgradeSO statUpgrade)
            {
                Upgrade(0, statUpgrade);
            }
            else if (testUpgrade is UnitStatusEffectUpgradeSO statusUpgrade)
            {
                UnitStatusEffectUpgrade(statusUpgrade);
            }
        }
    }
}