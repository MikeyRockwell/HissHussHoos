using System.Collections.Generic;
using UnityEngine;
using Data.Customization;
using DG.Tweening;
using Managers;
using UnityEngine.Serialization;

namespace UI.CustomiseMenu
{
    public class CustomizationMenu : MonoBehaviour
    {
        [SerializeField] private CustomizationEvents events;
        [SerializeField] private RectTransform itemGrid;
        [SerializeField] private RectTransform colorGrid;
        [SerializeField] private Ease colorAnimEase;

        private DataWrangler.GameData gd;
        private SO_CharacterPart currentPart;
        private Sequence colorAnim;

        private void Awake()
        {
            gd = DataWrangler.GetGameData();

            events.OnMenuOpened.AddListener(InitSubMenu);
            events.OnChangeCategory.AddListener(InitSubMenu);
            events.OnItemChanged.AddListener(InitializeColorGrid);
            events.OnItemUnlocked.AddListener(RefreshItemGrid);
            events.OnColorUnlocked.AddListener(RefreshColorGrid);
        }

        private void InitSubMenu(SO_CharacterPart part)
        {
            currentPart = part;
            InitializeItemGrid(part);
            InitializeColorGrid(part.CurrentItem);
        }

        private void RefreshItemGrid(SO_Item arg0)
        {
            InitializeItemGrid(currentPart);
        }

        private void InitializeItemGrid(SO_CharacterPart part)
        {
            DisableItemButtons();
            for (int i = 0; i < part.Items.Length; i++)
            {
                itemGrid.GetChild(i).gameObject.SetActive(true);
                itemGrid.GetChild(i).GetComponent<ItemSelectionButton>().InitButton(part.Items[i]);
            }
        }

        private void DisableItemButtons()
        {
            foreach (RectTransform child in itemGrid) child.gameObject.SetActive(false);
        }

        private void RefreshColorGrid()
        {
            InitializeColorGrid(currentPart.CurrentItem);
        }

        private void InitializeColorGrid(SO_Item item)
        {
            if (item.noColors)
            {
                DisableColorButtons();
                return;
            }

            // Loop through all colors in the all colors list and initialize the color buttons
            for (int i = 0; i < gd.itemData.allColors.Count; i++)
                colorGrid.GetChild(i).GetComponent<ColorButton>().Init(gd.itemData.allColors[i]);

            // AnimateColors();
        }

        private void DisableColorButtons()
        {
            foreach (RectTransform child in colorGrid)
            {
                child.localScale = Vector3.zero;
                child.gameObject.SetActive(false);
            }
        }
    }
}