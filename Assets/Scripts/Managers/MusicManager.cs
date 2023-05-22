using Data;
using DG.Tweening;
using UnityEngine;
using System.Collections;
using TMPro;

namespace Managers {
    public class MusicManager : MonoBehaviour {
        
        private float timer;
        
        public AudioSource audioSource;
        
        [SerializeField] private TextMeshProUGUI artistNameText;
        [SerializeField] private TextMeshProUGUI trackNameText;
        [SerializeField] private RectTransform trackText;

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
            if (md.playByDefault) {
                md.Play();
            }
        }

        private void Play(bool playing) {
            if (playing) {
                PlayMusicTrack(md.musicTracks[md.currentTrackIndex]);
            }
            else { PauseMusic(); }
        }
        
        // TODO Put this in a class on the text object
        private void DisplayTrackName() {
            
            // Display track name animation
            DOTween.Kill(trackText);
            trackText.localScale = Vector3.zero;
            
            MusicData.MusicTrack currentTrack = md.musicTracks[md.currentTrackIndex];

            string artistColor = ColorUtility.ToHtmlStringRGBA(gd.uIData.Gold);
            string trackNameColor = ColorUtility.ToHtmlStringRGBA(gd.uIData.HotPink);

            artistNameText.text = "<color=#" + artistColor + ">" + currentTrack.artist; 
            trackNameText.text = "<color=#" + trackNameColor + ">" + currentTrack.songName;
            
            Sequence seq = DOTween.Sequence(trackText);
            seq.Append(trackText.DOScale(2, 0.5f).SetEase(Ease.OutBack));
            seq.AppendInterval(5f);
            seq.Append(trackText.DOScale(0, 0.5f).SetEase(Ease.InBack));                
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

            DisplayTrackName();
            SetAudioTrack(track.clip);
            StopAllCoroutines();
            StartCoroutine(Timer(track.clip.length - audioSource.time));
        }

        private void SetAudioTrack(AudioClip music) {
            audioSource.clip = music;
            audioSource.Play();
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