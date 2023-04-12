using Utils;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Data.Customization {
    [CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Customization/CharacterItem", order = 0)]
    public class SO_Item : ScriptableObject {
        
        public SO_CharacterPart characterPart;
        
        [FoldoutGroup("Sprites")] public Sprite menuSprite;
        [FoldoutGroup("Sprites")] public Sprite[] animSprites;

        [FoldoutGroup("Colors")] public bool noColors;
        [FoldoutGroup("Colors")] public bool standardColors;
        [FoldoutGroup("Colors")] public bool customColors;
        [FoldoutGroup("Colors")] public bool colorMask;
        [FoldoutGroup("Colors")] public Color[] availableColors;
        [FoldoutGroup("Colors")] public Color color = Color.white;
        
        [FoldoutGroup("Status")] public bool unlocked;
        [FoldoutGroup("Status")] public bool equipped;


        public void LoadSaveData(SaveData sd) {
            
            /*Log.Message(
                "Loading Item: " + name + " | " +
                "Equipped: " + equipped + " | " +
                "Color: " + color,
                color
                );*/
            unlocked = sd.unlocked;
            equipped = sd.equipped;
            color = new Color(sd.color.x, sd.color.y, sd.color.z);
            
            // Is this equipped check enough to handle loading the current item??
            if (!equipped) return;
            
            characterPart.ChangeItem(this, false);
            // characterPart.ChangeItemColor(color, false);
        }
        
        public SaveData CreateSaveData() {
            /*Log.Message(
                "Saving Item: " + name + " | " +
                "Equipped: " + equipped + " | " +
                "Color: " + color,
                color
            );*/
            return new SaveData(name, unlocked, equipped, new Vector3(color.r, color.g, color.b));
        }
        
        [System.Serializable]
        public struct SaveData {

            public string itemName;
            public bool unlocked;
            public bool equipped;
            public Vector3 color;

            public SaveData(string name, bool unlocked, bool equipped, Vector3 color) {
                this.itemName = name;
                this.unlocked = unlocked;
                this.equipped = equipped;
                this.color = color;
            }
        }
    }
}