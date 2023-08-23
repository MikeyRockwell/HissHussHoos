using Audio;
using DG.Tweening;
using UnityEngine;
using System.Collections;

namespace Managers {
    public class SceneManager : MonoBehaviour {
        [SerializeField] private Animator transitionTextAnimator;
        [SerializeField] private Animator openingTextAnimator;
        [SerializeField] private RectTransform leftPanel;
        [SerializeField] private RectTransform rightPanel;
        [SerializeField] private float animationTime = 2;
        [SerializeField] private Ease ease = Ease.InOutSine;
        [SerializeField] private SoundFXPlayer soundFX;

        private static readonly int Play = Animator.StringToHash("Play");

        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnGameInitComplete.AddListener(DelayAnimation);
        }

        private void DelayAnimation() {
            Invoke(nameof(OpenScene), 0.25f);
        }

        private void OpenScene() {
            soundFX.PlayRandomAudio();
            openingTextAnimator.SetTrigger(Play);
            leftPanel.DOPivotX(1.5f, 0.35f).SetEase(ease);
            rightPanel.DOPivotX(1.5f, 0.35f).SetEase(ease).OnComplete(() => gd.eventData.GameReady());
        }


        public void LoadScene(int sceneBuildIndex) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneBuildIndex);
        }

        public void SceneTransition(int sceneBuildIndex) {
            StartCoroutine(PlayTransitionAnimation(sceneBuildIndex));
        }

        private IEnumerator PlayTransitionAnimation(int sceneBuildIndex) {
            soundFX.PlayRandomAudio();
            transitionTextAnimator.gameObject.SetActive(true);
            leftPanel.DOPivotX(0.5f, 0.2f).SetEase(ease);
            rightPanel.DOPivotX(0.5f, 0.2f).SetEase(ease);

            yield return new WaitForSeconds(animationTime);

            LoadScene(sceneBuildIndex);
        }
    }
}