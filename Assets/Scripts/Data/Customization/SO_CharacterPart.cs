using UnityEngine;
using UnityEngine.Events;

namespace Data.Customization {
    [CreateAssetMenu(
        fileName = "NewCharacterPart", menuName = "ScriptableObjects/Customization/CharacterPart", order = 0)]
    public class SO_CharacterPart : ScriptableObject {

        // This is a communicator for each part of the character
        // Sends a message to the correct body part when we want to switch to another article of clothing etc
        // Could be used as a save game data collection too

        public SO_Item[] Items;
        public SO_Item CurrentItem;
        public SO_Item DefaultItem;
        
        public UnityEvent<SO_Item> OnChangeItem;
        public UnityEvent<SO_Item, Color> OnChangeItemColor;

        public void ChangeItem(SO_Item newItem) {
            CurrentItem ??= DefaultItem;
            CurrentItem.equipped = false;
            CurrentItem = newItem;
            CurrentItem.equipped = true;
            OnChangeItem?.Invoke(newItem);
            OnChangeItemColor?.Invoke(newItem, newItem.color);
        }

        public void ChangeItemColor(Color newColor) {
            CurrentItem.color = newColor;
            OnChangeItemColor?.Invoke(CurrentItem, newColor);
        }
    }
}