using TMPro;
using DG.Tweening;
using UnityEngine;

namespace FX {
    public class Popup : MonoBehaviour {
        
        // ANIMATION
        [SerializeField] private Transform xf;
        [SerializeField] private Vector2 startScale;
        [SerializeField] private Vector2 startPos;
        [SerializeField] private float animDuration;
        [SerializeField] private float scaleDuration = 0.25f;
        [SerializeField] private Ease animEase;
        // TEXT
        [SerializeField] private TextMeshPro textMesh;


        public void Init() {
            ResetPopUp();
            Animate();
        }

        public void Init(string newText) {
            textMesh.text = newText;
            ResetPopUp();
            Animate();
        }

        private void ResetPopUp() {
            xf = transform;
            xf.localScale = startScale;
            xf.localPosition = startPos;
            textMesh.alpha = 1;
        }

        private void Animate() {
            Sequence seq = DOTween.Sequence();
            seq.Append(xf.DOScale(Vector2.one, scaleDuration).SetEase(animEase)).
                AppendInterval(0.3f).
                Append(textMesh.DOFade(0, animDuration).OnComplete(SetInactive));
            
            xf.DOLocalMove(new Vector2(startPos.x, startPos.y + 0.75f), 
                animDuration).SetEase(animEase);
        }

        private void SetInactive() {
            gameObject.SetActive(false);
        }
    }
}