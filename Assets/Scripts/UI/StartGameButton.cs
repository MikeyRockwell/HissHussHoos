using Data.Customization;
using Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class StartGameButton : MonoBehaviour {

        [SerializeField] private Vector3 startScale;
        
        private Button button;
        private DataWrangler.GameData gd;

        private void Awake() {
            
            gd = DataWrangler.GetGameData();
            button = GetComponent<Button>();
            button.onClick.AddListener(()=> gd.roundData.BeginGame());
            gd.roundData.OnGameBegin.AddListener(Disable);
            gd.eventData.OnGameOver.AddListener(Enable);
            gd.customEvents.OnMenuOpened.AddListener(Disable);
            gd.customEvents.OnMenuClosed.AddListener(Enable);
            Pulse();
        }


        private void Pulse() {
            transform.localScale = startScale;
            transform.DOScale(transform.localScale * 1.15f, 0.3f).SetLoops(-1, LoopType.Yoyo);
        }
        
        private void Disable(SO_CharacterPart arg0) {
            Disable(0);
        }

        private void Disable (int arg0) {
            button.enabled = false;
            transform.DOKill();
            transform.DOScale(Vector3.zero, 0.2f);
            gameObject.SetActive(false);
        }

        private void Enable() {
            button.enabled = true;
            gameObject.SetActive(true);
            Pulse();
        }
    }
}