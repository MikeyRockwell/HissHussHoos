using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace FX {
    public class RoundNumberPopUp : MonoBehaviour {
        
        // This class plays the animation and then deactivates the game object
        
        [SerializeField] private RectTransform rect;
        [SerializeField] private float animationLength;
        [SerializeField] private TextMeshPro roundNumber;
        [SerializeField] private Vector2 startScale;
        [SerializeField] private Vector2 endScale;
        [SerializeField] private Vector2 startPos;
        [SerializeField] private Vector2 endPos;

        public void Init(int round) {
            roundNumber.text = round.ToString();
            gameObject.SetActive(true);
            rect.localScale = startScale;
            StartCoroutine(nameof(Animate));
        }

        private IEnumerator Animate() {
            yield return new WaitForSeconds(0.5f);
            rect.DOScale(endScale, 0.2f).SetEase(Ease.OutBounce);
            // rect.DOMove(endPos, 0.2f);
            yield return new WaitForSeconds(1.2f);
            Deactivate();
        }
        
        private void Deactivate() {
            rect.DOScale(startScale, 0.1f).OnComplete(()=>gameObject.SetActive(false));
        }
        
    }
}