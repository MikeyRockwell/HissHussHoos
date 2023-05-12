using TMPro;
using System;
using Managers;
using DG.Tweening;
using UnityEngine;
using UI.Statistics;
using UnityEngine.UI;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using System.Collections.Generic;
using Data;
using Unity.Services.Leaderboards;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards.Models;
using Sequence = DG.Tweening.Sequence;

namespace UI {
    public class LeaderBoard : UIWindow {
        
        // This class opens and populates the LeaderBoard UI
        private const string leaderBoardId = "highScores";
        
        [SerializeField] private Button topScoresTab;
        [SerializeField] private Button yourScoreTab;
        // An array to hold the high score text meshes
        [SerializeField] private LeaderBoardEntry[] uIEntries;
        [SerializeField] private float animDuration;
        [SerializeField] private float animDelay;
        [SerializeField] private Ease animEase;
        
        private DataWrangler.GameData gd;
        private String textColor;

        protected override void Awake() {
            base.Awake();
            gd = DataWrangler.GetGameData();
            
            // Reconfigure the button subscriptions
            openWindowButton.onClick.RemoveListener(CheckWindowStatus);

            openWindowButton.onClick.AddListener(CheckLeaderBoardOpen);
            topScoresTab.onClick.AddListener(GetTopScores);
            yourScoreTab.onClick.AddListener(GetPlayerRange);
        }

        private void CheckLeaderBoardOpen() {
            if (gd.roundData.roundType != RoundData.RoundType.warmup) {
                return;
            }
            // Check if the leaderboard is open
            if (background.gameObject.activeSelf) {
                CloseWindow();
            }
            else {
                GetPlayerRange();
            }
        }

        private async void GetTopScores() {
            LeaderboardScoresPage scoresResponse = await LeaderboardsService.Instance
                .GetScoresAsync(leaderBoardId);
            FormatScores(scoresResponse.Results);
        }

        public async void GetPlayerRange() {
            // Check if the player has a score on the leaderboard
            try {
                LeaderboardEntry playerScore = await LeaderboardsService.Instance
                    .GetPlayerScoreAsync(leaderBoardId);
            }
            catch {
                GetTopScores();
                return;
            }

            // Returns a total of 11 entries (the given player plus 5 on either side)
            var rangeLimit = uIEntries.Length - 1;
            LeaderboardScores scoresResponse = await LeaderboardsService.Instance
                .GetPlayerRangeAsync(leaderBoardId, new GetPlayerRangeOptions{ RangeLimit = rangeLimit }
                );
            FormatScores(scoresResponse.Results);
        }

        private void FormatScores(List<LeaderboardEntry> scoresResponse) {
            // Extract the names and scores from the response
            for (int i = 0; i < uIEntries.Length; i++) {
                
                // Disable the text mesh
                uIEntries[i].gameObject.SetActive(false);
                // If there are no more scores continue
                if (i > scoresResponse.Count-1) {
                    continue;
                }
                // Extract the player name from the full name
                string playerName = "";
                string playerNameFull = scoresResponse[i].PlayerName;
                int index = playerNameFull.IndexOf("#", StringComparison.Ordinal);
                if (index > 0) {
                    playerName = playerNameFull[..index];
                }
                // Check if this is the player
                bool isPlayer = playerNameFull == AuthenticationService.Instance.PlayerName;
                // Initialize the entry
                uIEntries[i].SetEntry(
                        rank: scoresResponse[i].Rank+1,
                        playerName: playerName,
                        score: (int)scoresResponse[i].Score,
                        isPlayer: isPlayer
                    );
            }

            AnimateEntries();
        }

        private void AnimateEntries() {
            // Animate each entry scale to 1 in a cascade using DoTween
            Sequence seq = DOTween.Sequence();

            foreach (LeaderBoardEntry entry in uIEntries) {
                entry.transform.DOKill();
                seq.Append(entry.transform.DOScale(1, animDuration)).SetEase(animEase);
            }

            OpenWindow();
        }

        public async void GetPlayerScore() {
            var scoreResponse = await LeaderboardsService.Instance
                .GetPlayerScoreAsync(leaderBoardId);
            Debug.Log(JsonConvert.SerializeObject(scoreResponse));
        }
    }
}