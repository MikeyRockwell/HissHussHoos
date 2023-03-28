using Data;
using DG.Tweening;
using UnityEngine;
using System.Collections;

namespace Managers {
    public class MusicManager : MonoBehaviour {
        
        [SerializeField] private bool musicEnabled;
        
        private float timer;
        
        public AudioSource audioSource;

        private DataWrangler.GameData gd;
        private MusicData md;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            md = gd.musicData;
            md.OnPressPlay.AddListener(Play);
            md.OnFastForward.AddListener(Forward);
            md.OnRewind.AddListener(Rewind);
        }

        private void Start() {
            md.currentTrackIndex = PlayerPrefs.GetInt("CurrentMusicTrack");
            md.playing = false;
            // md.Play();
        }

        private void Play(bool playing) {

            if (playing) {
                PlayMusicTrack(md.musicTracks[md.currentTrackIndex]);
            }
            else { PauseMusic(); }
        }
        
        private void Forward() {
            md.currentTrackIndex++;
            if (md.currentTrackIndex == gd.musicData.musicTracks.Length) {
                md.currentTrackIndex = 0;
            }
            PlayMusicTrack(md.musicTracks[md.currentTrackIndex]);
        }
        
        private void Rewind() {
           md. currentTrackIndex--;
            if (md.currentTrackIndex < 0) {
                md.currentTrackIndex = gd.musicData.musicTracks.Length - 1;
            }
            PlayMusicTrack(md.musicTracks[md.currentTrackIndex]);
        }

        private void PlayMusicTrack(MusicData.MusicTrack track) {

            SetAudioTrack(track.clip);
            StopAllCoroutines();
            StartCoroutine(Timer(track.clip.length - audioSource.time));
        }

        private void SetAudioTrack(AudioClip music) {
            
            audioSource.clip = music;
            audioSource.Play();
            // audioSource.DOFade(maxVolume, 1.5f);
        }

        private IEnumerator Timer(float time) {

            // Timer is the length of the song
            yield return new WaitForSecondsRealtime(time);
            Forward();
        }

        public void FadeMusicOut() {
            
            // Fade music and trigger OnMusicStopped event
            audioSource.DOFade(0, 2.5f);
            
            StopAllCoroutines();
        }

        private void PauseMusic() {
            audioSource.Pause();
            StopAllCoroutines();
        }

        private void OnDisable() {
            PlayerPrefs.SetInt("CurrentMusicTrack", md.currentTrackIndex);
        }
    }
}