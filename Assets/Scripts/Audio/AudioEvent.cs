using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;
using Utils;

namespace Audio
{
    [CreateAssetMenu(fileName = "NewAudioEvent", menuName = "ScriptableObjects/Audio/AudioEvent", order = 0)]
    public class AudioEvent : ScriptableObject
    {
        public AudioClip[] clips;
        [MinMaxSlider(0, 1)] public Vector2 volume;
        [MinMaxSlider(0, 2)] public Vector2 pitch;
        [Range(0, 1)] public float chance;

        public float coolDownDuration;
        public bool coolingDown;
        public bool priority;
        public AudioClip currentClip;
        public bool voiceEffects;

        public void PlayRandomClip(AudioSource source)
        {
            if (coolingDown) return;

            if (clips.Length == 0) return;

            currentClip = clips[Random.Range(0, clips.Length)];
            source.volume = Random.Range(volume.x, volume.y);
            source.pitch = Random.Range(pitch.x, pitch.y);
            source.PlayOneShot(currentClip);
        }


        public IEnumerator CoolDown()
        {
            if (coolDownDuration == 0) yield break;
            coolingDown = true;
            yield return new WaitForSeconds(coolDownDuration);
            coolingDown = false;
        }
    }
}