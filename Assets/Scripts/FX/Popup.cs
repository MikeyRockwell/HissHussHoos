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
        [SerializeField] private float riseHeight = 0.25f;
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
        
        public void Init(string newText, float time) {
            textMesh.text = newText;
            ResetPopUp();
            Animate();
        }
        
        public void Init(string newText, Color newColor) {
            textMesh.text = newText;
            textMesh.color = newColor;
            ResetPopUp();
            Animate();
        }

        private void ResetPopUp() {
            // Cache the transform is it is null
            if (xf == null) xf = transform;
            xf.DOKill();
            xf.localScale = startScale;
            xf.localPosition = startPos;
            textMesh.alpha = 1;
        }

        private void Animate() {
            Sequence seq = DOTween.Sequence();
            seq.Append(xf.DOScale(Vector2.one, scaleDuration).SetEase(animEase)).
                AppendInterval(0.3f).
                Append(textMesh.DOFade(0, animDuration).OnComplete(SetInactive));
            
            xf.DOLocalMove(new Vector2(startPos.x, startPos.y + riseHeight), 
                animDuration).SetEase(animEase);
        }

        private void SetInactive() {
            gameObject.SetActive(false);
        }
    }
}