using Data;
using UnityEngine;

namespace Audio {
    /// <summary>
    /// Part of the SFX Player prefab
    /// Plays a random audio clip using the AudioEvent
    /// Requires a SoundData SO and an AudioEvent SO
    /// Requires an AudioSource
    /// </summary>
    public class SoundFXPlayer : MonoBehaviour {
        public SoundData SoundData;
        public AudioEvent AudioEvent;
        public AudioSource Source;

        public void PlayRandomAudio() {
            AudioEvent.PlayRandomClip(Source);
        }

        public void PlayAudioDetached() {
            SoundData.PlayAudioDetached(AudioEvent);
        }
    }
}