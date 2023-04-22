using Data;
using Utils;
using System;
using UnityEngine;
using LootLocker.Requests;

namespace Managers {
    
    
    public class ScoreManager : MonoBehaviour {
        
        private const int leaderboardID = 12628;
        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnHit.AddListener(AddScore);
            gd.eventData.OnGameInit.AddListener(NewGame);
            gd.eventData.OnGameOver.AddListener(GameOver);
            gd.roundData.OnSpeedBonus.AddListener(AddSpeedBonus);
        }

        private void NewGame() {
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
        }

        private void SubmitScore(int scoreToUpload) {
            
            string playerID = PlayerPrefs.GetString("PlayerID");
            LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, leaderboardID, (response) => 
                {
                    if (response.success) {
                        Log.Message("Successfully uploaded score");
                    }
                    else {
                        Log.Message("Failed " + response.Error);
                    }
                }
            );
        }
    }
}