using TMPro;
using System;
using Audio;
using Managers;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Data.Customization;
using Utils;

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
        [SerializeField] private Sprite colorSprite;
        [SerializeField] private SoundFXPlayer unlockSFX;
        [SerializeField] private ParticleSystem unlockFX;

        private RectTransform xf;
        private bool open;
        private DataWrangler.GameData gd;
        private SO_Item currentItem;
        private SO_Color currentColor;

        private void Awake() {
            // Cache components
            xf = GetComponent<RectTransform>();
            // Subscribe to events            
            gd = DataWrangler.GetGameData();
            gd.customEvents.OnLockedItemPressed.AddListener(OpenWindow);
            // gd.customEvents.OnLockedColorPressed.AddListener(OpenWindow);
            gd.customEvents.OnMenuClosed.AddListener(CloseWindow);
            gd.customEvents.OnItemUnlocked.AddListener(delegate(SO_Item arg0) { CloseWindow(); });
            gd.customEvents.OnColorUnlocked.AddListener(CloseWindow);
            // Init button
            button.onClick.AddListener(CloseWindow);
            button.image.color = gd.uIData.MenuBackgroundColor;

            // Init window
            open = Math.Abs(xf.localScale.x - 1) < 0.001f;
            CloseWindow();
        }

        private void OpenWindow(SO_Item item) {
            unlockButton.onClick.RemoveAllListeners();
            // Open the window and set the item
            currentItem = item;
            // Check if the item is available
            bool available = item.price <= gd.playerData.md.moralePoints;
            unlockButton.enabled = available;
            // Init graphics
            InitGraphics(item, available);
            // Init unlock button
            unlockButton.onClick.AddListener(UnlockItem);
            // Open window
            if (!open)
                xf.DOScale(Vector3.one, gd.uIData.MenuAnimSpeed).SetEase(gd.uIData.DefaultMenuEase)
                    .OnComplete(() => open = true);
        }

        private void OpenWindow(SO_Color color) {
            unlockButton.onClick.RemoveAllListeners();
            // Open the window and set the item
            currentColor = color;
            // Check if the item is available
            bool available = color.price <= gd.playerData.md.moralePoints;
            unlockButton.enabled = available;
            // Init graphics
            InitGraphics(color, available);
            // Init unlock button
            unlockButton.onClick.AddListener(UnlockColor);
            // Open window
            if (!open)
                xf.DOScale(Vector3.one, gd.uIData.MenuAnimSpeed).SetEase(gd.uIData.DefaultMenuEase)
                    .OnComplete(() => open = true);
        }

        private void InitGraphics(SO_Color color, bool available) {
            // Init graphics
            itemImage.color = color.Color;
            itemImage.sprite = colorSprite;
            // Set the color of the text
            Color textColor = available ? gd.uIData.LaserGreen : Color.red;
            itemPrice.color = textColor;
            unlockText.color = textColor;
            // Set the text            
            currentMorale.text = "MORAALE POINTS " + gd.playerData.md.moralePoints;
            itemName.text = color.ColorName;
            itemPrice.text = color.price + " MP";
        }

        private void InitGraphics(SO_Item item, bool available) {
            // Init graphics
            itemImage.sprite = item.menuSprite;
            itemImage.color = Color.white;
            // Set the color of the text
            Color textColor = available ? gd.uIData.LaserGreen : Color.red;
            itemPrice.color = textColor;
            unlockText.color = textColor;
            // Set the text            
            currentMorale.text = "MORAALE POINTS " + gd.playerData.md.moralePoints;
            itemName.text = item.itemName;
            itemPrice.text = item.price + " MP";
        }

        private void UnlockItem() {
            gd.customEvents.UnlockItem(currentItem);
            PlayFX();
        }

        private void UnlockColor() {
            // gd.customEvents.UnlockItem(currentColor);
            PlayFX();
        }

        private void PlayFX() {
            unlockSFX.PlayRandomAudio();
            unlockFX.Play();
        }

        private void CloseWindow() {
            // Close the window
            if (open)
                xf.DOScale(Vector3.zero, gd.uIData.MenuAnimSpeed).SetEase(gd.uIData.DefaultMenuEase)
                    .OnComplete(() => open = false);
        }
    }
}