using TMPro;
using Managers;
using UnityEngine;

namespace UI {
    public class ScoreGraphics : MonoBehaviour {

        private DataWrangler.GameData gd;

        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.playerData.OnScoreUpdated.AddListener(UpdateScore);
            gd.playerData.OnHighScoreUpdated.AddListener(UpdateHighScore);
        }

        private void UpdateScore(int score) {
            scoreText.text = "SCORE: " + score;
        }
        
        private void UpdateHighScore(int highScore) {
            highScoreText.text = "PERSONAL BEST: " + highScore;
        }
    }
}