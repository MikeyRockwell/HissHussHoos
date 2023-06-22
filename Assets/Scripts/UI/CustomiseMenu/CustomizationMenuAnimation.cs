using DG.Tweening;
using UnityEngine;
using Data.Customization;
using UnityEngine.UI;

namespace UI.CustomiseMenu
{
    public class CustomizationMenuAnimation : MonoBehaviour
    {
        [SerializeField] private CustomizationEvents menuEvents;

        [SerializeField] private float openPivot;
        [SerializeField] private float closedPivot;
        [SerializeField] private float animSpeed = 0.2f;

        private RectTransform xf;
        private Button button;

        private void Awake()
        {
            xf = GetComponent<RectTransform>();

            button = GetComponent<Button>();
            button.onClick.AddListener(() => menuEvents.CloseMenu());

            menuEvents.OnMenuOpened.AddListener(OpenMenu);
            menuEvents.OnMenuClosed.AddListener(CloseMenu);
        }

        private void OpenMenu(SO_Category arg0)
        {
            xf.DOKill();
            xf.DOPivotX(openPivot, animSpeed);
        }

        private void CloseMenu()
        {
            xf.DOKill();
            xf.DOPivotX(closedPivot, animSpeed);
        }
    }
}