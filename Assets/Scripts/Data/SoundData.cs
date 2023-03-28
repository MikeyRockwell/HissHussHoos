using Audio;
using UnityEngine;
using UnityEngine.Events;

namespace Data {
    [CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObjects/Data/SoundData", order = 0)]
    public class SoundData : ScriptableObject {
        
        // This is a communication SO that sends an audio even
        // from the source, to the sound manager
        // Each audio event needs an audio source as well
        // Is there a cleaner way to do this?
        public UnityEvent<AudioEvent, AudioSource> OnAudioEvent;

        public void PlayAudio(AudioEvent audioEvent, AudioSource source) {
            OnAudioEvent?.Invoke(audioEvent, source);
        }

    }
}