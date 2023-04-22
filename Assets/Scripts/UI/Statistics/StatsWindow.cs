using Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Statistics {
    public class StatsWindow : MonoBehaviour {
        // Window to display the stats
        
        [SerializeField] private Button button;
        [SerializeField] private RectTransform statsBackground;
        
        private DataWrangler.GameData gd;
        
        // Add a listener to the button that opens and closes the window
        private void Awake() {
            // Get the game data
            gd = DataWrangler.GetGameData();

            button.onClick.AddListener(()=> {
                if (statsBackground.gameObject.activeSelf) {
                    CloseWindow();
                } 
                else {
                    OpenWindow();
                }
            });
        }
        
        private void OpenWindow() {
            // Animate the window y scale using DOTween
            statsBackground.gameObject.SetActive(true);
            statsBackground.DOKill();
            statsBackground.DOScaleY(1, gd.uIData.MenuAnimSpeed * 0.5f).
                SetEase(gd.uIData.DefaultMenuEase);

        }
        
        private void CloseWindow() {
            // Animate the window closing using DOTween
            statsBackground.DOKill();
            statsBackground.DOScaleY(0, gd.uIData.MenuAnimSpeed * 0.5f).
                SetEase(gd.uIData.DefaultMenuEase).
                OnComplete(()=> statsBackground.gameObject.SetActive(false)
                );
        }
        
        
    }
}