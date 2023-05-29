using Sirenix.OdinInspector;
using UnityEngine;

namespace Audio {
    [CreateAssetMenu(fileName = "NewAudioEvent", menuName = "ScriptableObjects/Audio/AudioEvent", order = 0)]
    
    public class AudioEvent : ScriptableObject {
        
        public AudioClip[] clips;
        [MinMaxSlider(0, 1)] public Vector2 volume;
        [MinMaxSlider(0, 2)] public Vector2 pitch;
    
        public void PlayRandomClip(AudioSource source) {

            if (clips.Length == 0) return;
        
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            source.volume = Random.Range(volume.x, volume.y);
            source.pitch = Random.Range(pitch.x, pitch.y);
            source.PlayOneShot(clip);
        }
    }
}