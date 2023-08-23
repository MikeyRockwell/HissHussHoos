using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace Audio {
    // SO Audio event that is authored in the inspector
    // Contains a list of audio clips, volume, pitch, and chance
    // Can be played by an object with an AudioSource
    [CreateAssetMenu(fileName = "NewAudioEvent", menuName = "ScriptableObjects/Audio/AudioEvent", order = 0)]
    public class AudioEvent : ScriptableObject {
        
        public AudioClip[] clips;
        [MinMaxSlider(0, 1)] public Vector2 volume;
        [MinMaxSlider(0, 2)] public Vector2 pitch;
        [Range(0, 1)] public float chance;

        public float coolDownDuration;
        public bool coolingDown;
        public bool priority;
        public AudioClip currentClip;
        public bool voiceEffects;

        public void PlayRandomClip(AudioSource source) {
            // AudioEvent has a cooldown that can be set to zero to disable it
            // If the cooldown is active, return
            if (coolingDown) return;
            // If there is no clip, return
            if (clips.Length == 0) return;
            // Select a random clip from the list
            currentClip = clips[Random.Range(0, clips.Length)];
            // Set the volume and pitch to random values
            source.volume = Random.Range(volume.x, volume.y);
            source.pitch = Random.Range(pitch.x, pitch.y);
            // Play the clip
            source.PlayOneShot(currentClip);
        }

        public IEnumerator CoolDown() { // Coroutine to cool down the AudioEvent
            if (coolDownDuration == 0) yield break;
            coolingDown = true;
            yield return new WaitForSeconds(coolDownDuration);
            coolingDown = false;
        }
    }
}