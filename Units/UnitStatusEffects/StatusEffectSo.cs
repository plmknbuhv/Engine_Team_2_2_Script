using Ami.BroAudio;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Code.Units.UnitStatusEffects
{
    [CreateAssetMenu(fileName = "StatusEffectSo", menuName = "SO/StatusEffect/StatusEffectSo", order = 0)]
    public class StatusEffectSo : ScriptableObject
    {
        public StatusEffectType statusEffectType;
        public PoolItemSO effectPoolItem;
        public SoundID statusEffectSound;
    }
}