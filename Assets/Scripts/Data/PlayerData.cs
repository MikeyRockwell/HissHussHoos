using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Data {
    
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/Data/PlayerData", order = 0)]
    
    public class PlayerData : ScriptableObject {

        public int score;
        public int health;
        public int maxHealth = 5;
        public float punchSpeed = 0.1f;
        
        [TitleGroup("SpeedBonus")]
        public float superThreshold = 1.8f;
        public float fastThreshold = 0.08f;
        public bool punching;


        public UnityEvent<int> OnScoreUpdated;
        public UnityEvent<int> OnHighScoreUpdated;
        public UnityEvent<int> OnHealthChange;

        public void ResetScore() {
            score = 0;
            OnScoreUpdated?.Invoke(score);
            LoadHighScore();
        }

        private void LoadHighScore() {
            int high = PlayerPrefs.GetInt("HighScore");
            OnHighScoreUpdated?.Invoke(high);
        }
        
        public void UpdateScore(int addition) {
            score += addition;
            OnScoreUpdated?.Invoke(score);
            
            UpdateHighScore();
        }

        private void UpdateHighScore() {
            
            int high = PlayerPrefs.GetInt("HighScore");
            if (score <= high) return;
            
            OnHighScoreUpdated?.Invoke(score);
            PlayerPrefs.SetInt("HighScore", score);
        }

        public void ResetHealth() {
            health = maxHealth;
            OnHealthChange?.Invoke(health);
        }

        public void ChangeHealth(int amount) {
            health += amount;
            OnHealthChange?.Invoke(amount);
        }
        
        public float GetHealth() {
            return (float)health / maxHealth;
        }

        public void CoolDown() {
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(punchSpeed).OnComplete(()=>punching = false);
        }
    }
}