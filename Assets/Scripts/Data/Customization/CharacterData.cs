using UnityEngine;

namespace Data.Customization
{
    [CreateAssetMenu(
        fileName = "NewCharacter", menuName = "ScriptableObjects/Customization/Character", order = 0)]
    public class CharacterData : ScriptableObject
    {
        public string UIName;

        public Sprite menuIcon;
        
        public enum Character
        {
            William,
            TerryTamati,
            None
        }
        
        public Character currentCharacter;
    }
}