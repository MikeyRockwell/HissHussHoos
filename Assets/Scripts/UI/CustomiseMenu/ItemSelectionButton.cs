using UnityEngine;
using UnityEngine.UI;
using Data.Customization;

namespace UI.CustomiseMenu
{
    public class ItemSelectionButton : MonoBehaviour
    {
        [SerializeField] private CustomizationEvents events;
        [SerializeField] private Button button;
        [SerializeField] private Image iconImage;
        [SerializeField] private Image lockedImage;

        public SO_Item item;

        private void Awake()
        {
            button.onClick.AddListener(SetTreat);
            events.OnItemChanged.AddListener(SetActiveSwitch);
            events.OnColorChanged.AddListener(ChangeColor);
        }

        // Called when the button is initialized
        public void InitButton(SO_Item newItem)
        {
            // Done when category is opened
            item = newItem;
            iconImage.sprite = newItem.menuSprite;
            iconImage.color = item.unlocked ? item.color : new Color(1, 1, 1, 0.1f);
            lockedImage.enabled = !item.unlocked;

            button.image.color = newItem.equipped ? button.colors.selectedColor : Color.clear;
        }

        // Called when the button is clicked
        private void SetTreat()
        {
            if (item == item.characterPart.CurrentItem) return;
            events.ChangeItem(item);
        }

        // Called when the item is changed
        private void SetActiveSwitch(SO_Item newItem)
        {
            button.image.color = newItem == item ? button.colors.selectedColor : Color.clear;
        }

        // Called when the color is changed
        private void ChangeColor(Color newColor)
        {
            if (gameObject.activeSelf == false) return;
            if (events.targetPart.CurrentItem != item) return;

            // if (newColor == item.color) return;
            iconImage.color = newColor;
        }
    }
}