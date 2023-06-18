using Managers;
using DG.Tweening;
using UnityEngine;
using Data.Customization;
using MoreMountains.Feedbacks;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace UI.CustomiseMenu
{
    // This is the main menu window
    public class CustomizationMenuWindow : MonoBehaviour
    {
        [SerializeField] private CustomizationEvents menuEvents;
        [SerializeField] private MMF_Player openFeedbacks;
        [SerializeField] private MMF_Player closeFeedbacks;

        [SerializeField] private RectTransform xf;
        [SerializeField] private float windowScreenPercent;
        [SerializeField] private float animSpeed = 0.2f;

        [SerializeField] private Button bgButton;
        private DataWrangler.GameData gd;

        private void Awake()
        {
            // Get the game data
            gd = DataWrangler.GetGameData();


            // Add a listener to the button that closes the menu
            bgButton.onClick.AddListener(() => menuEvents.CloseMenu());

            // Add listeners to the menu events
            menuEvents.OnMenuOpened.AddListener(OpenMenu);
            menuEvents.OnMenuClosed.AddListener(() => CloseMenu(0));
            gd.roundData.OnGameBegin.AddListener(CloseMenu);
        }

        private void OpenMenu(SO_CharacterPart arg0)
        {
            openFeedbacks.PlayFeedbacks();
        }

        private void CloseMenu(int arg0)
        {
            closeFeedbacks.PlayFeedbacks();
        }
    }
}