using UnityEngine;
using UnityEngine.UI;

namespace UI.CustomiseMenu {
    public class CategoryButton : MonoBehaviour {
        
        public Button button;
        public Color disabledColor;
        public Image icon;
        public Outline iconOutline;
        public CustomizationEvents events;

        public virtual void Awake() {
            // Cache button and set disabled
            button = GetComponent<Button>();
            disabledColor = SetButtonColor();
            button.image.color = button.colors.selectedColor;
            icon.color = disabledColor;
            // Check if active button on any button press
            events.OnCategoryButtonPressed.AddListener(SetActiveButton);
        }

        private Color SetButtonColor() {
            int index = transform.GetSiblingIndex();
            Vector3 HSVColor = new (index * 0.1f, 1, 1f);
            return Color.HSVToRGB(HSVColor.x, HSVColor.y, HSVColor.z);
        }

        protected void SetActiveButton(Button eventButton) {
            // Manual navigation button highlighting
            bool isSelected = button == eventButton;
            button.image.color      = isSelected ? disabledColor : button.colors.selectedColor;
            icon.color              = isSelected ? new Color(0.110f, 0.059f, 0.114f, 1.0f) : disabledColor;
            // iconOutline.effectColor = !isSelected ? Color.black : Color.clear;
        }
    }
}