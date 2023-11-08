using Utils;
using Managers;
using UnityEngine;
using DG.Tweening;
using Data.Customization;

namespace UI.CustomiseMenu {
    public class CustomizationMenu : MonoBehaviour {
        [SerializeField] private CustomizationEvents events;
        [SerializeField] private RectTransform itemGrid;
        [SerializeField] private RectTransform colorGrid;
        [SerializeField] private SO_Category defaultCategory;

        private DataWrangler.GameData gd;
        private SO_Category currentCategory;
        private Sequence colorAnim;
        private CustomizationMenuWindow window;

        private void Awake() {
            gd = DataWrangler.GetGameData();

            events.OnMenuOpened.AddListener(InitSubMenu);
            events.OnMenuClosed.AddListener(ResetItems);
            events.OnChangeCategory.AddListener(InitSubMenu);
            events.OnItemChanged.AddListener(InitializeColorGrid);
            events.OnItemUnlocked.AddListener(RefreshItemGrid);
            events.OnColorUnlocked.AddListener(RefreshColorGrid);

            window = GetComponent<CustomizationMenuWindow>();
        }

        private void Start() {
            events.targetCategory = defaultCategory;
        }

        private void InitSubMenu(SO_Category category) {
            currentCategory = category;
            // if (!window.isOpen) return;

            InitializeItemGrid(category);
            InitializeColorGrid(category.CurrentItem);
        }

        private void RefreshItemGrid(SO_Item arg0) {
            InitializeItemGrid(currentCategory);
        }

        private void InitializeItemGrid(SO_Category category) {
            Log.Message("Initializing item grid");
            DisableItemButtons();
            for (int i = 0; i < category.Items.Length; i++) {
                // Check if the item is related to this character
                if (!category.isCharacter && category.Items[i].character != gd.characterData.currentCharacter
                                          && category.Items[i].character != CharacterData.Character.None) continue;

                // If so then enable the button and initialize it
                itemGrid.GetChild(i).gameObject.SetActive(true);
                itemGrid.GetChild(i).GetComponent<ItemSelectionButton>().InitButton(category.Items[i]);
                // if (category.Items[i] == category.CurrentItem) {
                //     itemGrid.GetChild(i).GetComponent<ItemSelectionButton>().SetActiveSwitch(category.CurrentItem);
                // }
            }
        }

        private void DisableItemButtons() {
            foreach (RectTransform child in itemGrid) child.gameObject.SetActive(false);
        }

        private void RefreshColorGrid(SO_Color color) {
            SO_Item item = events.GetTargetItem();
            InitializeColorGrid(item);
        }

        private void InitializeColorGrid(SO_Item item) {
            if (item.noColors) {
                DisableColorButtons();
                return;
            }

            Log.Message("Initializing color grid");
            // Check if the item is related to this character
            if (!item.category.isCharacter && item.character != gd.characterData.currentCharacter) return;

            // Loop through all colors in the all colors list and initialize the color buttons
            for (int i = 0; i < gd.itemData.allColors.Count; i++)
                // If so then enable the button and initialize it
                // colorGrid.GetChild(i).gameObject.SetActive(true);
                colorGrid.GetChild(i).GetComponent<ColorButton>().Init(gd.itemData.allColors[i]);
        }

        private void DisableColorButtons() {
            foreach (RectTransform child in colorGrid) {
                child.localScale = Vector3.zero;
                child.gameObject.SetActive(false);
            }
        }

        private void ResetItems() {
            foreach (SO_Category cat in gd.itemData.allCategories) cat.ChangeItem(cat.CurrentItem, true);
        }
    }
}