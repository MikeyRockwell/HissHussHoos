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
            // If the timer is active and below the superThreshold - give the player a super bonus
            if (elapsedTime < gd.playerData.superThreshold) {
                isActive = false;
                gd.roundData.SpeedBonus(BONUS.super);
            }
            // If the timer is active and above the superThreshold and below the fastThreshold
            // Give the player a fast bonus
            else if (elapsedTime > gd.playerData.superThreshold && elapsedTime < gd.playerData.fastThreshold) {
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