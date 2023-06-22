using UnityEngine;
using UnityEngine.UI;
using Data.Customization;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Utils;

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
        public UnityEvent<SO_Category> OnMenuOpened;
        public UnityEvent OnMenuClosed;
        public bool MenuOpen;
        public UnityEvent<SO_Category> OnChangeCategory;
        public UnityEvent<Button> OnCategoryButtonPressed;
        public UnityEvent<SO_Item> OnLockedItemPressed;
        public UnityEvent<SO_Color> OnColorButtonPressed;
        public UnityEvent<SO_Item> OnItemUnlocked;
        public UnityEvent OnColorUnlocked;
        public UnityEvent<SO_Item> OnItemChanged;
        public UnityEvent<SO_Item, Color> OnColorChanged;

        public SO_Category targetCategory;
        public bool TryingOnItem;
        public bool TryingOnColor;

        public void OpenMenu()
        {
            OnMenuOpened?.Invoke(targetCategory);
            MenuOpen = true;
        }

        public void CloseMenu()
        {
            OnMenuClosed?.Invoke();
            MenuOpen = false;
        }

        public void SelectClothingItem(SO_Category newPart, Button button)
        {
            // When a new part is selected, trigger the change category event
            OnCategoryButtonPressed?.Invoke(button);
            OnChangeCategory?.Invoke(newPart);
            // Set the target part to the new part
            targetCategory = newPart;
        }

        public void ChangeItemColor(SO_Item item, SO_Color color)
        {
            if (TryingOnItem)
            {
                TryingOnColor = true;
                targetCategory.TryOnColorLockedItem(color.Color);
                OnColorChanged?.Invoke(item, color.Color);
                return;
            }

            if (!color.unlocked && !TryingOnItem)
            {
                TryingOnColor = true;
                targetCategory.TryOnColorUnlockedItem(color.Color);
                OnColorChanged?.Invoke(item, color.Color);
                return;
            }

            TryingOnColor = false;
            // Change the colour of the target part
            targetCategory.ChangeItemColor(color.Color, true);
            // Trigger the colour changed event
            OnColorChanged?.Invoke(item, color.Color);
        }

        public void ChangeItem(SO_Item item)
        {
            // If item is locked, trigger the locked item event
            if (!item.unlocked)
            {
                // OnLockedItemPressed?.Invoke(item);
                // Store the item we are trying on
                TryingOnItem = true;
                item.category.TryOnItem(item);
                OnItemChanged.Invoke(item);
                return;
            }

            // Change the item of the target part
            TryingOnItem = false;
            item.category.ChangeItem(item, true);
            OnItemChanged?.Invoke(item);
        }

        public void ColorButtonPressed(SO_Color color)
        {
            OnColorButtonPressed?.Invoke(color);
        }

        public void UnlockItem(SO_Item item)
        {
            // Trigger the item unlocked event
            item.unlocked = true;
            OnItemUnlocked?.Invoke(item);
            ChangeItem(item);
        }

        public void UnlockItem(SO_Item currentItem, SO_Color color)
        {
            color.unlocked = true;
            OnColorUnlocked?.Invoke();
            ChangeItemColor(currentItem, color);
        }

        public SO_Item GetTargetItem()
        {
            return TryingOnItem ? targetCategory.TryingItem : targetCategory.CurrentItem;
        }
    }
}