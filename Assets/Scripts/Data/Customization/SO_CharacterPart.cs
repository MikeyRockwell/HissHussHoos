using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Data.Customization {
    [CreateAssetMenu(
        fileName = "NewCharacterPart", menuName = "ScriptableObjects/Customization/CharacterPart", order = 0)]
    public class SO_CharacterPart : ScriptableObject {

        // This is a communicator for each part of the character
        // Sends a message to the correct body part when we want to switch to another article of clothing etc
        // Could be used as a save game data collection too

        public string UIName;
        
        public SO_Item[] Items;
        public SO_Item CurrentItem;
        public SO_Item DefaultItem;
        public UnityEvent<SO_Item> OnChangeItem;
        
        public UnityEvent<SO_Item, Color> OnChangeItemColor;

        [FormerlySerializedAs("loadSave")] [FormerlySerializedAs("SaveData")] public LoadSaveData loadSaveData;

        public void ChangeItem(SO_Item newItem, bool save) {
            // Sets default item if none is current
            CurrentItem ??= DefaultItem;
            
            // Un equip the current item 
            CurrentItem.equipped = false;
            
            // Apply and equip new item
            CurrentItem = newItem;
            CurrentItem.equipped = true;
            
            // Events and change color
            OnChangeItem?.Invoke(newItem);
            ChangeItemColor(CurrentItem.color, save);
        }

        public void ChangeItemColor(Color newColor, bool save) {
            CurrentItem.color = newColor;
            OnChangeItemColor?.Invoke(CurrentItem, newColor);
            // SaveData.SaveGame();
            if (save) {
                loadSaveData.SaveGame();
            }
        }
    }
}