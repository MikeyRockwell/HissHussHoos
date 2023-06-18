using TMPro;
using Managers;
using UnityEngine;
using DG.Tweening;
using TARGET = Data.TargetData.Target;

namespace UI
{
    public class TimeAttackTarget : MonoBehaviour
    {
        // Time Attack Target
        // This target is spawned in the Time Attack round
        // It has a short timer before it disappears
        // It changes color based on how much time is left

        public TARGET type;

        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private RectTransform xf;
        [SerializeField] private float timeToKill = 1;
        [SerializeField] private Vector2 spawnPosition;

        private DataWrangler.GameData gd;

        private void Awake()
        {
            CacheReference();
        }

        private void CacheReference()
        {
            if (!ReferenceEquals(gd.roundData, null)) return;
            gd = DataWrangler.GetGameData();
        }

        public void Init(TARGET newType, int index)
        {
            timeToKill = index == 0 ? 2.5f : 1f;

            xf.localScale = Vector3.zero;
            xf.anchoredPosition = spawnPosition;
            xf.rotation = Quaternion.identity;
            type = newType;
            textMesh.text = type.ToString();

            gameObject.SetActive(true);
            xf.DOScale(Vector3.one, 0.2f);
            AnimateColor();
        }

        private void AnimateColor()
        {
            // Animate the color of the text from green to red
            textMesh.color = gd.uIData.LaserGreen;
            textMesh.DOColor(Color.red, timeToKill).SetEase(Ease.Linear).OnComplete(TimeOut);
        }

        private void TimeOut()
        {
            gd.roundData.TimeAttackTargetTimedOut();
            DisableSelfMiss();
        }

        public void DisableSelfHit()
        {
            textMesh.DOKill();
            xf.DORotate(Random.Range(-90, -180) * Vector3.forward, 0.8f);
            xf.DOJump(new Vector3(3, 0, 0), 1.5f, 1, 1f);
            xf.DOScale(Vector3.zero, 1f).OnComplete(() => gameObject.SetActive(false));
        }

        public void DisableSelfMiss()
        {
            textMesh.DOKill();
            xf.DORotate(Random.Range(90, 180) * Vector3.forward, 0.8f);
            xf.DOJump(new Vector3(-3, -3, 0), 3f, 1, 1f);
            xf.DOScale(Vector3.zero, 1f).OnComplete(() => gameObject.SetActive(false));
        }
    }
}