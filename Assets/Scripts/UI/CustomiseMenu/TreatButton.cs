using UnityEngine;
using UnityEngine.UI;
using Data.Customization;

namespace UI.CustomiseMenu {
    public class TreatButton : MonoBehaviour {

        [SerializeField] private CustomiseEvents events;
        [SerializeField] private Image iconImage;
        [SerializeField] private Button button;
        private SO_Item item;

        private void Awake() {
            button.onClick.AddListener(SetTreat); 
        }

        public void InitButton(SO_Item newItem) {
            // Done when category is opened
            item = newItem;
            iconImage.sprite = newItem.menuSprite;
        }

        private void SetTreat() {
            item.characterPart.ChangeItem(item, true);
            events.ChangeItem(item);
        }
    }
}