using Audio;
using Data;
using Managers;
using DG.Tweening;
using UnityEngine;
using TARGET = Data.TargetData.Target;

namespace UI {
    public class PrecisionGoal : MonoBehaviour {
        
        public TARGET type;
        public SoundFXPlayer sfx;
        
        [SerializeField] private float rotationSpeed = 1.0f;
        
        private DataWrangler.GameData gd;
        private Transform xf;
        private CircleCollider2D col2D;
        
        private void Awake() {
            gd = DataWrangler.GetGameData();
            col2D = GetComponent<CircleCollider2D>();
            gd.roundData.OnPrecisionRoundBegin.AddListener(Init);
            gd.roundData.OnPrecisionRoundComplete.AddListener(Disable);
            gd.eventData.OnPunchPrecision.AddListener(EnableCollider);
            xf = transform;
            Disable(0);
        }

        private void Init() {
            // Scale up using tween
            gameObject.SetActive(true);
            xf.DOScale(1.0f, 0.5f).SetEase(Ease.OutBack);
            col2D.enabled = false;
        }
        
        private void EnableCollider(TargetData.Target punchType) {
            if (punchType != type) return;
            col2D.enabled = true;
            // Switch off the collider after the punch speed time has elapsed
            Invoke(nameof(DisableCollider), gd.playerData.punchSpeed);
        }
        
        private void DisableCollider() {
            col2D.enabled = false;
        }

        private void Disable(int arg) {
            // Scale down using tween
            xf.DOScale(0.0f, 0.5f).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
        }

        private void Update() {
            // Slowly rotate the goal
            xf.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }
}