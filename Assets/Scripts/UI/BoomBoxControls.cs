using Managers;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

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

        private bool controlsOpen;
        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            boomBox.onClick.AddListener(OpenControls);
            playButton.onClick.AddListener(Play);
            fastForwardButton.onClick.AddListener(FastForward);
            rewindButton.onClick.AddListener(Rewind);
        }

        private void OpenControls() {
            if (!controlsOpen) {
                controls.DOKill();
                controls.DOLocalMoveY(ctrlOpenY, 0.3f).
                    SetEase(animEase).OnComplete(() => controlsOpen = true);
            }
            else {
                controls.DOKill();
                controls.DOLocalMoveY(ctrlClosedY, 0.3f).
                    SetEase(animEase).OnComplete(() => controlsOpen = false);;
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