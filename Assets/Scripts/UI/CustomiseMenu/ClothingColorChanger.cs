using UnityEngine;
using UnityEngine.UI;

namespace UI.CustomiseMenu {
    public class ClothingColorChanger : MonoBehaviour {
        
        [SerializeField] private CustomiseEvents events;
        private Image image;

        private void Awake() {
            image = GetComponent<Image>();
            GetComponent<Button>().onClick.AddListener(()=>events.ChangeItemColor(image.color));
        }

        public void Init(Color col) {
            image.color = col;
        }
    }
}