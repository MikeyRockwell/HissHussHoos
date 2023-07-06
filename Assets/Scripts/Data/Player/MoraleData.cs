using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Utils;

namespace Data
{
    [CreateAssetMenu(fileName = "MoraleData", menuName = "ScriptableObjects/Data/MoraleData", order = 0)]
    public class MoraleData : ScriptableObject
    {
        [TitleGroup("Morale", "Moraaaaale")] 
        public float moralePoints;
        public int morale;
        public int maxMorale = 100;
        public float moraleBoostDuration;
        public int moraleBoostScoreMultiplier = 3;
        public float moralePointsEarned;
        public bool moraleBoostActive;

        [FoldoutGroup("Events", false)] public UnityEvent OnMoraleBoost;
        [FoldoutGroup("Events", false)] public UnityEvent OnMoraleBoostEnd;
        [FoldoutGroup("Events", false)] public UnityEvent<float> OnMoraleUpdated;
        [FoldoutGroup("Events", false)] public UnityEvent OnMoralePointsEarned;
        [FoldoutGroup("Events", false)] public UnityEvent<float, float> OnMoralePointsSpent;

        public void ResetMoralePoints()
        {
            // Called on first launch
            Log.Message("Resetting Morale Points");
            moralePoints = 0;
            SaveMoralePoints();
        }

        public void UpdateMoraleMeter(float newMorale)
        {
            OnMoraleUpdated?.Invoke(newMorale);
        }

        public void DisplayMoralePoints()=> OnMoralePointsEarned?.Invoke();

        public void SpendMoralePoints(int moraleSpent)
        {
            OnMoralePointsSpent?.Invoke(moralePoints, moraleSpent);
            moralePoints -= moraleSpent;
            SaveMoralePoints();
        }

        public float LoadMoralePoints()
        {
            // Called on game init
            moralePoints = PlayerPrefs.GetFloat("MoralePoints");
            return moralePoints;
        }

        public void ResetMorale()
        {
            // Not morale points but morale itself
            morale = 0;
            // moralePointsEarned = 0;
            moraleBoostActive = false;
        }

        public void UpdateMoralePoints(float addition)
        {
            moralePoints += addition;
            PlayerPrefs.SetFloat("MoralePoints", moralePoints);
        }

        public float GetMorale()
        {
            // Get the morale as a percentage
            return (float)morale / maxMorale;
        }

        public void SaveMoralePoints()
        {
            // Write the morale points to the player prefs
            PlayerPrefs.SetFloat("MoralePoints", moralePoints);
        }
    }
}