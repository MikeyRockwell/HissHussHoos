using UnityEngine;
using UnityEngine.UI;
using Data.Customization;

namespace UI.CustomiseMenu {
    public class TreatButton : MonoBehaviour {

        [SerializeField] private CustomiseEvents events;
        private Button button;
        private SO_Item item;

        private void Awake() {
            button = GetComponent<Button>();
            button.onClick.AddListener(SetTreat); 
        }

        public void InitButton(SO_Item newItem) {
            // Done when category is opened
            item = newItem;
            button.image.sprite = newItem.menuSprite;
        }

        private void SetTreat() {
            item.characterPart.ChangeItem(item);
            events.ChangeItem(item);
        }
    }
}