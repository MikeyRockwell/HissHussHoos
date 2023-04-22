using UnityEngine;

namespace Data.Customization {
    [CreateAssetMenu(
        fileName = "NewCharacter", menuName = "ScriptableObjects/Customization/Character", order = 0)]
        public class SO_Character : ScriptableObject {
            
            public string UIName;

            public Sprite menuIcon;

        }
}