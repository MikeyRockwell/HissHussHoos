using UnityEngine;
using UnityEngine.UI;
using Data.Customization;
using UnityEngine.Events;

namespace UI.CustomiseMenu {
    
    [CreateAssetMenu(
        fileName = "CustomiseMenuEvents", 
        menuName = "ScriptableObjects/UI/CustomiseMenuEvents", 
        order = 0)]
    
    public class CustomiseEvents : ScriptableObject {

        public UnityEvent<SO_CharacterPart> OnMenuOpened;    
        public UnityEvent<SO_CharacterPart> OnChangeCategory;
        public UnityEvent<Button> OnCategoryButtonPressed;
        public UnityEvent<SO_Item> OnItemSelected;
        public UnityEvent OnCloseOtherTabs;
        public SO_CharacterPart targetPart;
        public SO_CharacterPart defaultPart;
        
        public void OpenMenu() {
            OnMenuOpened?.Invoke(targetPart == null ? defaultPart : targetPart);
        }
        
        public void CloseOtherTabs() {
            OnCloseOtherTabs?.Invoke();
        }

        public void SelectClothingItem(SO_CharacterPart newPart, Button button) {
            OnCategoryButtonPressed?.Invoke(button);
            OnChangeCategory?.Invoke(newPart);
            targetPart = newPart;
        }

        public void ChangeItemColor(Color color) {
            targetPart.ChangeItemColor(color);
        }

        public void ChangeItem(SO_Item item) {
            OnItemSelected?.Invoke(item);
        }
    }
}