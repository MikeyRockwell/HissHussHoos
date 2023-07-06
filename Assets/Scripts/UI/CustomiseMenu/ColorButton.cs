using System;
using System.Collections;
using Data.Customization;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CustomiseMenu
{
    public class ColorButton : MonoBehaviour
    {
        [SerializeField] private CustomizationEvents events;
        [SerializeField] private Button button;
        [SerializeField] private Image lockedImage;
        [SerializeField] private Image colorImage;
        [SerializeField] private AudioSource animationSound;

        private SO_Color color;
        private Sequence sequence;
        public int index;

        private void Awake()
        {
            index = transform.GetSiblingIndex();
            animationSound.pitch = 1 + index * 0.1f;
            button.onClick.AddListener(ChangeColor);
            button.onClick.AddListener(ColorButtonPressed);
        }

        public void Init(SO_Color col)
        {
            lockedImage.enabled = !col.unlocked;

            if (color == col && gameObject.activeSelf) return;
            
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            
            color = col;
            colorImage.color = col.Color;
            sequence.Kill();

            float delay = (index + 1) * 0.05f;
            sequence = DOTween.Sequence();
            sequence.PrependInterval(delay).SetUpdate(true).OnComplete(PlayAnimation);
        }

        private void ChangeColor()
        {
            // if (color.Color == events.targetCategory.CurrentItem.color) return;
            
            events.ChangeItemColor(events.GetTargetItem(), color);
        }

        private void ColorButtonPressed()
        {
            // if (color.Color == events.targetCategory.CurrentItem.color) return;

            events.ColorButtonPressed(color);
        }

        public void PlayAnimation()
        {   
            transform.DOScale(1, 0.1f).SetEase(Ease.OutBack).SetUpdate(true);
            animationSound.PlayOneShot(animationSound.clip);
        }
    }
}