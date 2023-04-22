﻿using TMPro;
using Managers;
using UnityEngine;

namespace UI.Statistics {
    public class PlayerStatistics : MonoBehaviour {
        
        // This script updates the player statistics window
        [SerializeField] private TextMeshProUGUI bestRoundText;
        [SerializeField] private TextMeshProUGUI fastestAverageText;
        [SerializeField] private TextMeshProUGUI lastAverageText;
        [SerializeField] private TextMeshProUGUI[] comboSpeedTexts;
        [SerializeField] private float[] comboSpeeds = new float[10];
        
        private readonly string highestRound = "HIGHEST ROUND: ";
        
        private DataWrangler.GameData gd;

        private void Awake() {
            // Get the game data and sub to events
            gd = DataWrangler.GetGameData();
            gd.roundData.OnLogTimer.AddListener(LogComboSpeed);
            gd.roundData.OnRoundComplete.AddListener(UpdateAverage);
            gd.playerData.OnBestRoundUpdated.AddListener(UpdateBestRound);
            
            InitializeTextObjects();
        }

        private void InitializeTextObjects() {
            for (int i = 0; i < comboSpeedTexts.Length; i++) {
                comboSpeedTexts[i].text = $"<mspace=25>COMBO {i + 1:00}: ";
            }
            bestRoundText.text = highestRound + $"{PlayerPrefs.GetInt("BestRound", 0):000}";
            fastestAverageText.text =
                $"FASTEST AVERAGE COMBO: {PlayerPrefs.GetFloat("FastestAverageComboSpeed", 0):00.00}";
            lastAverageText.text = $"LAST ROUND AVERAGE: {0:00.00}";
        }

        private void LogComboSpeed(int comboNumber, float comboSpeed) {
            comboSpeeds[comboNumber] = comboSpeed;    
            comboSpeedTexts[comboNumber].gameObject.SetActive(true);
            comboSpeedTexts[comboNumber].text = $"<mspace=25>COMBO {comboNumber + 1:00}: {comboSpeed:00.00}";
            comboSpeedTexts[comboNumber].color = gd.uIData.LaserGreen;
            // Set the previous combo speed text color to yellow
            if (comboNumber > 0) {
                comboSpeedTexts[comboNumber - 1].color = gd.uIData.DisabledComboText;
            }
            // If the combo number is 0, set the last combo speed text color to yellow
            else if (gd.roundData.currentRound != 2) {
                comboSpeedTexts[^1].color = gd.uIData.DisabledComboText;
            }
        }
        
        private void UpdateBestRound(int bestRound) {
            bestRoundText.text = highestRound + $"{bestRound:000}";
        }
        
        private void UpdateAverage(int roundNumber) {
            float average = 0;
            int count = 0;
            for (int i = 0; i < comboSpeeds.Length; i++) {
                if (comboSpeeds[i] != 0) {
                    average += comboSpeeds[i];
                    count++;
                }
            }
            average /= count;
            lastAverageText.text = $"LAST ROUND AVERAGE: {average:00.00}";
            
            // If the average is faster than the fastest average, update the fastest average
            if (!(average < PlayerPrefs.GetFloat("FastestAverageComboSpeed", 0))) return;
            
            PlayerPrefs.SetFloat("FastestAverageComboSpeed", average);
            fastestAverageText.text = $"FASTEST AVERAGE COMBO: {average:00.00}";
        }
    }
}