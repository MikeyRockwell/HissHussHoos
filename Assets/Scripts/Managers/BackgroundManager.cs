using System;
using UnityEngine;
using UnityEngine.UI;
using Data.Customization;

namespace Managers
{
    public class BackgroundManager : MonoBehaviour
    {
        [SerializeField] private SO_Category backgroundPart;
        [SerializeField] private SpriteRenderer bgImage;
        [SerializeField] private Transform fieldingFlag;

        private void Awake()
        {
            backgroundPart.OnChangeItem.AddListener(ChangeBackground);
        }

        private void Start()
        {
            ChangeBackground(backgroundPart.CurrentItem);
        }

        private void ChangeBackground(SO_Item newBG)
        {
            bgImage.sprite = newBG.bgSprite;
            fieldingFlag.gameObject.SetActive(newBG.hasFlag);
        }
    }
}