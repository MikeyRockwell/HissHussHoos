using Data;
using Utils;
using System;
using UnityEngine;
using Newtonsoft.Json;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;

namespace Managers {
    public class ScoreManager : MonoBehaviour {

        [SerializeField] private int lowScoreThreshold;
        
        private int previousHighScore;
        
        private const string leaderboardID = "highScores";
        private DataWrangler.GameData gd;
        private MoraleData md;
        private Gradient gradient;
        private GradientColorKey[] colorKey;

        private void Awake() {
            gd = DataWrangler.GetGameData();
                
            gd.eventData.OnGameInit.AddListener(NewGame);
            gd.eventData.initMethods++;
            
            gd.eventData.OnGameFirstLaunch.AddListener(ClearSavedScores);
            gd.roundData.OnGameBegin.AddListener(GameBegin);
            gd.eventData.OnHit.AddListener(AddScore);
            gd.eventData.OnHitTimeAttack.AddListener(AddScoreTimeAttack);
            gd.roundData.OnTimeAttackPerfectScore.AddListener(PerfectTimeAttackScoreBonus);
            gd.eventData.OnHitPrecision.AddListener(AddScorePrecision);
            
            gd.eventData.OnGameOver.AddListener(GameOver);
            gd.roundData.OnSpeedBonus.AddListener(AddSpeedBonus);
            md = gd.playerData.md;
        }

        private void NewGame() {
            gd.playerData.punching = false;
            gd.playerData.ResetScore();
            gd.eventData.RegisterCallBack();
        }

        private void GameBegin(int arg) {
            previousHighScore = PlayerPrefs.GetInt("HighScore", 0);
        }

        private void ClearSavedScores() {
            PlayerPrefs.SetInt("HighScore", 0);
            gd.playerData.ResetScore();
        }

        private void AddScore(int arg) {
            int scoreToAdd = md.moraleBoostActive ? 1 * md.moraleBoostScoreMultiplier : 1;
            gd.playerData.UpdateScore(scoreToAdd);
            PlayScorePopUp(scoreToAdd);
        }

        private void AddScoreTimeAttack(int streak) {
            int scoreToAdd = md.moraleBoostActive ? streak * md.moraleBoostScoreMultiplier : streak;
            scoreToAdd = Mathf.Max(Mathf.RoundToInt(scoreToAdd * 0.5f), 1);
            gd.playerData.UpdateScore(scoreToAdd);
            PlayScorePopUp(scoreToAdd);
        }

        private void AddScorePrecision(int index) {
            float scoreToAdd = md.moraleBoostActive ? 10 * md.moraleBoostScoreMultiplier : 10;
            float accuracy = gd.targetData.precisionAccuracy;
            // Multiply the score by the accuracy as a percentage
            scoreToAdd *= accuracy;
            // Round the score to the nearest whole number
            // Add at least 1 point
            int intScoreToAdd = Mathf.RoundToInt(scoreToAdd);
            intScoreToAdd = Mathf.Max(intScoreToAdd, 1);
            gd.playerData.UpdateScore(intScoreToAdd);
            PlayScorePopUp(intScoreToAdd);
        }

        private void PerfectTimeAttackScoreBonus() {
            int scoreBonus = gd.roundData.timeAttackPerfectScoreBonus;
            gd.playerData.UpdateScore(scoreBonus);
            PlayBonusScorePopUp(scoreBonus);
        }

        private void AddSpeedBonus(RoundData.SpeedBonusType bonus) {
            int bonusScore = bonus switch {
                RoundData.SpeedBonusType.fast => 2,
                RoundData.SpeedBonusType.super => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(bonus), bonus, null)
            };

            bonusScore = md.moraleBoostActive ? bonusScore * md.moraleBoostScoreMultiplier : bonusScore;
            gd.playerData.UpdateScore(bonusScore);

            PlayBonusScorePopUp(bonusScore);
        }

        private void PlayScorePopUp(int scoreToAdd) {
            gd.roundData.OnScoreAdded.Invoke(scoreToAdd);
        }

        private void PlayBonusScorePopUp(int scoreToAdd) {
            gd.roundData.OnBonusScoreAdded.Invoke(scoreToAdd);
        }

        private void GameOver() {
            SubmitScore(gd.playerData.score);
            if (gd.playerData.score < lowScoreThreshold) {
                gd.playerData.OnLowScore.Invoke();
            }
            else if (gd.playerData.score > previousHighScore) {
                gd.playerData.OnNewHighScore.Invoke();
            }
            else {
                gd.playerData.OnRegularScore.Invoke();
            }
            gd.playerData.ResetScore();
        }


        public async void SubmitScore(int score) {
            LeaderboardEntry playerEntry = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardID, score);
            Log.Message(JsonConvert.SerializeObject(playerEntry));
        }
    }
}