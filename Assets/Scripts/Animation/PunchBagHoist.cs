using Data;
using Managers;
using DG.Tweening;
using UnityEngine;

namespace Animation {
    public class PunchBagHoist : MonoBehaviour {
        
        [SerializeField] private float yDefault;
        [SerializeField] private float yHoist;
        [SerializeField] private float animDuration = 0.5f;
        
        private Transform xf;
        private DataWrangler.GameData gd;
        
        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.roundData.OnBonusRoundBegin.AddListener((() => HoistBag(0)));
            gd.roundData.OnRoundBegin.AddListener(HoistBag);
            xf = transform;
        }
        
        private void HoistBag(int arg0) {

            float endPosY = 0;
            switch (gd.roundData.roundType) {
                case RoundData.RoundType.warmup:
                    endPosY = yDefault;
                    break;
                case RoundData.RoundType.normal:
                    endPosY = yDefault;
                    break;
                case RoundData.RoundType.bonus:
                    endPosY = yHoist;
                    break;
            }
            xf.DOLocalMove(new Vector2(xf.position.x, endPosY), animDuration);
        }
    }
}