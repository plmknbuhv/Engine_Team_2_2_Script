using System.Collections.Generic;
using Ami.BroAudio;
using Code.Entities;
using EventSystem;
using UnityEngine;

namespace Code.Units.UnitStatusEffects
{
    public enum StatusEffectType
    {
        None = -1,
        
        DEFAULT,
        FREEZE,
        STUN,
        
        Max
    }
    
    public class UnitStatusEffect : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private List<StatusEffectSo> statusEffectData;
        [SerializeField] private GameEventChannelSO soundChannel;
        
        public StatusEffectSo CurrentStatusEffectData { get; private set; }
        public StatusEffectType StatusType { get; private set; }
        public float Duration { get; private set; }
        
        public bool IsDefaultStatus => StatusType == StatusEffectType.DEFAULT;

        public void Initialize(Entity entity)
        {
            StatusType = StatusEffectType.DEFAULT;
        }
        
        public void ApplyStatusEffect(StatusEffectType statusEffectType, float duration = 0)
        {
            foreach (var statusEffect in statusEffectData)
            {
                if (statusEffect.statusEffectType == statusEffectType)
                {
                    CurrentStatusEffectData = statusEffect;

                    var evt = SoundEvents.PlaySFXSoundEvent.Initializer(CurrentStatusEffectData.statusEffectSound);
                    soundChannel.RaiseEvent(evt);
                    break;
                }
            }
            
            
            StatusType = statusEffectType;
            Duration = duration;
        }
    }
}