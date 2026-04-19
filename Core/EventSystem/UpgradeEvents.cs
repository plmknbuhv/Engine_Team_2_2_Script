using Code.Upgrades;
using Code.Upgrades.Data;

namespace EventSystem
{
    public static class UpgradeEvents
    {
        public static readonly TargetUpgradeEvent TargetUpgradeEvent = new TargetUpgradeEvent();
        public static readonly UnlockUpgradeEvent UnlockUpgradeEvent = new UnlockUpgradeEvent();
        public static readonly RequestSelectUpgradesEvent RequestSelectUpgradesEvent = new RequestSelectUpgradesEvent();
        public static readonly SelectRandomUpgradesEvent SelectRandomUpgradesEvent = new SelectRandomUpgradesEvent();
    }

    /// <summary>
    /// 업그레이드 한다.
    /// </summary>
    public class TargetUpgradeEvent : GameEvent
    {
        public UpgradeDataSO target;

        public TargetUpgradeEvent Initializer(UpgradeDataSO data)
        {
            target = data;
            return this;
        }
    }
    
    /// <summary>
    /// 업그레이드 잠금 해제
    /// </summary>
    public class UnlockUpgradeEvent : GameEvent
    {
        public UpgradeDataSO unlockUpgrade;

        public UnlockUpgradeEvent Initializer(UpgradeDataSO data)
        {
            unlockUpgrade = data;
            return this;
        }
    }
    
    /// <summary>
    /// 선택창에 띄울 업그레이드 요청
    /// </summary>
    public class RequestSelectUpgradesEvent : GameEvent
    {
    }
    
    /// <summary>
    /// 선택창에 띄울 업그레이드 전달
    /// </summary>
    public class SelectRandomUpgradesEvent : GameEvent
    {
        public UpgradeDataSO[] upgrades;

        public SelectRandomUpgradesEvent Initializer(UpgradeDataSO[] datas)
        {
            upgrades = datas;
            return this;
        }
    }
}