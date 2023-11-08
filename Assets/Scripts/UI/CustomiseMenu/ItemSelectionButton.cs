using UnityEngine;
using UnityEngine.UI;
using Data.Customization;
using UnityEngine.Serialization;
using Utils;

namespace UI.CustomiseMenu {
    public class ItemSelectionButton : MonoBehaviour {
        [SerializeField] private CustomizationEvents events;
        [SerializeField] private Button button;
        [SerializeField] private Image iconImage;
        [SerializeField] private Image lockedImage;

        public SO_Item item;

        [SerializeField] Material defaultMaterial;
        private Material customMat;
        private bool initialized;

        private void Initialize() {
            button.onClick.AddListener(SetTreat);
            events.OnItemChanged.AddListener(SetActiveSwitch);
            events.OnColorChanged.AddListener(ChangeColor);
            defaultMaterial = iconImage.material;
            initialized = true;
        }

        private void InitMaterial() {
            
        }

        // Called when the button is initialized
        public void InitButton(SO_Item newItem) {
            
            if (!initialized) Initialize();
            
            // Done when category is opened
            item = newItem;
            iconImage.sprite = newItem.menuSprite;
            iconImage.color = item.color;
            lockedImage.enabled = !item.unlocked;
            

            button.image.color = newItem.equipped ? button.colors.selectedColor : Color.clear;
            if (newItem.equipped) {
                Log.Message("Equipped: " + item.name + " in " + item.category.name, item.color);
                events.ChangeItem(item);
            }

            if (newItem.customShader) {
                customMat = newItem.customMaterial;
                iconImage.material = item.customIconMaterial;
            }
            else {
                iconImage.material = defaultMaterial;
            }
        }

        // Called when the button is clicked
        private void SetTreat() {
            if (item == item.category.CurrentItem && item == item.category.TryingItem) return;
            events.ChangeItem(item);
        }

        // Called when the item is changed
        public void SetActiveSwitch(SO_Item newItem) {
            button.image.color = newItem == item ? button.colors.selectedColor : Color.clear;
        }

        // Called when the color is changed
        private void ChangeColor(SO_Item targetItem, Color newColor) {
            if (gameObject.activeSelf == false) return;
            // if (events.targetCategory.CurrentItem == item && events.TryingOnItem) return;
            // if (events.targetCategory.CurrentItem != item && events.targetCategory.TryingItem != item) return;
            // if (!item.equipped && item != item.category.TryingItem) return;
            if (item != targetItem) return;
            // if (newColor == item.color) return;
            iconImage.color = newColor;
        }
    }
}