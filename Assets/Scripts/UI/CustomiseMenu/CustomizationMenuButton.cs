using System;
using Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.CustomiseMenu {
    public class CustomizationMenuButton : MonoBehaviour {
        [SerializeField] private CustomizationEvents events;

        [SerializeField] private Button button;
        [SerializeField] private RectTransform xf;
        [SerializeField] private float animSpeed = 0.2f;

        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();

            button.onClick.AddListener(OpenMenu);
            events.OnMenuClosed.AddListener(UnHideButton);

            gd.roundData.OnGameBeginDelayed.AddListener(HideButton);
            gd.eventData.OnGameOver.AddListener(UnHideButton);
        }

        private void UnHideButton() {
            button.interactable = true;
            xf.DOKill();
            xf.DOScaleY(1, gd.uIData.MenuAnimSpeed);
        }

        private void HideButton() {
            button.interactable = false;
            xf.DOKill();
            xf.DOScaleY(0, gd.uIData.MenuAnimSpeed);
        }

        private void OpenMenu() {
            events.OpenMenu();
        }
    }
}