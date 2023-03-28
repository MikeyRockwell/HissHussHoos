using Audio;
using UnityEngine;

namespace Managers {
    
    public class SoundManager : MonoBehaviour {

        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.soundData.OnAudioEvent.AddListener(PlayBasicAudio);
        }

        private static void PlayBasicAudio(AudioEvent audioEvent, AudioSource source) {
            audioEvent.Play(source);
        }
    }
}