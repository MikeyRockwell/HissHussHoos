using Audio;
using Utils;
using Managers;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace UI {
    public class BoomBoxControls : MonoBehaviour {
        [SerializeField] private Button boomBox;
        [SerializeField] private Button playButton;
        [SerializeField] private Button rewindButton;
        [SerializeField] private Button fastForwardButton;
        [SerializeField] private RectTransform controls;
        [SerializeField] private float ctrlOpenY;
        [SerializeField] private float ctrlClosedY;
        [SerializeField] private Ease animEase;
        [SerializeField] private Slider slider;
        [SerializeField] private AudioMixer musicMixer;
        [SerializeField] private SoundFXPlayer openSFX;
        [SerializeField] private SoundFXPlayer closeSFX;

        private bool controlsOpen;
        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            boomBox.onClick.AddListener(OpenControls);
            playButton.onClick.AddListener(Play);
            fastForwardButton.onClick.AddListener(FastForward);
            rewindButton.onClick.AddListener(Rewind);
            slider.onValueChanged.AddListener(SetMusicVolume);
        }

        private void Start() {
            float volume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);

            SetMusicVolume(volume);
            slider.value = volume;
            Log.Message("Volume level loaded = " + slider.value, Color.yellow);
        }

        private void SetMusicVolume(float volume) {
            musicMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }

        private void OpenControls() {
            if (!controlsOpen) {
                openSFX.PlayRandomAudio();
                controls.DOKill();
                controls.DOLocalMoveY(ctrlOpenY, 0.3f).SetEase(animEase).OnComplete(() => controlsOpen = true);
            }
            else {
                closeSFX.PlayRandomAudio();
                controls.DOKill();
                controls.DOLocalMoveY(ctrlClosedY, 0.3f).SetEase(animEase).OnComplete(() => controlsOpen = false);
                ;
            }
        }

        private void Play() {
            gd.musicData.Play();
        }

        private void FastForward() {
            gd.musicData.FastForward();
        }

        private void Rewind() {
            gd.musicData.Rewind();
        }
    }
}