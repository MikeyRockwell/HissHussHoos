using Data;
using UnityEngine;
using Data.Customization;
using Unity.VisualScripting;

namespace Managers {
    
    // This class is used to communicate between the game and the player data
    // For morale points, the player data is the source of truth
    public class MoraleManager : MonoBehaviour {

        [SerializeField] private int moralePointReduction = 25;
        [SerializeField] private int moralePointsEarned;
        
        private DataWrangler.GameData gd;
        private PlayerData pd;
        
        private void Awake() {
            // Cache components
            gd = DataWrangler.GetGameData();
            pd = gd.playerData;
            // Subscribe to events
            gd.customEvents.OnItemUnlocked.AddListener(SpendMoralPoints);
            gd.eventData.OnHit.AddListener(AddMoraleFromPunch);
            gd.eventData.OnMiss.AddListener(RemoveMorale);
            gd.roundData.OnSpeedBonus.AddListener(AddMoraleFromBonus);
            gd.eventData.OnGameOver.AddListener(DisplayMoralePoints);
            
            pd.ResetMorale();
            // Trigger the update morale event
            gd.uIData.UpdateMoraleUI(pd.GetMorale());
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
            int moraleAdded = Mathf.RoundToInt(pd.moraleBoostActive ? morale * pd.moraleBoostMultiplier : morale);
            // Update the morale points to a maximum of 100
            pd.morale = Mathf.Min(pd.maxMorale, pd.morale + moraleAdded);
            // Update the morale points earned
            moralePointsEarned += moraleAdded;
            pd.moralePoints += moraleAdded;
            // Trigger the update morale event
            gd.uIData.UpdateMoraleUI(pd.GetMorale());

            // If the player has reached the maximum morale, trigger the morale boost event
            if (pd.morale != pd.maxMorale) return;
            // If the morale boost is already active, return
            if (pd.moraleBoostActive) return;
            
            pd.moraleBoostActive = true;
            pd.OnMoraleBoost?.Invoke();
            // Start a timer to reset the morale
            Invoke(nameof(EndMoraleBoost), pd.moraleBoostDuration);
        }
        
        private void RemoveMorale() {
            if (pd.moraleBoostActive) return;
            // Remove morale points to a minimum of zero
            pd.morale = Mathf.Max(0, pd.morale - moralePointReduction);
            // Trigger the update morale event
            gd.uIData.UpdateMoraleUI(pd.GetMorale());
        }
        
        private void EndMoraleBoost() {
            // Reset the morale points to zero
            pd.ResetMorale();
            pd.OnMoraleBoostEnd?.Invoke();
            // Trigger the update morale event
            gd.uIData.UpdateMoraleUI(pd.GetMorale());
        }
        
        private void DisplayMoralePoints() {
            // Display the morale points earned
            gd.uIData.DisplayMoralePoints(moralePointsEarned);
        }

        private void SpendMoralPoints(SO_Item item) {
            // Spend morale points
            pd.moralePoints -= item.price;
        }
    }
}