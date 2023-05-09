using System;
using Managers;
using TMPro;
using UnityEngine;

namespace UI {
    public class LeaderBoardEntry : MonoBehaviour {
        
        // This class is used to display the leaderboard entries
        [SerializeField] private TextMeshProUGUI rankText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI scoreText;
        
        [SerializeField] private Gradient rankGradient;
        
        private DataWrangler.GameData gd;
        private RectTransform rect;
        private bool initialized;

        private void Init() {
            rect = GetComponent<RectTransform>();
            gd = DataWrangler.GetGameData();
            initialized = true;
        }

        public void SetEntry(int rank, string playerName, int score, bool isPlayer) {
            // Check that the class has been initialized
            if (!initialized) Init();
            
            // Set the scale of the entry to zero
            rect.localScale = Vector3.zero;
            
            // Set the color of the player name
            Color playerNameColor = isPlayer ? gd.uIData.Gold : gd.uIData.HotPink;
            
            // Set the text of the entry
            rankText.text = rank.ToString();
            rankText.color = rankGradient.Evaluate(rank / 10f);
            
            // Set the player name 
            nameText.text = playerName;
            nameText.color = playerNameColor;
            
            // Set the score
            scoreText.text = score.ToString();

            gameObject.SetActive(true);
        }
    }
}