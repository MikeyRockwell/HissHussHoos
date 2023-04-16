using UnityEngine;
using UnityEngine.UI;
using Data.Customization;

namespace UI.CustomiseMenu {
    public class TreatButton : MonoBehaviour {

        [SerializeField] private CustomiseEvents events;
        [SerializeField] private Image iconImage;
        [SerializeField] private Button button;
        [SerializeField] private Color disabledColor;

        private SO_Item item;

        private void Awake() {
            button.onClick.AddListener(SetTreat); 
            events.OnItemChanged.AddListener(SetActiveSwitch);
        }

        public void InitButton(SO_Item newItem) {
            // Done when category is opened
            item = newItem;
            iconImage.sprite = newItem.menuSprite;
            SetActiveInit(newItem);
        }

        private void SetTreat() {
            item.characterPart.ChangeItem(item, true);
            events.ChangeItem(item);
        }

        private void SetActiveInit(SO_Item newItem) {
            button.image.color = newItem.equipped ? button.colors.selectedColor : Color.clear;
        }

        private void SetActiveSwitch(SO_Item newItem){
            button.image.color = newItem == item ? button.colors.selectedColor : Color.clear;
        }
    }
}