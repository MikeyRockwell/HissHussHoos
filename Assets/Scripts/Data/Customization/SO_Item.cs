using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.Customization {
    [CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Customization/CharacterItem", order = 0)]
    public class SO_Item : ScriptableObject {
        
        public SO_CharacterPart characterPart;
        
        [FoldoutGroup("Sprites")] public Sprite menuSprite;
        [FoldoutGroup("Sprites")] public Sprite[] animSprites;

        [FoldoutGroup("Colors")] public bool noColors;
        [FoldoutGroup("Colors")] public bool standardColors;
        [FoldoutGroup("Colors")] public bool customColors;
        [FoldoutGroup("Colors")] public Color[] availableColors;
        [FoldoutGroup("Colors")] public Color color = Color.white;
        
        [FoldoutGroup("Status")] public bool unlocked;
        [FoldoutGroup("Status")] public bool equipped;


        public void LoadSaveData(SaveData sd) {
            
            unlocked = sd.unlocked;
            equipped = sd.equipped;
            color = new Color(sd.color.x, sd.color.y, sd.color.z);
            
            // Is this equipped check enough to handle loading the current item??
            if (!equipped) return;
            
            characterPart.ChangeItem(this);
            characterPart.ChangeItemColor(color);
        }
        
        public SaveData CreateSaveData() {
            return new SaveData(unlocked, equipped, new Vector3(color.r, color.g, color.b));
        }
        
        [System.Serializable]
        public struct SaveData {

            public bool unlocked;
            public bool equipped;
            public Vector3 color;

            public SaveData(bool unlocked, bool equipped, Vector3 color) {
                this.unlocked = unlocked;
                this.equipped = equipped;
                this.color = color;
            }
        }
    }
}