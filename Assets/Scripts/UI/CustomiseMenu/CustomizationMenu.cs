﻿using UnityEngine;
using Data.Customization;
using Managers;

namespace UI.CustomiseMenu {
    public class CustomizationMenu : MonoBehaviour {

        [SerializeField] private CustomizationEvents events;
        [SerializeField] private RectTransform itemGrid;
        [SerializeField] private RectTransform colorGrid;

        private DataWrangler.GameData gd;
        private SO_CharacterPart currentPart;

        private void Awake() {

            gd = DataWrangler.GetGameData();
            
            events.OnMenuOpened.AddListener(InitSubMenu);
            events.OnChangeCategory.AddListener(InitSubMenu);
            events.OnItemChanged.AddListener(InitializeColorGrid);
            events.OnItemUnlocked.AddListener(RefreshItemGrid);
        }

        private void InitSubMenu(SO_CharacterPart part) {
            currentPart = part;
            InitializeItemGrid(part);
            InitializeColorGrid(part.CurrentItem);
        }
        private void RefreshItemGrid(SO_Item arg0) {
            InitializeItemGrid(currentPart);
        }
        
        private void InitializeItemGrid(SO_CharacterPart part) {
            
            DisableItemButtons();
            for (int i = 0; i < part.Items.Length; i++) {
                itemGrid.GetChild(i).gameObject.SetActive(true);
                itemGrid.GetChild(i).GetComponent<ItemSelectionButton>().InitButton(part.Items[i]);
            }
        }

        
        private void DisableItemButtons() {
            foreach (RectTransform child in itemGrid) {
                child.gameObject.SetActive(false);
            }
        }
        
        private void InitializeColorGrid(SO_Item item) {
            
            DisableColorButtons();

            Color[] colors = null;
            
            if (item.noColors) return;

            if (item.standardColors) {
                colors = gd.colorData.defaultClothingColors;
            }
            
            if (item.customColors) {
                colors = item.availableColors;
            }

            for (int i = 0; i < colors.Length; i++) {
                colorGrid.GetChild(i).gameObject.SetActive(true);
                colorGrid.GetChild(i).GetComponent<ClothingColorChanger>().Init(colors[i]);
            }
        }
        
        private void DisableColorButtons() {
            foreach (RectTransform child in colorGrid) {
                child.gameObject.SetActive(false);
            }
        }
    }
}