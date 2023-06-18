using System;
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
        [SerializeField] private AudioSource animationSound;

        private Image image;
        private SO_Color color;
        public int index;

        private void Awake()
        {
            index = transform.GetSiblingIndex();
            animationSound.pitch = 1 + index * 0.1f;
            image = GetComponent<Image>();
            button.onClick.AddListener(ChangeColor);
        }

        public void Init(SO_Color col)
        {
            lockedImage.enabled = !col.unlocked;

            if (color == col && gameObject.activeSelf == true) return;

            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            color = col;
            image.color = col.Color;
            transform.DOKill();
            CancelInvoke();
            float delay = (index + 1) * 0.05f;
            Invoke(nameof(PlayAnimation), delay);
        }

        private void ChangeColor()
        {
            if (color.Color == events.targetPart.CurrentItem.color) return;
            events.ChangeItemColor(color);
        }

        public void PlayAnimation()
        {
            transform.DOScale(1, 0.1f).SetEase(Ease.OutBack);
            animationSound.PlayOneShot(animationSound.clip);
        }
    }
}