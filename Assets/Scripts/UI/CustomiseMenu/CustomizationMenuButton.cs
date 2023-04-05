using System;
using Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.CustomiseMenu {
    public class CustomizationMenuButton : MonoBehaviour {

        [SerializeField] private CustomiseEvents events;
        
        [SerializeField] private Button button;
        [SerializeField] private RectTransform xf;
        [SerializeField] private float animSpeed = 0.2f;

        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            
            button.onClick.AddListener(OpenMenu);
            events.OnMenuClosed.AddListener(UnHideMenu);
            
            gd.roundData.OnGameBegin.AddListener(HideMenu);
            gd.eventData.OnGameOver.AddListener(UnHideMenu);
        }

        private void UnHideMenu() {
            xf.DOKill();
            xf.DOScaleY( 1, animSpeed);
        }

        private void HideMenu(int arg0) {
            xf.DOKill();
            xf.DOScaleY( 0, animSpeed);
        }

        private void OpenMenu() {
            events.OpenMenu();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                events.CloseMenu();
            }
        }
    }
}