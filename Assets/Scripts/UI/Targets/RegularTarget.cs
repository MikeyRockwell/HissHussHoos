using Utils;
using TMPro;
using Managers;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class RegularTarget : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private int index;
        [SerializeField] private float resetDelayMult;
        [SerializeField] private Vector3 inactiveScale = new(0.9f, 0.9f, 0.9f);

        private DataWrangler.GameData gd;

        private Transform xf;
        private bool activeTarget;

        private void Awake()
        {
            // Cache transform
            xf = transform;
            // Wrangle data paths
            gd = DataWrangler.GetGameData();
            // Subscribe Events
            gd.eventData.OnHit.AddListener(CheckHit);
            gd.targetData.OnTargetsReset.AddListener(Init);
            gd.roundData.OnComboBegin.AddListener(OnComboBegin);
            gd.eventData.OnGameOver.AddListener(GameOver);
            // Reset scale to zero
            xf.localScale = Vector3.zero;
        }

        private void OnComboBegin(float time)
        {
            Init();
        }

        private void Init()
        {
            // Reset the target
            xf.DOKill();
            CheckActiveTarget();
            ScaleTextUp();
            RotateTextToCenter();
            SetText();
        }

        private void CheckStatus()
        {
            CheckActiveTarget();

            if (!activeTarget) return;

            ScaleTextUp();
            SetText();
        }

        private void CheckActiveTarget()
        {
            activeTarget = index == gd.targetData.step;
        }

        private void ScaleTextUp()
        {
            Vector3 targetScale = activeTarget ? Vector3.one : inactiveScale;
            xf.DOScale(targetScale, 0.1f).SetUpdate(true).OnComplete(StartPulse);
        }

        private void RotateTextToCenter()
        {
            xf.DORotate(Vector3.zero, 0.05f).SetUpdate(true);
        }

        private void SetText()
        {
            textMesh.text = gd.targetData.currentSet[index].ToString();
            Color targetColor = activeTarget ? RSColors.Green() : Color.white;
            textMesh.DOColor(targetColor, 0.2f).SetUpdate(true);
        }

        private void StartPulse()
        {
            if (!activeTarget) return;
            // Pulse loop
            xf.DOScale(
                xf.localScale + new Vector3(-0.1f, -0.1f, -0.1f), 0.5f)
                .SetUpdate(true)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void CheckHit(int step)
        {
            CheckStatus();

            if (step != index) return;

            Vector3 targetRotation = new(0, 0, xf.rotation.z + Random.Range(180f, 360f));
            textMesh.DOKill();
            textMesh.color = Color.yellow;
            xf.DOKill();
            xf.DORotate(targetRotation, 0.6f);
            xf.DOScale(Vector3.zero, 0.6f);
        }

        private void GameOver()
        {
            xf.DOKill();
            xf.localScale = Vector3.zero;
        }
    }
}