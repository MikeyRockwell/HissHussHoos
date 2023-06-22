using System;
using System.Linq;
using UnityEngine;
using UI.CustomiseMenu;
using Data.Customization;
using System.Collections.Generic;
using UnityEngine.Serialization;
using static Data.Customization.SO_Color;
using static Data.Customization.SO_Item;

namespace Data
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/Data/ItemData", order = 0)]

    // This is a scriptable object that holds all the items and parts
    public class ItemData : ScriptableObject
    {
        public CustomizationEvents events;

        [FormerlySerializedAs("allParts")] public List<SO_Category> allCategories;
        public List<SO_Item> allItems;
        public List<SO_Color> allColors;


        private void Initialize()
        {
            // Create a list of all the items
            allItems.Clear();
            foreach (SO_Item item in allCategories.SelectMany(part => part.Items)) allItems.Add(item);
        }

        public void ResetItems()
        {
            // Reset all the items
            foreach (SO_Item item in allCategories.SelectMany(part => part.Items)) item.ResetItem();
            // Reset all the colors
            foreach (SO_Color color in allColors) color.ResetColor();
        }

        public void LoadItemData(ItemSaveData data)
        {
            Initialize();
            // Load all the save data
            for (int i = 0; i < allItems.Count; i++) allItems[i].LoadSaveData(data);
            for (int i = 0; i < allColors.Count; i++) allColors[i].LoadSaveData(data);
        }

        public ItemSaveData GetItemSaveData()
        {
            // Create a list of all the save data 
            var items = allItems.Select(item => item.CreateSaveData()).ToList();
            var colors = allColors.Select(color => color.CreateSaveData()).ToList();
            return new ItemSaveData(items, colors);
        }

        [Serializable]
        public struct ItemSaveData
        {
            // This is the save data for all the items
            public List<SO_Item.SaveData> itemSaveData;
            public List<SO_Color.SaveData> colorSaveData;

            public ItemSaveData(List<SO_Item.SaveData> itemSaveData, List<SO_Color.SaveData> colorSaveData)
            {
                // Constructor
                this.itemSaveData = itemSaveData;
                this.colorSaveData = colorSaveData;
            }
        }
    }
}