using Data;
using Utils;
using TMPro;
using Managers;
using DG.Tweening;
using UnityEngine;

namespace UI {
    public class RegularTarget : MonoBehaviour {
        
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private int index;
        [SerializeField] private Vector3 inactiveScale = new(0.9f, 0.9f, 0.9f);
        [SerializeField] private Transform targetPool;

        private DataWrangler.GameData gd;
        private Transform xf;
        private bool currentTarget;
        private bool active;

        private void Awake() {
            // Cache transform
            xf = transform;
            // Wrangle data paths
            gd = DataWrangler.GetGameData();
            // Subscribe Events
            gd.eventData.OnHit.AddListener(CheckHit);
            // gd.targetData.OnTargetsReset.AddListener(Init);
            // gd.roundData.OnComboBegin.AddListener(OnComboBegin);
            gd.eventData.OnGameOver.AddListener(GameOver);
            // Reset scale to zero
            xf.localScale = Vector3.zero;
        }

        /*private void OnComboBegin(float time) {
            Init();
        }*/

        public void Init(int newIndex) {
            // Reset the target
            gameObject.SetActive(true);
            active = true;
            index = newIndex;
            xf.DOKill();
            CheckActiveTarget();
            ScaleTextUp();
            RotateTextToCenter();
            SetText();
        }

        private void CheckStatus() {
            CheckActiveTarget();
            if (!currentTarget) return;
            ScaleTextUp();
            SetText();
        }

        private void CheckActiveTarget() {
            currentTarget = index == gd.targetData.step;
        }

        private void ScaleTextUp() {
            Vector3 targetScale = currentTarget ? Vector3.one : inactiveScale;
            xf.DOScale(targetScale, 0.1f).SetUpdate(true).OnComplete(StartPulse);
        }

        private void RotateTextToCenter() {
            xf.DORotate(Vector3.zero, 0.05f).SetUpdate(true);
        }

        private void SetText() {
            textMesh.text = gd.targetData.currentSet[index].ToString();
            Color targetColor = currentTarget ? RSColors.Green() : Color.white;
            textMesh.DOColor(targetColor, 0.2f).SetUpdate(true);
        }

        private void StartPulse() {
            if (!currentTarget) return;
            // Pulse loop
            xf.DOScale(
                    xf.localScale + new Vector3(-0.1f, -0.1f, -0.1f), 0.5f)
                .SetUpdate(true)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void CheckHit(int step) {
            if (!active) return;
            
            CheckStatus();

            if (step != index) return;
            active = false;
            Vector3 targetRotation = new(0, 0, xf.rotation.z + Random.Range(180f, 360f));
            textMesh.DOKill();
            textMesh.DOColor(gd.uIData.HotPink, 0.5f).SetUpdate(true);
            textMesh.DOFade(0, 0.5f).SetUpdate(true);
            xf.DOKill();
            xf.DORotate(targetRotation, 0.6f);
            xf.DOMove(xf.position + new Vector3(0.75f, 0.25f), 0.6f).SetUpdate(true);
            xf.DOScale(Vector3.zero, 0.6f).OnComplete(DisableTarget);
        }

        private void DisableTarget() {
            xf.SetParent(targetPool);
            gameObject.SetActive(false);
        }

        private void GameOver() {
            xf.DOKill();
            xf.localScale = Vector3.zero;
            DisableTarget();
            
        }
    }
}