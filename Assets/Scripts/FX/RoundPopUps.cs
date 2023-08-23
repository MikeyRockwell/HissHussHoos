using System;
using Managers;
using UnityEngine;
using TYPE = Data.RoundData.RoundType;

namespace FX {
    public class RoundPopUps : MonoBehaviour {
        // Handle round popups based on round type
        [SerializeField] private AnimatedFX regularRoundPop;
        [SerializeField] private AnimatedFX timeAttackPop;
        [SerializeField] private RoundNumberPopUp roundNumber;

        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            // Hide all popups
            regularRoundPop.gameObject.SetActive(false);
            timeAttackPop.gameObject.SetActive(false);
        }

        public void InitRoundPopup(TYPE type) {
            switch (type) {
                case TYPE.warmup:
                    break;
                case TYPE.normal:
                    regularRoundPop.Init();
                    roundNumber.Init(gd.roundData.currentRound);
                    Invoke(nameof(BeginRound), regularRoundPop.animationLength);
                    break;
                case TYPE.timeAttack:
                    timeAttackPop.Init();
                    Invoke(nameof(BeginRound), timeAttackPop.animationLength);
                    break;
                case TYPE.precision:
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void BeginRound() {
            switch (gd.roundData.roundType) {
                case TYPE.warmup:
                    break;
                case TYPE.normal:
                    gd.roundData.BeginRound();
                    break;
                case TYPE.timeAttack:
                    gd.roundData.BeginTimeAttackRound();
                    break;
                case TYPE.precision:
                    break;
            }
        }
    }
}