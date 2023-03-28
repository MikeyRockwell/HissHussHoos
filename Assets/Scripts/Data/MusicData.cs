using UnityEngine;
using UnityEngine.Events;

namespace Data {
    [CreateAssetMenu(fileName = "MusicData", menuName = "ScriptableObjects/Data/MusicData", order = 0)]
    public class MusicData : ScriptableObject {
        
        public MusicTrack[] musicTracks;
        public int currentTrackIndex;
        public bool playing;
        
        public UnityEvent<bool> OnPressPlay;
        public void Play() {
            playing = !playing;
            OnPressPlay?.Invoke(playing);
        }
        
        public UnityEvent OnFastForward;
        public void FastForward() {
            OnFastForward?.Invoke();
        }
        
        public UnityEvent OnRewind;
        public void Rewind() {
            OnRewind?.Invoke();
        }
        
        [System.Serializable]
        public struct MusicTrack {
            public AudioClip clip;
            public int bpm;

            public MusicTrack(AudioClip clip, int bpm) {
                this.clip = clip;
                this.bpm = bpm;
            }
        }
    }
}