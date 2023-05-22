using Data;
using Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Data.Customization;
using UnityEngine.EventSystems;
using TARGET = Data.TargetData.Target;

namespace UI {
    
    public class PunchButton : MonoBehaviour, IPointerDownHandler {
    
    [SerializeField] private TARGET target;
    [SerializeField] private SO_CharacterPart gloves;

    private Button button;

    private DataWrangler.GameData gd;
    private RectTransform xf;

    private void Awake() {

        gd = DataWrangler.GetGameData();
        xf = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        
        gloves.OnChangeItemColor.AddListener(ChangeButtonColor);
    }

        private void ChangeButtonColor(SO_Item arg0, Color color) {
            button.image.color = color;
        }

        public void OnPointerDown(PointerEventData eventData){

            if (gd.playerData.punching) return;

            // Punch type
            switch (gd.roundData.roundType) {
                case RoundData.RoundType.warmup:
                    gd.eventData.PunchWarmup(target);
                    break;
                case RoundData.RoundType.normal:
                    gd.eventData.PunchNormal(target);
                    break;
                case RoundData.RoundType.timeAttack:
                    gd.eventData.PunchTimeAttack(target);
                    break;
                default:
                    break;
            }
            // Animations
            xf.DOKill();
            xf.DOScale(xf.localScale * 0.9f, 0.05f).SetLoops(2, LoopType.Yoyo);

            // Cooldown
            gd.playerData.punching = true;
            gd.playerData.CoolDown();
        }
    }
}

