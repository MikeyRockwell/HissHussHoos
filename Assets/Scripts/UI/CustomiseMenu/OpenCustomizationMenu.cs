using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

namespace UI.CustomiseMenu {
    public class OpenCustomizationMenu : MonoBehaviour {

        [SerializeField] private CustomiseEvents events;
        
        [SerializeField] private Button button;
        [SerializeField] private RectTransform xf;
        [SerializeField] private float xClosedPos;
        [SerializeField] private float xOpenPos;
        [SerializeField] private float animSpeed = 0.2f;

        [SerializeField] private bool open;
        public RectTransform subMenu;

        private void Awake() {
            button.onClick.AddListener(CheckStatus);
            events.OnCloseOtherTabs.AddListener(CheckOpen);
            Close();
        }

        private void CheckOpen() {
            if (open) Close();
        }

        private void CheckStatus() {
            events.CloseOtherTabs();
            
            if (open) {
                Close();
                return;
            }
            Open();
        }

        private void Open() {
            xf.DOKill();
            xf.DOLocalMove(new Vector2(xOpenPos, xf.localPosition.y), animSpeed).
                OnComplete(OpenSubMenu);
        }

        private void OpenSubMenu() {
            open = true;
            subMenu.DOScaleY(1, animSpeed * 0.5f);
            events.OpenMenu();
        }

        private void Close() {

            xf.DOKill();
            Sequence seq = DOTween.Sequence();
            seq.Append(subMenu.DOScaleY(0, animSpeed * 0.5f)).
                Append(xf.DOLocalMove(new Vector2(xClosedPos, xf.localPosition.y), animSpeed)).
                OnComplete(() => open = false);
        }
    }
}