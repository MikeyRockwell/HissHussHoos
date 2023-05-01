using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UI.Statistics;
using Newtonsoft.Json;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;

namespace UI {
    public class LeaderBoard : UIWindow {
        
        // This class opens and populates the LeaderBoard UI
        private const string leaderBoardId = "highScores";
        // An array to hold the high scores
        
        [SerializeField] private TextMeshProUGUI[] scoreTextMeshes;

        private DataWrangler.GameData gd;
        private String textColor;

        protected override void Awake() {
            base.Awake();
            openWindowButton.onClick.AddListener(GetPlayerRange);
            gd = DataWrangler.GetGameData();
        }

        private async void GetTopScores() {
            LeaderboardScoresPage scoresResponse = await LeaderboardsService.Instance
                .GetScoresAsync(leaderBoardId);
            FormatScores(scoresResponse.Results);
        }

        public async void GetPlayerRange() {
            // Returns a total of 11 entries (the given player plus 5 on either side)
            var rangeLimit = 5;
            LeaderboardScores scoresResponse = await LeaderboardsService.Instance
                .GetPlayerRangeAsync(leaderBoardId, new GetPlayerRangeOptions{ RangeLimit = rangeLimit }
                );
            FormatScores(scoresResponse.Results);
        }

        private void FormatScores(List<LeaderboardEntry> scoresResponse) {
            
            // Extract the names and scores from the response
            for (int i = 0; i < scoreTextMeshes.Length; i++) {
                // Disable the text mesh
                scoreTextMeshes[i].transform.parent.gameObject.SetActive(false);
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

                textColor = playerNameFull == AuthenticationService.Instance.PlayerName
                    ? ColorUtility.ToHtmlStringRGBA(gd.uIData.Gold)
                    : ColorUtility.ToHtmlStringRGBA(gd.uIData.HotPink);
                
                TextMeshProUGUI textMesh = scoreTextMeshes[i];
                textMesh.text = "<color=#" + textColor + ">" + 
                                $"<mspace=32>{scoresResponse[i].Rank+1}. " + 
                                playerName + " - " +
                                // $"{scoresResponse.Results[i].PlayerName} - " +
                                $"{scoresResponse[i].Score}";
                textMesh.transform.parent.gameObject.SetActive(true);
            }
        }
        
        public async void GetPlayerScore() {
            var scoreResponse = await LeaderboardsService.Instance
                .GetPlayerScoreAsync(leaderBoardId);
            Debug.Log(JsonConvert.SerializeObject(scoreResponse));
        }
    }
}