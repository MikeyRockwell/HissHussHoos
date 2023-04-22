using TMPro;
using System;
using Managers;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Data.Customization;

namespace UI.CustomiseMenu {
    public class ItemUnlockWindow : MonoBehaviour {
            
        // Window that pops up when you click on a locked item
        [SerializeField] private Button button;
        [SerializeField] private Button unlockButton;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemPrice;
        [SerializeField] private TextMeshProUGUI unlockText;
        [SerializeField] private TextMeshProUGUI currentMorale;
        [SerializeField] private Image itemImage;

        private RectTransform xf;
        private bool open;
        private DataWrangler.GameData gd;
        private SO_Item currentItem;

        private void Awake() {
            // Cache components
            xf = GetComponent<RectTransform>();
            // Subscribe to events            
            gd = DataWrangler.GetGameData();
            gd.customEvents.OnLockedItemPressed.AddListener(OpenWindow);
            gd.customEvents.OnMenuClosed.AddListener(CloseWindow);
            gd.customEvents.OnItemUnlocked.AddListener(delegate(SO_Item arg0) { CloseWindow(); });
            // Init button
            button.onClick.AddListener(CloseWindow);
            button.image.color = gd.uIData.MenuBackgroundColor;
            // Init unlock button
            unlockButton.onClick.AddListener(()=>gd.customEvents.UnlockItem(currentItem));
            // Init window
            open = Math.Abs(xf.localScale.x - 1) < 0.001f;
            CloseWindow();
        }

        private void OpenWindow(SO_Item item) {
            // Open the window and set the item
            currentItem = item;
            // Check if the item is available
            bool available = item.price <= gd.playerData.moralePoints;
            unlockButton.enabled = available;
            // Init graphics
            InitGraphics(item, available);
            // Open window
            if (!open) {
                xf.DOScale(Vector3.one, gd.uIData.MenuAnimSpeed).
                    SetEase(gd.uIData.DefaultMenuEase).
                    OnComplete(()=>open = true);
            }
        }

        private void InitGraphics(SO_Item item, bool available) {
            // Init graphics
            itemImage.sprite = item.menuSprite;
            // Set the color of the text
            Color textColor = available ? gd.uIData.LaserGreen : Color.red;
            itemPrice.color = textColor;
            unlockText.color = textColor;
            // Set the text            
            currentMorale.text = "MORAALE POINTS " + gd.playerData.moralePoints;
            itemName.text = item.itemName;
            itemPrice.text = item.price + " MP";
            
        }

        private void CloseWindow() {
            // Close the window
            if (open) {
                xf.DOScale(Vector3.zero, gd.uIData.MenuAnimSpeed).
                    SetEase(gd.uIData.DefaultMenuEase).
                    OnComplete(()=>open = false);
            }
        }
    }
}