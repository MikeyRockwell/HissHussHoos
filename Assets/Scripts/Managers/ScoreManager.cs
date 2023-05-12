using Data;
using Utils;
using System;
using UnityEngine;
using Newtonsoft.Json;
using Unity.Services.Leaderboards;

namespace Managers {
    
    
    public class ScoreManager : MonoBehaviour {
        
        private const string leaderboardID = "highScores";
        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnGameFirstLaunch.AddListener(ClearSavedScores);
            gd.eventData.OnHit.AddListener(AddScore);
            gd.eventData.OnGameInit.AddListener(NewGame);
            gd.eventData.OnGameOver.AddListener(GameOver);
            gd.roundData.OnSpeedBonus.AddListener(AddSpeedBonus);
        }

        private void NewGame() {
            gd.playerData.ResetScore();
        }
        
        private void ClearSavedScores() {
            PlayerPrefs.SetInt("HighScore", 0);
            gd.playerData.ResetScore();
        }

        private void AddScore(int arg) {
            gd.playerData.UpdateScore(1);
        }
        
        private void AddSpeedBonus(RoundData.SpeedBonusType bonus) {
            int bonusScore = bonus switch {
                RoundData.SpeedBonusType.fast => 2,
                RoundData.SpeedBonusType.super => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(bonus), bonus, null)
            };
            gd.playerData.UpdateScore(bonusScore);
        }

        private void GameOver() {
            SubmitScore(gd.playerData.score);
            gd.playerData.ResetScore();
        }

        public async void SubmitScore(int score) {
            var playerEntry = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardID, score);
            Log.Message(JsonConvert.SerializeObject(playerEntry));
        }
    }
}