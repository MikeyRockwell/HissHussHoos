using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Data
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/Data/PlayerData", order = 0)]

    // A scriptable object to hold all the player data and events
    public class PlayerData : ScriptableObject
    {
        [TitleGroup("Stats")] public int score;
        public int health;
        public int maxHealth = 5;

        [TitleGroup("Punches", "Punch Speed")] public float punchSpeed = 0.1f;
        public float punchCoolDown = 0.15f;

        [TitleGroup("SpeedBonus", "Combo Bonuses")] [GUIColor(0.5f, 1, 0.5f)]
        public float superThreshold = 1.8f;

        [GUIColor(0.5f, 1, 0.5f)] public float fastThreshold = 0.08f;
        [GUIColor(0.5f, 1, 0.5f)] public bool punching;

        public MoraleData md;

        [FoldoutGroup("Events", false)] public UnityEvent<int> OnScoreUpdated;
        [FoldoutGroup("Events", false)] public UnityEvent<int> OnHealthChange;
        [FoldoutGroup("Events", false)] public UnityEvent<int> OnHighScoreUpdated;
        [FoldoutGroup("Events", false)] public UnityEvent<int> OnBestRoundUpdated;

        public void ResetScore()
        {
            // Reset the score
            score = 0;
            OnScoreUpdated?.Invoke(score);
            LoadHighScore();
        }

        private void LoadHighScore()
        {
            // Load the high score
            int high = PlayerPrefs.GetInt("HighScore");
            OnHighScoreUpdated?.Invoke(high);
        }

        public void UpdateScore(int addition)
        {
            // Update the score  
            score += addition;
            OnScoreUpdated?.Invoke(score);
            // Check if the score is higher than the current best            
            if (score <= PlayerPrefs.GetInt("HighScore")) return;
            // Update the high score
            OnHighScoreUpdated?.Invoke(score);
            PlayerPrefs.SetInt("HighScore", score);
        }

        public void UpdateRound(int round)
        {
            // Check if the round is higher than the current best
            if (round <= PlayerPrefs.GetInt("BestRound")) return;
            // Update the best round
            PlayerPrefs.SetInt("BestRound", round);
            OnBestRoundUpdated?.Invoke(round);
        }

        public void ResetHealth()
        {
            // Reset the health
            health = maxHealth;
            OnHealthChange?.Invoke(health);
        }

        public void ChangeHealth(int amount)
        {
            // Change the health  
            health += amount;
            OnHealthChange?.Invoke(amount);
        }

        public float GetHealth()
        {
            // Get the health as a percentage
            return (float)health / maxHealth;
        }

        public void CoolDown()
        {
            // Cool down the punch
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(punchCoolDown).OnComplete(() => punching = false);
        }
    }
}