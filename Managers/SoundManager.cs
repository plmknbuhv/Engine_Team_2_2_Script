using System.Collections.Generic;
using Ami.BroAudio;
using Ami.BroAudio.Runtime;
using EventSystem;
using UnityEngine;

namespace Code.Managers
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO soundChannel;

        private Dictionary<string, IAudioPlayer> _stopAudioDict;
        
        private void Awake()
        {
            _stopAudioDict = new Dictionary<string, IAudioPlayer>();
            
            soundChannel.AddListener<PlaySFXSoundEvent>(HandlePlaySound);
            soundChannel.AddListener<PlayLongSFXSoundEvent>(HandlePlayLongSound);
            soundChannel.AddListener<StopLongSFXSoundEvent>(HandleStopLongSound);
        }
        
        private void OnDestroy()
        {
            soundChannel.RemoveListener<PlaySFXSoundEvent>(HandlePlaySound);
            soundChannel.RemoveListener<PlayLongSFXSoundEvent>(HandlePlayLongSound);
            soundChannel.RemoveListener<StopLongSFXSoundEvent>(HandleStopLongSound);
        }

        private void HandlePlaySound(PlaySFXSoundEvent evt)
        {
            BroAudio.Play(evt.soundID);
        }
        
        private void HandlePlayLongSound(PlayLongSFXSoundEvent evt)
        {
            if (string.IsNullOrEmpty(evt.stopKey)) return;
            if (_stopAudioDict.ContainsKey(evt.stopKey))
            {
                Debug.LogWarning($"sound ID '{evt.stopKey}' is already added");
                return;
            }
            
            var player = BroAudio.Play(evt.soundID);
            _stopAudioDict.Add(evt.stopKey, player);
        }
        
        private void HandleStopLongSound(StopLongSFXSoundEvent evt)
        {
            if (string.IsNullOrEmpty(evt.stopKey)) return;
            if (!_stopAudioDict.TryGetValue(evt.stopKey, out var player))
            {
#if UNITY_EDITOR
                Debug.LogWarning($"sound ID '{evt.stopKey}' is not found");
#endif
                return;
            }
            
            if(player.IsPlaying)
                player.Stop();
            
            _stopAudioDict.Remove(evt.stopKey);
        }
    }
}