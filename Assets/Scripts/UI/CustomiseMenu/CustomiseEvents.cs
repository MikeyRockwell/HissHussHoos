using UnityEngine;
using UnityEngine.UI;
using Data.Customization;
using UnityEngine.Events;
using Utils;

namespace UI.CustomiseMenu {
    
    [CreateAssetMenu(
        fileName = "CustomiseMenuEvents", 
        menuName = "ScriptableObjects/UI/CustomiseMenuEvents", 
        order = 0)]
    
    public class CustomiseEvents : ScriptableObject {

        public UnityEvent<SO_CharacterPart> OnMenuOpened;    
        public UnityEvent OnMenuClosed;    
        public UnityEvent<SO_CharacterPart> OnChangeCategory;
        public UnityEvent<Button> OnCategoryButtonPressed;
        public UnityEvent<SO_Item> OnItemChanged;
        public UnityEvent<Color> OnColorChanged;
        public SO_CharacterPart targetPart;
        public SO_CharacterPart defaultPart;
        
        public void OpenMenu() {
            OnMenuOpened?.Invoke(targetPart == null ? defaultPart : targetPart);
        }

        public void CloseMenu() {
            // This can be called by clicking a close button
            // Or maybe tapping outside the menu
            Log.Message("Closing Menu!");
            OnMenuClosed?.Invoke();
        }

        public void SelectClothingItem(SO_CharacterPart newPart, Button button) {
            OnCategoryButtonPressed?.Invoke(button);
            OnChangeCategory?.Invoke(newPart);
            targetPart = newPart;
        }

        public void ChangeItemColor(Color color) {
            targetPart.ChangeItemColor(color, true);
            OnColorChanged?.Invoke(color);
        }

        public void ChangeItem(SO_Item item) {
            OnItemChanged?.Invoke(item);
        }
    }
}