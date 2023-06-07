using Data;
using Utils;
using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using Newtonsoft.Json;
using Unity.Services.Leaderboards;

namespace Managers {
    
    
    public class ScoreManager : MonoBehaviour {
        
        private const string leaderboardID = "highScores";
        private DataWrangler.GameData gd;
        private MoraleData md;
        private Gradient gradient;
        private GradientColorKey[] colorKey;
        
        [SerializeField] private MMF_Player scoreText;
        [SerializeField] private Transform scoreTextPos;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnGameFirstLaunch.AddListener(ClearSavedScores);
            gd.eventData.OnHit.AddListener(AddScore);
            gd.eventData.OnGameInit.AddListener(NewGame);
            gd.eventData.OnGameOver.AddListener(GameOver);
            gd.roundData.OnSpeedBonus.AddListener(AddSpeedBonus);
            md = gd.playerData.md;
        }

        private void NewGame() {
            gd.playerData.ResetScore();
        }
        
        private void ClearSavedScores() {
            PlayerPrefs.SetInt("HighScore", 0);
            gd.playerData.ResetScore();
        }

        private void AddScore(int arg) {
            int scoreToAdd = md.moraleBoostActive ? 1 * md.moraleBoostScoreMultiplier : 1;
            gd.playerData.UpdateScore(scoreToAdd);
            
            PlayScorePopUp(scoreToAdd, Color.red, Color.blue);
        }

      

        private void AddSpeedBonus(RoundData.SpeedBonusType bonus)
        {
            Color color1;
            Color color2;
            int bonusScore;
            int intensity;
            switch (bonus)
            {
                case RoundData.SpeedBonusType.fast:
                    bonusScore = 2;
                    color1 = Color.cyan;
                    color2 = Color.magenta;
                    intensity = 2;
                    break;
                case RoundData.SpeedBonusType.super:
                    color1 = Color.yellow;
                    color2 = Color.red;
                    bonusScore = 3;
                    intensity = 3;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bonus), bonus, null);
            }

            bonusScore = md.moraleBoostActive ? bonusScore * md.moraleBoostScoreMultiplier : bonusScore;
            gd.playerData.UpdateScore(bonusScore);
            PlayScorePopUp(bonusScore, color1, color2, intensity);
        }
        
        private void PlayScorePopUp(int scoreToAdd, Color color1, Color color2, int intensity = 1)
        {
            MMF_FloatingText floatingText = scoreText.GetFeedbackOfType<MMF_FloatingText>();

            // we apply a random value as our display value
            floatingText.Value = "+" + scoreToAdd;

            // we setup some fancy colors
            gradient = new Gradient();
            // Populate the color keys at the relative time 0 and 1 (0 and 100%)
            colorKey = new GradientColorKey[2];
            colorKey[0].color = color1;
            colorKey[0].time = 0.0f;
            colorKey[1].color = color2;
            colorKey[1].time = 1.0f;
            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            var alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 0.0f;
            alphaKey[1].time = 1.0f;
            gradient.SetKeys(colorKey, alphaKey);

            floatingText.ForceColor = true;
            floatingText.AnimateColorGradient = gradient;
            floatingText.Intensity = intensity;

            scoreText.PlayFeedbacks(scoreTextPos.position);
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