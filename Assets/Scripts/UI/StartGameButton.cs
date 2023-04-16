using Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class StartGameButton : MonoBehaviour {

        private Button button;
        private DataWrangler.GameData gd;

        private void Awake() {
            
            gd = DataWrangler.GetGameData();
            button = GetComponent<Button>();
            button.onClick.AddListener(()=> gd.roundData.BeginGame());
            gd.roundData.OnGameBegin.AddListener(Disable);
            gd.eventData.OnGameOver.AddListener(Enable);
            Pulse();
        }

        private void Pulse() {
            transform.DOScale(transform.localScale * 1.15f, 0.3f).SetLoops(-1, LoopType.Yoyo);
        }

        private void Disable (int arg0) {
            transform.DOKill();
            gameObject.SetActive(false);
        }

        private void Enable() {
            gameObject.SetActive(true);
            Pulse();
        }
    }
}