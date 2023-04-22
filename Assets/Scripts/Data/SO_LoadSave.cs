﻿using UnityEngine;
using Utils;

namespace Data {
    [CreateAssetMenu(fileName = "SaveData", menuName = "ScriptableObjects/Data/SaveData", order = 0)]
    public class SO_LoadSave : ScriptableObject {
        
        // This is a scriptable object - only to make life easier when accessing this object
        // It holds lists of every save-able item 
        // Items such as clothing items have save and load functions
        // These are called and made into lists - then serialized
        
        // This data is loaded from the parts
        
        [SerializeField] private ItemData itemData;

        public void SaveGame() {
            ItemData.ItemSaveData data = itemData.GetItemSaveData();
            Serialization.Save("Save.dat", data);
        }
        
        // How can I make this expandable?
        // I want to be able to add more save data
        
        public void LoadGame() {
            
            if (Serialization.CheckSaveExists("Save.dat")) {
                // If the save file exists
                ItemData.ItemSaveData data = Serialization.Load("Save.dat");
                itemData.LoadItemData(data);
            }
            else {
                // Create a default save file
                SaveGame();
            }
        }
    }
}