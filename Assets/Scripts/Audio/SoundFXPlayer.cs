using Data;
using UnityEngine;

namespace Audio
{
    public class SoundFXPlayer : MonoBehaviour
    {
        public SoundData SoundData;
        public AudioEvent AudioEvent;
        public AudioSource Source;

        public void PlayRandomAudio()
        {
            AudioEvent.PlayRandomClip(Source);
        }

        public void PlayAudioDetached()
        {
            SoundData.PlayAudioDetached(AudioEvent);
        }
    }
}