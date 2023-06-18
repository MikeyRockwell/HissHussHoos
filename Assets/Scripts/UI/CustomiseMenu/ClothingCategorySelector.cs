using UnityEngine;
using Data.Customization;

namespace UI.CustomiseMenu
{
    public class ClothingCategorySelector : CategoryButton
    {
        [SerializeField] private SO_CharacterPart characterPart;

        public override void Awake()
        {
            base.Awake();
            // Link button press to customize menu events
            button.onClick.AddListener(() => events.SelectClothingItem(characterPart, button));

            // Set first category active on game start
            if (transform.parent.GetChild(0) == transform) events.SelectClothingItem(characterPart, button);
        }
    }
}