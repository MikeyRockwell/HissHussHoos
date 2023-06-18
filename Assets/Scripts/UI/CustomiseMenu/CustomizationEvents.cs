using UnityEngine;
using UnityEngine.UI;
using Data.Customization;
using UnityEngine.Events;

namespace UI.CustomiseMenu
{
    [CreateAssetMenu(
        fileName = "CustomiseMenuEvents",
        menuName = "ScriptableObjects/UI/CustomiseMenuEvents",
        order = 0)]

    // This is a scriptable object that holds all the events for the customisation menu
    public class CustomizationEvents : ScriptableObject
    {
        // These are all the events that can be triggered by the customisation menu
        public UnityEvent<SO_CharacterPart> OnMenuOpened;
        public UnityEvent OnMenuClosed;
        public bool MenuOpen;
        public UnityEvent<SO_CharacterPart> OnChangeCategory;
        public UnityEvent<Button> OnCategoryButtonPressed;
        public UnityEvent<SO_Item> OnLockedItemPressed;
        public UnityEvent<SO_Color> OnLockedColorPressed;
        public UnityEvent<SO_Item> OnItemUnlocked;
        public UnityEvent OnColorUnlocked;
        public UnityEvent<SO_Item> OnItemChanged;
        public UnityEvent<Color> OnColorChanged;

        // This is the part that is currently being customised
        public SO_CharacterPart targetPart;

        // This is the default part that is opened when the menu is opened
        public SO_CharacterPart defaultPart;

        public void OpenMenu()
        {
            OnMenuOpened?.Invoke(targetPart == null ? defaultPart : targetPart);
            MenuOpen = true;
        }

        public void CloseMenu()
        {
            OnMenuClosed?.Invoke();
            MenuOpen = false;
        }

        public void SelectClothingItem(SO_CharacterPart newPart, Button button)
        {
            // When a new part is selected, trigger the change category event
            OnCategoryButtonPressed?.Invoke(button);
            OnChangeCategory?.Invoke(newPart);
            // Set the target part to the new part
            targetPart = newPart;
        }

        public void ChangeItemColor(SO_Color color)
        {
            // If the color is locked, trigger the locked item event
            if (!color.unlocked)
            {
                OnLockedColorPressed?.Invoke(color);
                return;
            }

            // Change the colour of the target part
            targetPart.ChangeItemColor(color.Color, true);
            // Trigger the colour changed event
            OnColorChanged?.Invoke(color.Color);
        }

        public void ChangeItem(SO_Item item)
        {
            // If item is locked, trigger the locked item event
            if (!item.unlocked)
            {
                OnLockedItemPressed?.Invoke(item);
                return;
            }

            // Change the item of the target part
            item.characterPart.ChangeItem(item, true);
            OnItemChanged?.Invoke(item);
        }

        public void UnlockItem(SO_Item item)
        {
            // Trigger the item unlocked event
            item.unlocked = true;
            OnItemUnlocked?.Invoke(item);
            ChangeItem(item);
        }

        public void UnlockItem(SO_Color color)
        {
            color.unlocked = true;
            OnColorUnlocked?.Invoke();
            ChangeItemColor(color);
        }
    }
}