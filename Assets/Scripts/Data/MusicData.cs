using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(fileName = "MusicData", menuName = "ScriptableObjects/Data/MusicData", order = 0)]
    public class MusicData : ScriptableObject
    {
        public MusicTrack[] musicTracks;
        public int currentTrackIndex;
        public bool playing;

        public bool playByDefault;

        public UnityEvent<bool> OnPressPlay;

        public void Play()
        {
            playing = !playing;
            OnPressPlay?.Invoke(playing);
        }

        public UnityEvent OnFastForward;

        public void FastForward()
        {
            OnFastForward?.Invoke();
        }

        public UnityEvent OnRewind;

        public void Rewind()
        {
            OnRewind?.Invoke();
        }

        [Serializable]
        public struct MusicTrack
        {
            public string artist;
            [FormerlySerializedAs("name")] public string songName;
            public AudioClip clip;
            public int bpm;

            public MusicTrack(string artist, string songName, AudioClip clip, int bpm)
            {
                this.artist = artist;
                this.songName = songName;
                this.clip = clip;
                this.bpm = bpm;
            }
        }
    }
}