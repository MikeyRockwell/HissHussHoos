using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(Image))]
    public class ImagePulse : MonoBehaviour {
        [SerializeField] private float offset;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Image image;

        private Sequence sequence;

        private void OnEnable() {
            Color targetColor = new(0.75f, 0.5f, .75f, 1);
            sequence = DOTween.Sequence();
            sequence.AppendInterval(offset).SetUpdate(true);
            sequence.Append(image.DOColor(targetColor, 1f).SetUpdate(true).SetLoops(-1, LoopType.Yoyo));
        }

        private void OnDisable() {
            sequence.Kill();
            image.color = defaultColor;
        }
    }
}