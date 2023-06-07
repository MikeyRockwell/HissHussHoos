using System;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class LeaderBoardEntry : MonoBehaviour {
        
        // This class is used to display the leaderboard entries
        [SerializeField] private TextMeshProUGUI rankText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Image playerBackground;
        [SerializeField] private Gradient rankGradient;
        
        private DataWrangler.GameData gd;
        private RectTransform rect;
        private bool initialized;

        private void Init() {
            rect = GetComponent<RectTransform>();
            gd = DataWrangler.GetGameData();
            initialized = true;
        }

        public void SetEntry(int rank, string playerName, string playerNumber, int score, bool isPlayer, int totalScores) {
            // Check that the class has been initialized
            if (!initialized) Init();
            
            // Set the scale of the entry to zero
            rect.localScale = Vector3.zero;
            
            // Set the color of the player name
            Color playerNameColor = isPlayer ? gd.uIData.HotPink : rankGradient.Evaluate((float)rank / totalScores);
            
            // Set the text of the entry
            rankText.text = rank.ToString();
            rankText.color = rankGradient.Evaluate(rank / 10f);
            
            // Set the player name 
            nameText.text = playerName + " <size=75%>#" + playerNumber + "</size>";
            nameText.color = playerNameColor;
            
            // Set the score
            scoreText.text = score.ToString();
            
            // Set the background color
            playerBackground.color = isPlayer ? gd.uIData.Gold : gd.uIData.DisabledComboText * 0.5f;
            
            gameObject.SetActive(true);
        }
    }
}