using System;
using System.Linq;
using UnityEngine;
using UI.CustomiseMenu;
using Data.Customization;
using System.Collections.Generic;

namespace Data {
    [CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/Data/ItemData", order = 0)]
    
    // This is a scriptable object that holds all the items and parts
    public class ItemData : ScriptableObject {

        public CustomizationEvents events; 
        
        public List<SO_CharacterPart> allParts;
        public List<SO_Item> allItems;


        private void Initialize() {
            // Create a list of all the items
            allItems.Clear();
            foreach (SO_Item item in allParts.SelectMany(part => part.Items)) {
                allItems.Add(item);
            }
        }

        public void ResetItems() {
            // Reset all the items
            foreach (SO_Item item in allParts.SelectMany(part => part.Items)) {
                item.ResetItem();
            }
        }

        public void LoadItemData(ItemSaveData data) {
            Initialize();
            // Load all the save data
            for (int i = 0; i < allItems.Count; i++) {
                allItems[i].LoadSaveData(data.itemSaveData[i]);
            }
        }

        public ItemSaveData GetItemSaveData() {
            // Create a list of all the save data
            List<SO_Item.SaveData> items = allItems.Select(item => item.CreateSaveData()).ToList();
            return new ItemSaveData(items);
        }
        
        [Serializable]
        public struct ItemSaveData {
            // This is the save data for all the items
            public List<SO_Item.SaveData> itemSaveData;
            
            public ItemSaveData(List<SO_Item.SaveData> itemSaveData) {
                // Constructor
                this.itemSaveData = itemSaveData;
            }
        }
    }
}