using Utils;
using UnityEngine;
using LootLocker.Requests;

namespace Managers {
    
    
    public class ScoreManager : MonoBehaviour {
        
        private const int leaderboardID = 12628;
        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnHit.AddListener(OnHit);
            gd.eventData.OnNewGame.AddListener(NewGame);
            gd.eventData.OnGameOver.AddListener(GameOver);
        }

        private void NewGame() {
            gd.playerData.ResetScore();
        }

        private void OnHit(int arg) {
            gd.playerData.UpdateScore(1);
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