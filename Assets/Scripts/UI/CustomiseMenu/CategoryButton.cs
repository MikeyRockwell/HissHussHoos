using UnityEngine;
using UnityEngine.UI;

namespace UI.CustomiseMenu {
    public class CategoryButton : MonoBehaviour {
        
        public Button button;
        public Color disabledColor;
        
        public CustomizationEvents events;

        public virtual void Awake() {
            // Cache button and set disabled
            button = GetComponent<Button>();
            disabledColor = button.image.color;
            // Check if active button on any button press
            events.OnCategoryButtonPressed.AddListener(SetActiveButton);
        }


        private void SetActiveButton(Button eventButton) {
            // Manual navigation button highlighting
            button.image.color = button == eventButton ? button.colors.selectedColor : disabledColor;
        }
    }
}