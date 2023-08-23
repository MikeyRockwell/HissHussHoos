using Managers;
using DG.Tweening;
using UnityEngine;
using Data.Customization;

namespace Animation {
    // Hoists the punching bag up and down
    // Applied to the punching bag
    public class PunchBagHoist : MonoBehaviour {
        
        [SerializeField] private float yDefault;
        [SerializeField] private float yHoist;
        [SerializeField] private float animDuration = 0.5f;

        private Transform xf;
        private DataWrangler.GameData gd;

        private void Awake() {
            // EVENTS
            gd = DataWrangler.GetGameData();
            gd.roundData.OnRoundBegin.AddListener(LowerBag);
            gd.eventData.OnGameOver.AddListener(RaiseBag);
            gd.customEvents.OnMenuOpened.AddListener((x)=>LowerBag(0));
            gd.customEvents.OnMenuClosed.AddListener(RaiseBag);
            // COMPONENTS
            xf = transform;
        }
        
        private void LowerBag(int arg0) {
            xf.DOKill();
            xf.DOLocalMove(new Vector2(xf.position.x, yDefault), animDuration).SetUpdate(true);
        }

        private void RaiseBag() {
            xf.DOKill();
            xf.DOLocalMove(new Vector2(xf.position.x, yHoist), animDuration).SetUpdate(true);
        }
    }
}