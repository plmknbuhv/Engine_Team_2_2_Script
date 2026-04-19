using System.Collections.Generic;
using System.Linq;
using Code.Upgrades.Data;
using EventSystem;
using GondrLib.Dependencies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Upgrades
{
    [DefaultExecutionOrder(-5), Provide]
    public class UpgradeManager : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private UpgradeEventInvokerSO upgradeInvoker;
        [SerializeField] private GameEventChannelSO upgradeChannel;

        ///모든 업그레이드 데이터(서브 업그레이드 포함)
        [SerializeField] private UpgradeGroupSO[] allUpgrades;

        ///처음부터 사용할 수 있는 업그레이드 데이터
        [SerializeField] private UpgradeGroupSO defaultUpgrades;

        [SerializeField] private int upgradeViewCount = 3;

        ///업그레이드들의 현재 레벨
        private Dictionary<string, int> _currentLevelDict;

        ///해제되어 있는 업그레이드만 업그레이드 된다.
        private Dictionary<string, bool> _unlockUpgradeDict;

        ///현재 사용할 수 있는 업그레이드들
        private List<UpgradeDataSO> _canUseUpgrades;

        #region Test

        [SerializeField] private UpgradeDataSO testUpgradeData;

        #endregion

        private void Awake()
        {
            _unlockUpgradeDict = new Dictionary<string, bool>();
            _currentLevelDict = new Dictionary<string, int>();
            _canUseUpgrades = defaultUpgrades.upgradeTargets.ToList();

            upgradeInvoker.Initialize();

            foreach (var group in allUpgrades)
            {
                foreach (var upgrader in group.upgradeTargets)
                {
                    if (!_currentLevelDict.TryAdd(upgrader.upgradeID, 0))
                    {
                        Debug.LogWarning($"{upgrader.upgradeID} already exists. Group: {group.name}");
                        continue;
                    }

                    _unlockUpgradeDict.Add(upgrader.upgradeID, false);
                }
            }

            foreach (var data in _canUseUpgrades)
            {
                _unlockUpgradeDict[data.upgradeID] = true;
            }

            SubscribeChannel();
        }

        private void OnDestroy()
        {
            UnsubscribeChannel();
        }

        #region Handle session

        private void SubscribeChannel()
        {
            upgradeChannel.AddListener<TargetUpgradeEvent>(HandleTargetUpgrade);
            upgradeChannel.AddListener<UnlockUpgradeEvent>(HandleUnlockUpgrader);
            upgradeChannel.AddListener<RequestSelectUpgradesEvent>(HandleRequestSelectUpgrades);
        }

        private void UnsubscribeChannel()
        {
            upgradeChannel.RemoveListener<TargetUpgradeEvent>(HandleTargetUpgrade);
            upgradeChannel.RemoveListener<UnlockUpgradeEvent>(HandleUnlockUpgrader);
            upgradeChannel.RemoveListener<RequestSelectUpgradesEvent>(HandleRequestSelectUpgrades);
        }

        private void HandleTargetUpgrade(TargetUpgradeEvent evt)
        {
            Upgrade(evt.target);
        }

        private void HandleUnlockUpgrader(UnlockUpgradeEvent evt)
        {
            UnlockUpgrade(evt.unlockUpgrade);
        }

        private void HandleRequestSelectUpgrades(RequestSelectUpgradesEvent evt)
        {
            var upgraders = GetRandomUpgrades();

            var selectUpgradeEvt =
                UpgradeEvents.SelectRandomUpgradesEvent.Initializer(upgraders);
            upgradeChannel.RaiseEvent(selectUpgradeEvt);
        }

        #endregion

        /// <summary>
        /// 업그레이드를 지정한 업그레이드 만큼 뽑아 반환한다.
        /// </summary>
        public UpgradeDataSO[] GetRandomUpgrades()
        {
            var selectUpgrades = new List<UpgradeDataSO>();
            var availableUpgrades = _canUseUpgrades.Where(CanUpgrade).ToList();

            for (int i = 0; i < upgradeViewCount; i++)
            {
                if (availableUpgrades.Count <= 0) return selectUpgrades.ToArray();

                int rmIdx = Random.Range(0, availableUpgrades.Count);
                var data = availableUpgrades[rmIdx];
                availableUpgrades.RemoveAt(rmIdx);
                selectUpgrades.Add(data);
            }

            return selectUpgrades.ToArray();
        }

        public void Upgrade(UpgradeDataSO upgradeData)
        {
            if (!CanUpgrade(upgradeData))
            {
                Debug.Log($"{upgradeData.upgradeID}가 존재하지 않거나 이미 최대 레벨입니다.");
                return;
            }

            //업그레이드 진행

            if (!upgradeData.isOneTime)
                _currentLevelDict[upgradeData.upgradeID]++;
            
            upgradeInvoker.RaiseEvent(upgradeData, _currentLevelDict[upgradeData.upgradeID]);

            if (_currentLevelDict[upgradeData.upgradeID] == 1)
            {
                UnlockSubUpgrades(upgradeData.subUpgradeDatas);
            }
        }

        /// <summary>
        /// 업그레이드 할 수 있는지 확인(해금 되어 있는가, 최대 레벨인가)
        /// </summary>
        /// <param name="upgradeData">확인할 업그레이드</param>
        /// <returns></returns>
        public bool CanUpgrade(UpgradeDataSO upgradeData)
        {
            if (!_currentLevelDict.TryGetValue(upgradeData.upgradeID, out var level)) return false;
            if (!_unlockUpgradeDict.TryGetValue(upgradeData.upgradeID, out var isUnlock)) return false;

            return level < upgradeData.maxLevel && isUnlock;
        }

        /// <summary>
        /// 업그레이드를 해금한다.
        /// </summary>
        /// <param name="targetUpgrade">해금할 업그레이드</param>
        private void UnlockUpgrade(UpgradeDataSO targetUpgrade)
        {
            _canUseUpgrades.Add(targetUpgrade);
            _unlockUpgradeDict[targetUpgrade.upgradeID] = true;
        }

        /// <summary>
        /// 하위 업그레이더들을 해금한다.
        /// </summary>
        /// <param name="subUpgradeDatas">해금할 하위 업그레이드들</param>
        private void UnlockSubUpgrades(UpgradeDataSO[] subUpgradeDatas)
        {
            foreach (var subUpgrade in subUpgradeDatas)
            {
                UnlockUpgrade(subUpgrade);
            }
        }

        [ContextMenu("Test Upgrade")]
        private void TestUpgrade()
        {
            if (testUpgradeData == null) return;

            Upgrade(testUpgradeData);
        }
    }
}