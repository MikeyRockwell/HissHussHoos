using Data;
using TMPro;
using System;
using System.Collections;
using Managers;
using DG.Tweening;
using UnityEngine;
using UI.Statistics;
using UnityEngine.UI;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Services.Leaderboards;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards.Models;
using Utils;
using Sequence = DG.Tweening.Sequence;

namespace UI {
    public class LeaderBoard : UIWindow {
        // This class opens and populates the LeaderBoard UI
        private const string leaderBoardId = "highScores";

        [SerializeField] private Transform xf;
        [SerializeField] private Button topScoresTab;

        [SerializeField] private Button yourScoreTab;

        // An array to hold the high score text meshes
        [SerializeField] private Transform uIEntriesParent;
        [SerializeField] private LeaderBoardEntry[] uIEntries;
        [SerializeField] private float animDuration;
        [SerializeField] private float animDelay;

        [SerializeField] private Ease animEase;

        // Scroll rect to hold the high score entries
        [SerializeField] private ScrollRect scrollRect;

        [FormerlySerializedAs("scrollMultiplier")] [SerializeField]
        private float scrollOffset = 0.8f;

        private DataWrangler.GameData gd;
        private string textColor;
        private int playerIndex;
        private bool playerIsInList;

        protected override void Awake() {
            base.Awake();
            gd = DataWrangler.GetGameData();

            // Reconfigure the button subscriptions
            // openWindowButton.onClick.RemoveListener(CheckWindowStatus);
            // openWindowButton.onClick.AddListener(CheckLeaderBoardOpen);
            topScoresTab.onClick.AddListener(GetTopScores);
            yourScoreTab.onClick.AddListener(GetPlayerRange);
            gd.roundData.OnGameBeginDelayed.AddListener(HideMenu);
            gd.eventData.OnGameOver.AddListener(UnHideMenu);

            OnWindowOpen.AddListener(GetPlayerRange);
        }

        private void UnHideMenu() {
            openWindowButton.interactable = true;
            xf.DOKill();
            xf.DOScaleY(1, gd.uIData.MenuAnimSpeed);
        }

        private void HideMenu() {
            openWindowButton.interactable = false;
            xf.DOKill();
            xf.DOScaleY(0, gd.uIData.MenuAnimSpeed);
        }

        private void Start() {
            // Get every child of the uiEntriesParent transform
            // Store them in the uIEntries array
            uIEntries = uIEntriesParent.GetComponentsInChildren<LeaderBoardEntry>();
            foreach (LeaderBoardEntry entry in uIEntries) entry.gameObject.SetActive(false);
        }

        private void CheckLeaderBoardOpen() {
            if (gd.roundData.roundType != RoundData.RoundType.warmup) return;
            // Check if the leaderboard is open
            if (background.gameObject.activeSelf)
                CloseWindow();
            else
                GetPlayerRange();
        }

        private async void GetTopScores() {
            LeaderboardScoresPage scoresResponse = await LeaderboardsService.Instance
                .GetScoresAsync(leaderBoardId, new GetScoresOptions {
                    Offset = 0,
                    Limit = 10
                });

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
            int rangeLimit = 100;
            LeaderboardScores scoresResponse = await LeaderboardsService.Instance
                .GetPlayerRangeAsync(leaderBoardId, new GetPlayerRangeOptions { RangeLimit = rangeLimit }
                );

            FormatScores(scoresResponse.Results);
        }

        private void FormatScores(List<LeaderboardEntry> scoresResponse) {
            playerIsInList = false;
            // Extract the names and scores from the response
            for (int i = 0; i < uIEntries.Length; i++) {
                // Disable the text mesh
                uIEntries[i].gameObject.SetActive(false);
                // If there are no more scores continue
                if (i > scoresResponse.Count - 1) continue;
                // Extract the player name from the full name
                string playerName = "";
                string playerNameFull = scoresResponse[i].PlayerName;
                int index = playerNameFull.IndexOf("#", StringComparison.Ordinal);
                if (index > 0) playerName = playerNameFull[..index];
                // Extract the player number from the full name
                string playerNumber = "";
                if (index > 0) playerNumber = playerNameFull[(index + 1)..];
                // Check if this is the player
                bool isPlayer = playerNameFull == AuthenticationService.Instance.PlayerName;
                if (isPlayer) {
                    playerIndex = i;
                    playerIsInList = true;
                }

                // Initialize the entry
                uIEntries[i].SetEntry(
                    scoresResponse[i].Rank + 1,
                    playerName,
                    playerNumber,
                    (int)scoresResponse[i].Score,
                    isPlayer,
                    uIEntries.Length
                );
            }

            StopAllCoroutines();
            StartCoroutine(nameof(SetVerticalScrollPosition), scoresResponse.Count);
            // AnimateEntries();
        }

        private IEnumerator SetVerticalScrollPosition(int count) {
            yield return null;
            // Scroll to the player's score
            if (!playerIsInList) yield break;

            if (count <= 0) yield break;

            float position = (float)playerIndex / (count - 1);

            // float normalizedPosition = Utils.Conversion.Remap(0, count, 0, 1,position);

            scrollRect.verticalNormalizedPosition = 1f - position;
            Log.Message("Scroll Rect Vertical Normalized Position: " + scrollRect.verticalNormalizedPosition);
        }

        private void AnimateEntries() {
            // Animate each entry scale to 1 in a cascade using DoTween
            Sequence seq = DOTween.Sequence();

            foreach (LeaderBoardEntry entry in uIEntries) {
                entry.transform.DOKill();
                seq.Append(
                    entry.transform.DOScale(1, animDuration));
            }

            // OpenWindow();
        }

        public async void GetPlayerScore() {
            LeaderboardEntry scoreResponse = await LeaderboardsService.Instance
                .GetPlayerScoreAsync(leaderBoardId);
            Debug.Log(JsonConvert.SerializeObject(scoreResponse));
        }
    }
}