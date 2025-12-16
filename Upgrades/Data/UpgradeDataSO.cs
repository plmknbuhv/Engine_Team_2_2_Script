using UnityEngine;

namespace Code.Upgrades.Data
{
    [CreateAssetMenu(fileName = "UpgradeData", menuName = "SO/Upgrade/Data", order = 0)]
    public class UpgradeDataSO : ScriptableObject
    {
        /// 업그레이드 아이디 (고유값)
        public string upgradeID;
        
        /// 업그레이드 타입
        public UpgradeType upgradeType;
        
        //업그레이드 아이콘
        public Sprite icon;
        
        /// 업그레이드의 이름(표시용) 
        public string displayName;
        
        /// 업그레이드의 최대 레벨
        [Range(1, 10)] public int maxLevel;

        /// 일시적 업그레이드 인지 확인
        public bool isOneTime;
        
        /// 업그레이드할 크기
        public float upgradeValue;
        
        /// 업그레이드를 1번 이상 했다면 같이 열리는 업그레이드들
        public UpgradeDataSO[] subUpgradeDatas; 
        
        /// 업그레이드 설명(참고, 표시용)
        [Space,TextArea] public string description;
    }
}