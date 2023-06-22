using UnityEngine;
using Data.Customization;
using Managers;
using UnityEngine.Serialization;

namespace UI.CustomiseMenu
{
    public class CategorySelector : CategoryButton
    {
        [SerializeField] private SO_Category category;

        private DataWrangler.GameData gd;
        
        public override void Awake()
        {
            base.Awake();
            
            gd = DataWrangler.GetGameData();
            
            gd.customEvents.OnMenuOpened.AddListener(CheckActiveCategory);
            
            // Link button press to customize menu events
            button.onClick.AddListener(() => events.SelectClothingItem(category, button));

            // Set first category active on game start
            if (transform.parent.GetChild(0) == transform) events.SelectClothingItem(category, button);
        }
        
        private void CheckActiveCategory(SO_Category menuCategory)
        {
            if (gd.customEvents.targetCategory == category)
            {
                button.Select();
                SetActiveButton(button);
            }
            else SetActiveButton(null);
        }
    }
}