using Data;
using Utils;
using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using Newtonsoft.Json;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine.Serialization;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        private const string leaderboardID = "highScores";
        private DataWrangler.GameData gd;
        private MoraleData md;
        private Gradient gradient;
        private GradientColorKey[] colorKey;

        private void Awake()
        {
            gd = DataWrangler.GetGameData();
            
            gd.eventData.OnGameInit.AddListener(NewGame);
            gd.eventData.initMethods++;
            
            gd.eventData.OnGameFirstLaunch.AddListener(ClearSavedScores);
            gd.eventData.OnHit.AddListener(AddScore);
            gd.eventData.OnHitTimeAttack.AddListener(AddScoreTimeAttack);
            
            
            gd.eventData.OnGameOver.AddListener(GameOver);
            gd.roundData.OnSpeedBonus.AddListener(AddSpeedBonus);
            md = gd.playerData.md;
        }

        private void NewGame()
        {
            gd.playerData.ResetScore();
            gd.eventData.RegisterCallBack();
        }

        private void ClearSavedScores()
        {
            PlayerPrefs.SetInt("HighScore", 0);
            gd.playerData.ResetScore();
        }

        private void AddScore(int arg)
        {
            int scoreToAdd = md.moraleBoostActive ? 1 * md.moraleBoostScoreMultiplier : 1;
            gd.playerData.UpdateScore(scoreToAdd);
            PlayScorePopUp(scoreToAdd);
        }

        private void AddScoreTimeAttack(int streak)
        {
            int scoreToAdd = md.moraleBoostActive ? streak * md.moraleBoostScoreMultiplier : streak;
            gd.playerData.UpdateScore(scoreToAdd);
            PlayScorePopUp(scoreToAdd);    
        }


        private void AddSpeedBonus(RoundData.SpeedBonusType bonus)
        {
            int bonusScore = bonus switch
            {
                RoundData.SpeedBonusType.fast => 2,
                RoundData.SpeedBonusType.super => 3,
                _ => throw new ArgumentOutOfRangeException(nameof(bonus), bonus, null)
            };

            bonusScore = md.moraleBoostActive ? bonusScore * md.moraleBoostScoreMultiplier : bonusScore;
            gd.playerData.UpdateScore(bonusScore);
            
            PlayBonusScorePopUp(bonusScore);
        }

        private void PlayScorePopUp(int scoreToAdd) => gd.roundData.OnScoreAdded.Invoke(scoreToAdd);
        private void PlayBonusScorePopUp(int scoreToAdd) => gd.roundData.OnBonusScoreAdded.Invoke(scoreToAdd);

        private void GameOver()
        {
            SubmitScore(gd.playerData.score);
            gd.playerData.ResetScore();
        }


        public async void SubmitScore(int score)
        {
            LeaderboardEntry playerEntry = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardID, score);
            Log.Message(JsonConvert.SerializeObject(playerEntry));
        }
    }
}