using System;
using UnityEngine;
using Data.Customization;

namespace Managers {
    public class BackgroundManager : MonoBehaviour {
        
        [SerializeField] private SO_Category backgroundPart;
        [SerializeField] private SpriteRenderer bgImage;
        [SerializeField] private Transform fieldingFlag;
        [SerializeField] private Transform taranakiLayers;

        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnGameInitComplete.AddListener(Init);
            backgroundPart.OnChangeItem.AddListener(ChangeBackground);
        }

        private void Init() {
            ChangeBackground(backgroundPart.CurrentItem);
        }

        private void ChangeBackground(SO_Item newBG) {
            bgImage.sprite = newBG.bgSprite;
            switch (newBG.bgType) {
                case SO_Item.BackgroundType.gym:
                    fieldingFlag.gameObject.SetActive(false);
                    taranakiLayers.gameObject.SetActive(false);
                    break;
                case SO_Item.BackgroundType.fielding:
                    fieldingFlag.gameObject.SetActive(true);
                    taranakiLayers.gameObject.SetActive(false);
                    break;
                case SO_Item.BackgroundType.taranaki:
                    fieldingFlag.gameObject.SetActive(false);
                    taranakiLayers.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }           
        }
    }
}