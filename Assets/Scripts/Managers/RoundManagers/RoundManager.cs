using FX;
using Data;
using TMPro;
using Utils;
using System;
using UnityEngine;
using System.Collections;

namespace Managers {
    public class RoundManager : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI roundText;
        [SerializeField] private RoundPopUps roundPops;

        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.roundData.OnGameBegin.AddListener(NewGame);
            gd.roundData.OnRoundInit.AddListener(NewRoundInit);
            gd.roundData.OnRoundBegin.AddListener(BeginRound);
            gd.roundData.OnRoundComplete.AddListener(_ => gd.roundData.InitRound());
            gd.roundData.OnTimeAttackRoundComplete.AddListener(_ => gd.roundData.InitRound());
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
            gd.roundData.roundActive = false;
        }

        private void NewGame(int round) {
            // Called when start game button is pushed
            // The round is always set to 1??
            gd.roundData.currentRound = round;
            NewRoundInit(round);
        }

        private void NewRoundInit(int round) {
            // Happens before the round begins
            // Sets the round type and round number
            // Then the popup can happen, then the round begins at the end of the popup
            // We will call round begin from the popup when it ends
            StartCoroutine(nameof(InitRoundSequence));
        }

        private IEnumerator InitRoundSequence() {
            gd.eventData.inputEnabled = false;
            gd.roundData.roundActive = false;
            int round = gd.roundData.currentRound;
            SelectRoundType(round);
            roundPops.InitRoundPopup(gd.roundData.roundType);
            yield return new WaitForSeconds(gd.roundData.roundTextTime);
            BeginRound(round);
            gd.roundData.roundActive = true;
        }

        private void SelectRoundType(int round) {
            // Check if the current round is a time attack round
            if (round % gd.roundData.timeAttackRoundDivisor == 0)
                gd.roundData.roundType = RoundData.RoundType.timeAttack;
            // else if (round % gd.roundData.precisionRoundDivisor == 0) {
            //     gd.roundData.roundType = RoundData.RoundType.precision;
            // }
            else
                gd.roundData.roundType = RoundData.RoundType.normal;
        }

        private void BeginRound(int round) {
            switch (gd.roundData.roundType) {
                case RoundData.RoundType.warmup:
                    break;
                case RoundData.RoundType.normal:
                    BeginRegularRound(round);
                    break;
                case RoundData.RoundType.timeAttack:
                    BeginTimeAttackRound();
                    break;
                case RoundData.RoundType.precision:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void BeginRegularRound(int round) {
            // Starts a regular round
            gd.eventData.inputEnabled = true;
            roundText.text = "ROUND " + $"{round:00}";
            // Send round to check for best round
            gd.playerData.UpdateRound(round);
        }

        private void BeginTimeAttackRound() {
            // Starts a time attack round
            gd.eventData.inputEnabled = true;
            roundText.text = "TIME ATTACK";
            gd.playerData.UpdateRound(gd.roundData.currentRound);
        }
    }
}