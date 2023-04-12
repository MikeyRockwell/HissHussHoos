using System;
using DG.Tweening;
using UnityEngine;
using Data.Customization;
using UnityEngine.UI;
using Utils;

namespace UI.CustomiseMenu {
    public class CustomizationMenuWindow : MonoBehaviour {

        [SerializeField] private CustomiseEvents menuEvents;
        
        [SerializeField] private RectTransform xf;
        [SerializeField] private float openPivot;
        [SerializeField] private float closedPivot;
        [SerializeField] private float animSpeed = 0.2f;

        private Button button;
        
        private void Awake() {
            button = GetComponent<Button>();
            button.onClick.AddListener(()=> menuEvents.CloseMenu());
            
            menuEvents.OnMenuOpened.AddListener(OpenMenu);
            menuEvents.OnMenuClosed.AddListener(CloseMenu);
            
            CloseMenu();
        }

        /*private void OnValidate() {
            CalculateWidth();
        }

        private void CalculateWidth() {
            float windowWidth = Screen.width * 0.5f;
            Log.Message("Screen Width = " + windowWidth );
            xf.sizeDelta = new Vector2(windowWidth, xf.sizeDelta.y);
        }*/

        private void OpenMenu(SO_CharacterPart arg0) {
            gameObject.SetActive(true);
            xf.DOKill();
            xf.DOScaleX(1, animSpeed);
        }

        private void CloseMenu() {
            xf.DOKill();
            xf.DOScaleX(0, animSpeed).OnComplete(()=> gameObject.SetActive(false));
        }
    }
}