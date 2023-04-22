using UnityEngine;
using UnityEngine.UI;
using Data.Customization;
using UnityEngine.Events;

namespace UI.CustomiseMenu {
    
    [CreateAssetMenu(
        fileName = "CustomiseMenuEvents", 
        menuName = "ScriptableObjects/UI/CustomiseMenuEvents", 
        order = 0)]
    
    // This is a scriptable object that holds all the events for the customisation menu
    public class CustomizationEvents : ScriptableObject {
        
        // These are all the events that can be triggered by the customisation menu
        public UnityEvent<SO_CharacterPart> OnMenuOpened;    
        public UnityEvent OnMenuClosed;    
        public UnityEvent<SO_CharacterPart> OnChangeCategory;
        public UnityEvent<Button> OnCategoryButtonPressed;
        public UnityEvent<SO_Item> OnLockedItemPressed;
        public UnityEvent<SO_Item> OnItemUnlocked;
        public UnityEvent<SO_Item> OnItemChanged;
        public UnityEvent<Color> OnColorChanged;
        
        // This is the part that is currently being customised
        public SO_CharacterPart targetPart;
        
        // This is the default part that is opened when the menu is opened
        public SO_CharacterPart defaultPart;
        
        public void OpenMenu() {
            OnMenuOpened?.Invoke(targetPart == null ? defaultPart : targetPart);
        }

        public void CloseMenu() {
            OnMenuClosed?.Invoke();
        }

        public void SelectClothingItem(SO_CharacterPart newPart, Button button) {
            // When a new part is selected, trigger the change category event
            OnCategoryButtonPressed?.Invoke(button);
            OnChangeCategory?.Invoke(newPart);
            // Set the target part to the new part
            targetPart = newPart;
        }

        public void ChangeItemColor(Color color) {
            // Change the colour of the target part
            targetPart.ChangeItemColor(color, true);
            // Trigger the colour changed event
            OnColorChanged?.Invoke(color);
        }

        public void ChangeItem(SO_Item item) {
            // If item is locked, trigger the locked item event
            if (!item.unlocked) {
                OnLockedItemPressed?.Invoke(item);
                return;
            }
            // Change the item of the target part
            item.characterPart.ChangeItem(item, true);
            OnItemChanged?.Invoke(item);
        }

        public void UnlockItem(SO_Item item) {
            // Trigger the item unlocked event
            item.unlocked = true;
            OnItemUnlocked?.Invoke(item);
        }
    }
}