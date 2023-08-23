using UnityEngine;

namespace Data.Customization {
    /// <summary>
    /// Is the data master 
    /// </summary>
    [CreateAssetMenu(
        fileName = "NewCharacter", menuName = "ScriptableObjects/Customization/Character", order = 0)]
    public class CharacterData : ScriptableObject {
        public string UIName;

        public Sprite menuIcon;

        public enum Character {
            William,
            TerryTamati,
            None
        }

        public Character currentCharacter;
    }
}