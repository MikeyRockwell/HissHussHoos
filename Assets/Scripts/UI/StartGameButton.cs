using Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Data.Customization;
using MoreMountains.Feedbacks;

namespace UI {
    public class StartGameButton : MonoBehaviour {

        [SerializeField] private Vector3 startScale;
        
        private Button button;
        private DataWrangler.GameData gd;

        private void Awake() {
            
            gd = DataWrangler.GetGameData();
            button = GetComponent<Button>();
            button.onClick.AddListener(BeginGame);
            
            gd.eventData.OnGameOver.AddListener(Enable);
            gd.customEvents.OnMenuOpened.AddListener(Disable);
            gd.customEvents.OnMenuClosed.AddListener(Enable);
        }

        private void BeginGame()
        {
            gd.roundData.BeginGameDelayed();
            Disable();
        }
        
        private void Disable(SO_CharacterPart arg0) {
            Disable();
        }

        private void Disable () {
            button.enabled = false;
            transform.DOKill();
            gameObject.SetActive(false);
        }

        private void Enable() {
            button.enabled = true;
            gameObject.SetActive(true);
        }
    }
}