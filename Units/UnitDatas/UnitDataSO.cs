using Ami.BroAudio;
using Code.Units.Upgrades;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Units.UnitDatas
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "SO/Unit/UnitData/UnitData", order = 0)]
    public class UnitDataSO : ScriptableObject
    {
        [Header("Info")]
        public string unitName;    //유닛 이름
        public Sprite icon;        //유닛 아이콘
        public PoolItemSO poolItem;  //유닛 풀링 프리팹
        public UnitUpgradeSO unitUpgradeData; //유닛 업그레이드 데이터
        [TextArea] public string description; //유닛 설명

        [Header("Stats")]
        public int health;         //체력
        public int damage;         //공격력
        public float moveSpeed;    //이동 속도
        public float attackDelay;  //공격 간격(초당으로 하기 힘들어서 이렇게 바꿈)
        public float detectRange = 3.0f; //감지 범위
        public float attackRange;  //공격 범위
        public int requiredCost; // 유닛 소환에 필요한 코스트
        public float skillCooldown;
        public int dropExpCount; //죽일시 드랍하는 경험치량
        public float knockbackMultiplier = 1.0f; //넉백 계수

        [Header("Type")]
        public UnitTeamType teamType; // 유닛 팀
        public UnitClassType classType;   // 유닛 등급
        public UnitSizeType sizeType;

        [Header("Sound")] 
        public SoundID signatureSoundData; //착지시 사운드
    }
}