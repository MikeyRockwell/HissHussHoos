﻿using Data;
using UnityEngine;
using Data.Customization;
using Utils;

namespace Managers {
    
    // This class is used to communicate between the game and the player data
    // For morale points, the player data is the source of truth
    public class MoraleManager : MonoBehaviour {

        [SerializeField] private int moralePointReduction = 25;
        [SerializeField] private int moralePointsEarned;
        
        private DataWrangler.GameData gd;
        private MoraleData moraleData;

        private void Awake() {
            // Cache components
            gd = DataWrangler.GetGameData();
            moraleData = gd.playerData.md;
            SubscribeEvents();

            // Initialize morale points
            moraleData.ResetMorale();
            moraleData.UpdateMoraleMeter(moraleData.GetMorale());
        }

        private void SubscribeEvents() {
            // Subscribe to events
            gd.eventData.OnGameFirstLaunch.AddListener(moraleData.ResetMoralePoints);
            gd.customEvents.OnItemUnlocked.AddListener(SpendMoralPoints);
            gd.eventData.OnHit.AddListener(AddMoraleFromPunch);
            gd.eventData.OnMiss.AddListener(RemoveMorale);
            gd.roundData.OnSpeedBonus.AddListener(AddMoraleFromBonus);
            gd.eventData.OnGameOver.AddListener(DisplayMoralePoints);
        }

        private void AddMoraleFromPunch(int unused) {
            AddMorale(1);
        }

        private void AddMoraleFromBonus(RoundData.SpeedBonusType arg0) {
            // Add morale points based on the type of speed bonus
            int moralePoints = arg0 switch {
                RoundData.SpeedBonusType.fast => 2,
                RoundData.SpeedBonusType.super => 3,
                _ => 0
            };
            AddMorale(moralePoints);
        }

        public void AddMorale(int morale) {
            // If morale boost is active multiply the amount of morale by the boost multiplier
            int moraleAdded = Mathf.RoundToInt(
                moraleData.moraleBoostActive ? morale * moraleData.moraleBoostMultiplier : morale);
            // Update the morale points to a maximum of 100
            moraleData.morale = Mathf.Min(moraleData.maxMorale, moraleData.morale + moraleAdded);
            // Update the morale points earned
            moralePointsEarned += moraleAdded;
            moraleData.UpdateMoralePoints(morale);
            // Trigger the update morale event
            moraleData.UpdateMoraleMeter(moraleData.GetMorale());
            // If the player has reached the maximum morale, trigger the morale boost event
            if (moraleData.morale != moraleData.maxMorale) return;
            // If the morale boost is already active, return
            if (moraleData.moraleBoostActive) return;
            
            // Trigger the morale boost event
            moraleData.moraleBoostActive = true;
            moraleData.OnMoraleBoost?.Invoke();
            Log.Message("Morale boost activated!", gd.uIData.HotPink);
            // Start a timer to reset the morale
            Invoke(nameof(EndMoraleBoost), moraleData.moraleBoostDuration);
        }
        
        private void RemoveMorale() {
            if (moraleData.moraleBoostActive) return;
            // Remove morale points to a minimum of zero
            moraleData.morale = Mathf.Max(0, moraleData.morale - moralePointReduction);
            // Trigger the update morale event
            moraleData.UpdateMoraleMeter(moraleData.GetMorale());
        }
        
        private void EndMoraleBoost() {
            Log.Message("Morale boost ended!", gd.uIData.LaserGreen);
            // Reset the morale points to zero
            moraleData.ResetMorale();
            moraleData.OnMoraleBoostEnd?.Invoke();
            // Trigger the update morale event
            moraleData.UpdateMoraleMeter(moraleData.GetMorale());
        }
        
        private void DisplayMoralePoints() {
            // Display the morale points earned
            moraleData.DisplayMoralePoints(moralePointsEarned);
        }

        private void SpendMoralPoints(SO_Item item) {
            // Spend morale points
            moraleData.SpendMoralePoints(item.price);
            DataWrangler.GetSaverLoader().SaveGame();
        }
    }
}