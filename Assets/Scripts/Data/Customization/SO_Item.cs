using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.Customization {
    
    [CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Customization/CharacterItem", order = 0)]
    
    // This is the base class for all items that can be equipped by the player
    public class SO_Item : ScriptableObject {
        
        public SO_CharacterPart characterPart;

        public string itemName;
        
        
        [PreviewField(100, ObjectFieldAlignment.Left)]
        [FoldoutGroup("Sprites")] public Sprite menuSprite;
        [PreviewField(100, ObjectFieldAlignment.Left)]
        [FoldoutGroup("Sprites")] public Sprite[] animSprites;
          
        [FoldoutGroup("Colors")] public bool noColors;
        [FoldoutGroup("Colors")] public bool standardColors;
        [FoldoutGroup("Colors")] public bool customColors;
        [FoldoutGroup("Colors")] public bool colorMask;
        [FoldoutGroup("Colors")] public bool zestGlasses;
        [FoldoutGroup("Colors"), ColorUsage(true, true)] 
                                 public Color zestLightColor;
        [FoldoutGroup("Colors")] public Color[] availableColors;
        [FoldoutGroup("Colors")] public Color color = Color.white;
        
        [FoldoutGroup("Status")] public int price;
        [FoldoutGroup("Status")] public bool defaultItem;
        [FoldoutGroup("Status")] public bool unlocked;
        [FoldoutGroup("Status")] public bool equipped;

        public void ResetItem() {
            // This is called when the game resets the character
            // DEBUG
            color = Color.white;
            if (defaultItem) {
                unlocked = true;
                equipped = true;
                characterPart.ChangeItem(this, true);
            }
            else {
                unlocked = false;
                equipped = false;
            }
        }

        public void LoadSaveData(SaveData sd) {
            // Loads the save data into the item           
            unlocked = sd.unlocked;
            equipped = sd.equipped;
            color = new Color(sd.color.x, sd.color.y, sd.color.z);
            
            if (!equipped) return;
            // If the item is equipped, change the character part to this item
            characterPart.ChangeItem(this, false);
        }
        
        public SaveData CreateSaveData() {
            // Creates a save data object from the item
            return new SaveData(name, unlocked, equipped, new Vector3(color.r, color.g, color.b));
        }
        
        [Serializable]
        public struct SaveData {
            // This is the save data for the item
            public string itemName;
            public bool unlocked;
            public bool equipped;
            public Vector3 color;

            public SaveData(string name, bool unlocked, bool equipped, Vector3 color) {
                // Constructor
                itemName = name;
                this.unlocked = unlocked;
                this.equipped = equipped;
                this.color = color;
            }
        }
    }
}