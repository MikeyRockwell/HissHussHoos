using Data;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Managers {
    public class RoundManager : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI roundText;
        [SerializeField] private Button readyButton;

        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.roundData.OnGameBegin.AddListener(NewGame);
            gd.roundData.OnRoundInit.AddListener(NewRoundInit);
            gd.eventData.OnGameOver.AddListener(GameOver);
            
            gd.roundData.OnRoundComplete.AddListener(_=> gd.roundData.InitRound());
            
            // Ready Button is currently disabled
            // gd.roundData.OnRoundComplete.AddListener(EnableReadyButton);
            // readyButton.onClick.AddListener(() => gd.roundData.InitRound());
        }

        /*
        private void EnableReadyButton(int arg0) {
            readyButton.gameObject.SetActive(true);
        }*/

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
            
            // After a delay, set the round type
            Sequence seq = DOTween.Sequence();
            seq.PrependInterval(gd.roundData.roundTextTime).OnComplete(() => SetRoundType(round));
            
            // readyButton.gameObject.SetActive(false);
        }

        private void SetRoundType(int round) {
            
            // Demo currently only plays regular rounds
            BeginRegularRound(round);
            
            // Check for bonus round
            /*  if (round % gd.roundData.bonusRound == 0) {
                BeginBonusRound(round);
            }
            else {
                BeginRegularRound(round);
            } */
        }

        private void BeginRegularRound(int round) {
            // This starts a new round
            gd.roundData.roundType = RoundData.RoundType.normal;
            gd.roundData.BeginRound();
            roundText.text = "ROUND " + $"{round:00}";
            // Send round to check for best round
            gd.playerData.UpdateRound(round);
        }

        private void BeginBonusRound(int round) {
            // Starts a bonus round
            gd.roundData.roundType = RoundData.RoundType.bonus;
            gd.roundData.BeginBonusRound();
            roundText.text = "BONUS ROUND";
        }

    }
}