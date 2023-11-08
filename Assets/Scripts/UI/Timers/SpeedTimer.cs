using Utils;
using System;
using Managers;
using UnityEngine;
using BONUS = Data.RoundData.SpeedBonusType;

namespace UI {
    public class SpeedTimer : MonoBehaviour {
        
        private bool isActive;
        private float elapsedTime;
        private int roundStep;
        private TimeSpan ts;
        private string currentString;

        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();

            gd.roundData.OnComboBegin.AddListener(StartTimer);
            gd.roundData.OnComboComplete.AddListener(LogTimer);
            gd.eventData.OnGameOver.AddListener(StopTimer);
        }

        private void StopTimer() {
            isActive = false;
        }

        private void StartTimer(float unused) {
            isActive = true;
            roundStep = gd.roundData.roundStep;
            elapsedTime = 0;
        }

        // A function to check the timer and give the player a bonus
        private void LogTimer() {
            CheckSpeedBonus();
            gd.roundData.LogTimer(roundStep, elapsedTime);
            StopTimer();
        }

        private void CheckSpeedBonus() {

            float adjustedSuperThreshold = gd.playerData.superThreshold;
            float adjustedFastThreshold  = gd.playerData.fastThreshold;
            
            // Adjust the thresholds based on the current amount of targets
            switch (gd.targetData.currentSet.Length) {
                case 1:
                    adjustedSuperThreshold = gd.playerData.superThreshold * gd.targetData.oneHitComboTimeMult;
                    adjustedFastThreshold  = gd.playerData.fastThreshold  * gd.targetData.oneHitComboTimeMult;
                    break;
                case 3:
                    adjustedSuperThreshold = gd.playerData.superThreshold;
                    adjustedFastThreshold  = gd.playerData.fastThreshold;
                    break;
                case 5:
                    adjustedSuperThreshold = gd.playerData.superThreshold * gd.targetData.fiveHitComboTimeMult;
                    adjustedFastThreshold  = gd.playerData.fastThreshold  * gd.targetData.fiveHitComboTimeMult;
                    break;
            }
            
            Log.Message("Adjusted Super Threshold: " + adjustedSuperThreshold, Color.green);
            Log.Message("Adjusted Fast Threshold:  " + adjustedFastThreshold,  Color.magenta);
            
            // If the timer is active and below the superThreshold - give the player a super bonus
            if (elapsedTime < adjustedSuperThreshold) {
                isActive = false;
                gd.roundData.SpeedBonus(BONUS.super);
            }
            
            // If the timer is active and above the superThreshold and below the fastThreshold
            // Give the player a fast bonus
            else if (elapsedTime > adjustedSuperThreshold && elapsedTime < adjustedFastThreshold) {
                isActive = false;
                gd.roundData.SpeedBonus(BONUS.fast);
            }
        }

        private void FixedUpdate() {
            // When active and below maxTime - count up the timer
            if (!isActive) return;

            elapsedTime += Time.deltaTime;
        }
    }
}