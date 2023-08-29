using FX;
using Data;
using UnityEngine;
using Data.Customization;
using System.Collections;
using UnityEngine.Serialization;
using Utils;

namespace Managers {
    // This class is used to communicate between the game and the player data
    // For morale points, the player data is the source of truth
    public class MoraleManager : MonoBehaviour {
        
        [SerializeField] private int moraleReduction = 25;
        [SerializeField] private float moralePointsMultiplier = 0.3f;
        [SerializeField] private ScalePulse moraleBoostText;
        [SerializeField] private VolumeFX moraleFXVolume;

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
            gd.roundData.OnGameBegin.AddListener(i => ResetMorale());
            gd.customEvents.OnItemUnlocked.AddListener(SpendMoralPoints);
            gd.eventData.OnHit.AddListener(AddMoraleFromPunch);
            gd.eventData.OnHitTimeAttack.AddListener(AddMoraleFromPunch);
            gd.eventData.OnMiss.AddListener(RemoveMorale);
            gd.roundData.OnSpeedBonus.AddListener(AddMoraleFromBonus);
            gd.eventData.OnGameOver.AddListener(DisplayMoralePoints);
        }

        private void ResetMorale() {
            moraleData.ResetMorale();
            moraleData.moralePointsEarned = 0;
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
           
            // Update the morale to a maximum of 100
            moraleData.morale = Mathf.Min(moraleData.maxMorale, moraleData.morale + morale);
            // Award the player morale points
            UpdateMoralePoints(morale);

            // If the player has not yet reached the maximum morale, update the meter
            if (moraleData.morale != moraleData.maxMorale && !moraleData.moraleBoostActive) {
                moraleData.UpdateMoraleMeter(moraleData.GetMorale());
                return;
            }
            // If the morale boost is already active, return
            if (moraleData.moraleBoostActive) return;
            
            // Trigger the morale boost event
            moraleData.moraleBoostActive = true;
            moraleData.OnMoraleBoost?.Invoke();
            moraleBoostText.gameObject.SetActive(true);
            moraleFXVolume.AnimateVolume(1);

            // Start a timer to reset the morale
            StartCoroutine(nameof(MoraleBoostTimer));
        }

        private IEnumerator MoraleBoostTimer() {
            float time = moraleData.moraleBoostDuration;
            
            while (time > 0) {
                // Only run the timer if the round is active
                if (gd.roundData.roundActive) {
                    // Calculate the time remaining as a normalized value
                    float timeRemaining = time / moraleData.moraleBoostDuration;
                    moraleData.TickMoraleBoost(timeRemaining);
                    time -= Time.deltaTime;
                    yield return null;
                }
                
                yield return null;
            }
            
            EndMoraleBoost();
        } 

        private void UpdateMoralePoints(int moraleAdded) {
            // Update the morale points earned
            float mpEarned = moraleAdded * moralePointsMultiplier;
            moraleData.moralePointsEarned += mpEarned;
            moraleData.UpdateMoralePoints(mpEarned);
        }

        private void RemoveMorale() {
            if (moraleData.moraleBoostActive) return;
            // Remove morale points to a minimum of zero
            moraleData.morale = Mathf.Max(0, moraleData.morale - moraleReduction);
            // Trigger the update morale event
            moraleData.UpdateMoraleMeter(moraleData.GetMorale());
        }

        private void EndMoraleBoost() {
            CancelInvoke();
            // Reset the morale points to zero
            moraleData.ResetMorale();
            moraleData.OnMoraleBoostEnd?.Invoke();
            // Trigger the update morale event
            moraleData.UpdateMoraleMeter(moraleData.GetMorale());
            
            moraleBoostText.Disable();
            moraleFXVolume.AnimateVolume(0);
        }

        private void DisplayMoralePoints() {
            // Display the morale points earned
            moraleData.DisplayMoralePoints();
            moraleBoostText.Disable();
        }

        private void SpendMoralPoints(SO_Item item) {
            // Spend morale points
            moraleData.SpendMoralePoints(item.price);
            DataWrangler.GetSaverLoader().SaveGame();
        }
    }
}