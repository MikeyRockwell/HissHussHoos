using Managers;
using DG.Tweening;
using UnityEngine;
using Data.Customization;
using UnityEngine.UI;

namespace UI.CustomiseMenu {
    
    // This is the main menu window
    public class CustomizationMenuWindow : MonoBehaviour {
        
        [SerializeField] private CustomizationEvents menuEvents;
        
        [SerializeField] private RectTransform xf;
        [SerializeField] private float windowScreenPercent;
        [SerializeField] private float animSpeed = 0.2f;

        private Button button;
        private DataWrangler.GameData gd;
        
        private void Awake() {
            // Get the game data
            gd = DataWrangler.GetGameData();
            // Get the button component
            button = GetComponent<Button>();
            // Add a listener to the button that closes the menu
            button.onClick.AddListener(()=> menuEvents.CloseMenu());
            // Add listeners to the menu events
            menuEvents.OnMenuOpened.AddListener(OpenMenu);
            menuEvents.OnMenuClosed.AddListener(CloseMenu);
            
            gd.roundData.OnGameBegin.AddListener(CloseMenu);

            // SetWidth(windowScreenPercent);
            CloseMenu();
        }
        
        
        // Adjust the menu width to a percentage of the screen width
        // private void SetWidth(float percent) {
        //     var rect = xf.rect;
        //     rect.width = Screen.width * (percent / 100);
        //     xf.sizeDelta = rect.size;
        // }

        private void OpenMenu(SO_CharacterPart arg0) {
            gameObject.SetActive(true);
#if UNITY_EDITOR
            // SetWidth(windowScreenPercent);
#endif
            xf.DOKill();
            xf.DOScaleX(1, animSpeed);
        }

        private void CloseMenu(int arg0) {
            xf.DOKill();
            xf.DOScaleX(0, animSpeed).OnComplete(()=> gameObject.SetActive(false));
        }

        private void CloseMenu() {
            xf.DOKill();
            xf.DOScaleX(0, animSpeed).OnComplete(()=> gameObject.SetActive(false));
        }
    }
}