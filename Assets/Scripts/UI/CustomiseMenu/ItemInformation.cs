using TMPro;
using Managers;
using UnityEngine;
using Data.Customization;
using UnityEngine.UI;

namespace UI.CustomiseMenu {
    public class ItemInformation : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;
        [SerializeField] private TextMeshProUGUI itemPrice;
        [SerializeField] private Button buyButton;
        [SerializeField] private Image buyButtonShadow;

        private DataWrangler.GameData gd;
        private SO_Item currentItem;
        private SO_Color currentColor;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            // gd.customEvents.OnMenuOpened.AddListener();
            gd.customEvents.OnItemChanged.AddListener(UpdateItemInformation);
            gd.customEvents.OnColorButtonPressed.AddListener(UpdateColorInformation);
        }

        private void UpdateItemInformation(SO_Item item) {
            currentItem = item;

            // bool unlocked = item.unlocked;

            itemName.text = currentItem.unlocked ? item.itemName : item.itemName + " (Locked)";
            itemName.color = item.color;

            UpdateItemPurchaseOptions();
        }

        private void UpdateItemPurchaseOptions() {
            buyButton.transform.parent.gameObject.SetActive(!currentItem.unlocked);
            itemPrice.gameObject.SetActive(!currentItem.unlocked);

            if (currentItem.unlocked) return;

            bool canAfford = gd.playerData.md.moralePoints >= currentItem.price;
            buyButton.interactable = canAfford;
            buyButton.image.color = canAfford ? gd.uIData.LaserGreen : Color.red;
            buyButtonShadow.color = canAfford ? gd.uIData.LaserGreen * 0.5f : Color.red * 0.5f;
            itemPrice.text = currentItem.price + " MP";
            itemPrice.color = canAfford ? gd.uIData.Gold : Color.red;

            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(BuyItem);
        }

        private void BuyItem() {
            if (currentItem == null) return;
            if (currentItem.unlocked) return;
            if (gd.playerData.md.moralePoints < currentItem.price) return;

            gd.customEvents.UnlockItem(currentItem);
            UpdateItemInformation(currentItem);
        }

        private void UpdateColorInformation(SO_Color color) {
            itemName.color = color.Color;

            if (color.unlocked && !currentItem.unlocked) {
                UpdateItemInformation(currentItem);
                return;
            }

            currentColor = color;

            bool unlocked = color.unlocked;

            itemName.text = unlocked ? color.ColorName : color.ColorName + " (Locked)";

            buyButton.transform.parent.gameObject.SetActive(!unlocked);
            itemPrice.gameObject.SetActive(!unlocked);


            bool canAfford = gd.playerData.md.moralePoints >= color.price;
            buyButton.interactable = canAfford;
            buyButton.image.color = canAfford ? gd.uIData.LaserGreen : Color.red;
            buyButtonShadow.color = canAfford ? gd.uIData.LaserGreen * 0.5f : Color.red * 0.5f;
            itemPrice.text = color.price + " MP";
            itemPrice.color = canAfford ? gd.uIData.Gold : Color.red;

            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(BuyColor);
        }

        private void BuyColor() {
            gd.customEvents.UnlockItem(currentItem, currentColor);
            UpdateItemInformation(currentItem);
        }
    }
}