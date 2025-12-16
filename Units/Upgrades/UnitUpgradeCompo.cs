using Code.Entities;
using UnityEngine;

namespace Code.Units.Upgrades
{
    public class UnitUpgradeCompo : MonoBehaviour, IEntityComponent
    {
        public int UpgradeCnt { get; private set; }
        public bool CanUpgrade => upgradeSO != null && UpgradeCnt < upgradeSO.maxLevel;

        [SerializeField] private UnitUpgradeSO upgradeSO;
        
        private Unit _unit;
        
        public void Initialize(Entity entity)
        {
            _unit = entity as Unit;
        }

        public void Upgrade(int currentLevel)
        {
            if(CanUpgrade == false) return;

            int beforeCnt = UpgradeCnt;
            UpgradeCnt = Mathf.Clamp(currentLevel, 0, upgradeSO.maxLevel);

            int upgradeAmount = UpgradeCnt - beforeCnt;
            
            if(upgradeAmount <= 0) return;

            for (int i = 0; i < upgradeAmount; ++i)
            {
                UnitPerkUpgradeSO perkUpgrade = upgradeSO.perkUpgrades;
                perkUpgrade.UpgradeUnit(_unit);
            }
        }

        public void RollbackUpgrade(int currentLevel)
        {
            if(UpgradeCnt <= 0) return;

            int beforeCnt = UpgradeCnt;
            UpgradeCnt = Mathf.Clamp(currentLevel, 0, upgradeSO.maxLevel);

            int rollbackAmount = beforeCnt - UpgradeCnt;
            
            if(rollbackAmount <= 0) return;
            
            for (int i = 0; i < rollbackAmount; ++i)
            {
                UnitPerkUpgradeSO perkUpgrade = upgradeSO.perkUpgrades;
                perkUpgrade.RollbackUpgrade(_unit);
            }
        }
    }
}