using System;
using Managers;
using UnityEngine;
using BONUS = Data.RoundData.SpeedBonusType;

namespace UI {
    public class SpeedTimer : MonoBehaviour {

        [SerializeField] private float maxTime = 3.0f;

        private bool isActive;
        private float currentTime;
        
        private TimeSpan ts;
        private string currentString;

        private DataWrangler.GameData gd;

        private void Awake() {
            
            gd = DataWrangler.GetGameData();
            
            gd.roundData.OnComboBegin.AddListener(StartTimer);
            gd.roundData.OnComboComplete.AddListener(CheckTimer);
            gd.eventData.OnGameOver.AddListener(StopTimer);
        }

        private void StopTimer() {
            isActive = false;
        }

        private void StartTimer(float unused) {
            isActive = true;
            currentTime = maxTime;
        }

        private void CheckTimer() {
            if (currentTime > gd.playerData.superThreshold) {
                isActive = false;
                gd.roundData.SpeedBonus(BONUS.super);
            }
            else if (currentTime < gd.playerData.superThreshold && currentTime > gd.playerData.fastThreshold) {
                isActive = false;
                gd.roundData.SpeedBonus(BONUS.fast);
            }
        }

        private void FixedUpdate() {

            // When active and above 0 - count down the timer
            
            if (!isActive) return;
            
            if (currentTime <= 0) {
                currentTime = 0;
                return;
            }

            currentTime -= Time.deltaTime;
        }
    }
}