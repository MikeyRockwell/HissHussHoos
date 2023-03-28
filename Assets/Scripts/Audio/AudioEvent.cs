using UnityEngine;

namespace Audio {
    [CreateAssetMenu(fileName = "NewAudioEvent", menuName = "ScriptableObjects/Audio/AudioEvent", order = 0)]
    
    public class AudioEvent : ScriptableObject {
        
        public AudioClip[] clips;
        
        public Vector2 volume;
        public Vector2 pitch;
    
        public void Play(AudioSource source) {

            if (clips.Length == 0) return;
        
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            source.volume = Random.Range(volume.x, volume.y);
            source.pitch = Random.Range(pitch.x, pitch.y);
            source.PlayOneShot(clip);
        }
    }
}