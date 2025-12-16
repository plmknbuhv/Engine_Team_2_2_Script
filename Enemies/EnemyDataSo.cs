using Code.Units.UnitDatas;
using UnityEngine;

namespace Enemies
{
    public enum EnemyType
    {
        None = 0,
        Common = 1,
        Special = 5,
        Boss = 15,
    }
    
    [CreateAssetMenu(fileName = "EnemyData", menuName = "SO/Unit/Enemy/EnemyData", order = 0)]
    public class EnemyDataSo : UnitDataSO
    {
        public EnemyType enemyType;
    }
}