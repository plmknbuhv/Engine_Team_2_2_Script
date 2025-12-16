using Code.Units.UnitDatas;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Units.UnitAnimals.Turtles
{
    [CreateAssetMenu(fileName = "TurtleData", menuName = "SO/Unit/UnitData/Turtle", order = 0)]
    public class TurtleDataSO : UnitDataSO
    {
        public float rushSpeed;
        public float rushDuration = 5f;
        public float randomizeDuration = 1f;
        public float rushDamageMultiplier;
        public float rushReadyMoveDuration;
        public float rushEndThreshold;
        public float crushCheckDistance;
        public float needFlyingProgress;
        public PoolItemSO rushSuccessEffect;
    }
}