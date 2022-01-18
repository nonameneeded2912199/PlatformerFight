using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerFight.CharacterThings
{
    public class CharacterAudio : MonoBehaviour
    {
        [SerializeField] 
        protected AudioCueEventChannelSO _sfxEventChannel = default;

        [SerializeField] 
        protected AudioConfigurationSO _audioConfig = default;

        public void PlayAudio(AudioCueSO audioCue, Vector3 positionInSpace = default)
        {
            _sfxEventChannel.RaisePlayEvent(audioCue, _audioConfig, positionInSpace);
        }
    }
}
