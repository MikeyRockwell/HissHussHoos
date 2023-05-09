using Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Statistics {
    public class UIWindow : MonoBehaviour {
        // Window that scales open on the Y axis
        // And sets its own active state
        public Button openWindowButton;
        public Button closeWindowButton;
        public RectTransform background;
        
        private DataWrangler.GameData gd;
        
        // Add a listener to the button that opens and closes the window
        protected virtual void Awake() {
            // Get the game data
            gd = DataWrangler.GetGameData();
            // Add a listener to the button that opens and closes the window
            if (openWindowButton != null) {
                openWindowButton.onClick.AddListener(CheckWindowStatus);
            }
            // Add a listener to the background button that closes the window
            closeWindowButton.onClick.AddListener(CheckWindowStatus);
        }

        protected bool CheckWindowOpen() {
            return background.gameObject.activeSelf;
        }

        protected void CheckWindowStatus() {
            if (background.gameObject.activeSelf) {
                CloseWindow();
            }
            else {
                OpenWindow();
            }
        }
        
        private void OpenWindow() {
            // Animate the window y scale using DOTween
            background.gameObject.SetActive(true);
            background.DOKill();
            background.DOScaleY(1, gd.uIData.MenuAnimSpeed * 0.5f).
                SetEase(gd.uIData.DefaultMenuEase);

        }
        
        private void CloseWindow() {
            // Animate the window closing using DOTween
            background.DOKill();
            background.DOScaleY(0, gd.uIData.MenuAnimSpeed * 0.5f).
                SetEase(gd.uIData.DefaultMenuEase).
                OnComplete(()=> background.gameObject.SetActive(false)
                );
        }
        
        
    }
}