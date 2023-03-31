using Data;
using TMPro;
using DG.Tweening;
using UnityEngine;

namespace Managers {
    public class RoundManager : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI roundText;

        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.roundData.OnGameBegin.AddListener(NewGame);
            gd.roundData.OnRoundComplete.AddListener(NewRoundInit);
            gd.roundData.OnBonusRoundComplete.AddListener(NewRoundInit);
            gd.eventData.OnGameOver.AddListener(GameOver);
        }

        private void GameOver() {
            gd.roundData.roundType = RoundData.RoundType.warmup;
            roundText.text = "";
        }

        private void Start() {
            // Hide round text
            roundText.text = "";
            // Set warmup round type for free play
            gd.roundData.roundType = RoundData.RoundType.warmup;
        }

        private void NewGame(int round) {
            // Called when start game button is pushed
            gd.roundData.currentRound = round;
            NewRoundInit(round);
        }

        private void NewRoundInit(int round) {
            // Begins a new round
            // Handles both regular and bonus rounds
            Sequence seq = DOTween.Sequence();
            seq.PrependInterval(gd.roundData.roundTextTime).OnComplete(() => SetRoundType(round));
        }

        private void SetRoundType(int round) {
            // Check for bonus round
            if (round % gd.roundData.bonusRound == 0) {
                BeginBonusRound(round);
            }
            else {
                BeginRegularRound(round);
            }
        }

        private void BeginRegularRound(int round) {
            // This starts a new round
            gd.roundData.roundType = RoundData.RoundType.normal;
            gd.roundData.BeginRound();
            roundText.text = "ROUND: " + round;
        }

        private void BeginBonusRound(int round) {
            // Starts a bonus round
            gd.roundData.roundType = RoundData.RoundType.bonus;
            gd.roundData.BeginBonusRound();
            roundText.text = "BONUS ROUND";
        }

    }
}