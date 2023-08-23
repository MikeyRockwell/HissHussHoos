using System;
using UnityEngine;

namespace Data.Customization {
    [CreateAssetMenu(fileName = "NewColor", menuName = "ScriptableObjects/Customization/Color", order = 0)]
    public class SO_Color : ScriptableObject {
        public string ColorName;
        [ColorUsage(true, true)] public Color Color;

        public bool unlocked;
        public float price;
        public bool defaultColor;

        public void ResetColor() {
            // This is called when the game resets the character
            unlocked = defaultColor;
        }

        public void LoadSaveData(ItemData.ItemSaveData saveData) {
            // Create a default state for this item
            SaveData sd = new(ColorName, defaultColor);

            // Load the save data if it exists
            foreach (SaveData item in saveData.colorSaveData) {
                if (defaultColor) {
                    unlocked = true;
                    return;
                }

                if (item.colorName == ColorName) unlocked = item.unlocked;
            }
        }

        public SaveData CreateSaveData() {
            // Create a save data for this item
            return new SaveData(ColorName, unlocked);
        }

        [Serializable]
        public struct SaveData {
            // This is the save data for all the items
            public string colorName;
            public bool unlocked;

            public SaveData(string colorName, bool unlocked) {
                // Constructor
                this.colorName = colorName;
                this.unlocked = unlocked;
            }
        }
    }
}