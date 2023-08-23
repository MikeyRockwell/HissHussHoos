using System.Collections;
using Audio;
using UnityEngine;

namespace Managers {
    public class SoundManager : MonoBehaviour {
        private DataWrangler.GameData gd;
        [SerializeField] private AudioSource source;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.soundData.OnAudioEvent.AddListener(PlayBasicAudio);
        }

        private void PlayBasicAudio(AudioEvent audioEvent) {
            audioEvent.PlayRandomClip(source);
        }
    }
}