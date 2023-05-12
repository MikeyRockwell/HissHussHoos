using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Utils;

namespace Data {
    [CreateAssetMenu(fileName = "MoraleData", menuName = "ScriptableObjects/Data/MoraleData", order = 0)]
    public class MoraleData : ScriptableObject {
        
        [TitleGroup("Morale", "Moraaaaale")]
        public int moralePoints;
        public int morale;
        public int maxMorale = 100;
        public float moraleBoostDuration;
        public float moraleBoostMultiplier;
        public bool moraleBoostActive;
        
        [FoldoutGroup("Events", false)] public UnityEvent OnMoraleBoost;
        [FoldoutGroup("Events", false)] public UnityEvent OnMoraleBoostEnd;
        [FoldoutGroup("Events", false)] public UnityEvent<float> OnMoraleUpdated;
        [FoldoutGroup("Events", false)] public UnityEvent<int> OnMoralePointsEarned;
        [FoldoutGroup("Events", false)] public UnityEvent<int, int> OnMoralePointsSpent;
        
        public void ResetMoralePoints() {
            // Called on first launch
            moralePoints = 0;
            SaveMoralePoints();
        }
        
        public void UpdateMoraleMeter(float newMorale) {
            OnMoraleUpdated?.Invoke(newMorale);
        }
        
        public void DisplayMoralePoints(int moraleEarned) {
            OnMoralePointsEarned?.Invoke(moraleEarned);
        }
        
        public void SpendMoralePoints(int moraleSpent) {
            OnMoralePointsSpent?.Invoke(moralePoints, moraleSpent);
            moralePoints -= moraleSpent;
            SaveMoralePoints();
        }

        public int LoadMoralePoints() {
            // Called on game init
            moralePoints = PlayerPrefs.GetInt("MoralePoints");
            return moralePoints;
        }

        public void ResetMorale() {
            // Not morale points but morale itself
            morale = 0;
            moraleBoostActive = false;
        }

        public void UpdateMoralePoints(int addition) {
            moralePoints += addition;
            PlayerPrefs.SetInt("MoralePoints", moralePoints);
        }

        public float GetMorale() {
            // Get the morale as a percentage
            return (float)morale / maxMorale;
        }

        private void SaveMoralePoints() {
            // Write the morale points to the player prefs
            PlayerPrefs.SetInt("MoralePoints", moralePoints);
        }
    }
}