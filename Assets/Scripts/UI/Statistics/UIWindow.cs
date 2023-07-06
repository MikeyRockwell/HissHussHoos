using Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI.Statistics
{
    public class UIWindow : MonoBehaviour
    {
        // Window that scales open on the Y axis
        // And sets its own active state
        public Button openWindowButton;
        public Button closeWindowButton;
        public RectTransform background;
        public UnityEvent OnWindowOpen;

        private DataWrangler.GameData gd;


        // Add a listener to the button that opens and closes the window
        protected virtual void Awake()
        {
            // Get the game data
            gd = DataWrangler.GetGameData();
            // Add a listener to the button that opens and closes the window
            if (openWindowButton != null) openWindowButton.onClick.AddListener(CheckWindowStatus);
            // Add a listener to the background button that closes the window
            if (closeWindowButton != null) closeWindowButton.onClick.AddListener(CheckWindowStatus);
        }

        protected void CheckWindowStatus()
        {
            if (background.gameObject.activeSelf)
            {
                Log.Message("Window Button Pressed");
                CloseWindow();
            }
            else
            {
                OpenWindow();
            }
        }

        protected void OpenWindow()
        {
            // Animate the window y scale using DOTween
            background.gameObject.SetActive(true);
            background.DOKill();
            background.DOScaleY(1, gd.uIData.MenuAnimSpeed * 0.5f).SetEase(gd.uIData.DefaultMenuEase)
                .OnComplete(() => OnWindowOpen?.Invoke());
        }

        protected void CloseWindow()
        {
            // Animate the window closing using DOTween
            background.DOKill();
            background.DOScaleY(0, gd.uIData.MenuAnimSpeed * 0.5f).SetEase(gd.uIData.DefaultMenuEase)
                .OnComplete(DisableWindow);
        }

        protected void DisableWindow()
        {
            background.gameObject.SetActive(false);
        }
    }
}