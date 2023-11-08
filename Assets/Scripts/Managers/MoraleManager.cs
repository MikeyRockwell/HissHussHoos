using FX;
using Data;
using UnityEngine;
using Data.Customization;

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
        private float moraleBoostClock;

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
            gd.customEvents.OnItemUnlocked.AddListener(SpendMoralePoints);
            gd.customEvents.OnColorUnlocked.AddListener(SpendMoralePoints);
            gd.eventData.OnHit.AddListener(AddMoraleFromPunch);
            gd.eventData.OnHitTimeAttack.AddListener(AddMoraleFromPunch);
            gd.eventData.OnMiss.AddListener(RemoveMorale);
            gd.roundData.OnSpeedBonus.AddListener(AddMoraleFromBonus);
            gd.eventData.OnGameOver.AddListener(DisplayMoralePoints);
            gd.eventData.OnGameOver.AddListener(GameOver);
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
            moraleBoostClock = moraleData.moraleBoostDuration;
        }

        private void FixedUpdate() {
            // If the morale boost is not active, return
            if (!moraleData.moraleBoostActive) return;
            // If the round is not active, return
            if (!gd.roundData.roundActive) return;
            // If the morale boost timer is up, end the morale boost
            if (moraleBoostClock <= 0) {
                EndMoraleBoost();
            }
            // Update the morale boost timer
            if (!(moraleBoostClock > 0)) return;
            
            moraleData.TickMoraleBoost(moraleBoostClock / moraleData.moraleBoostDuration);
            moraleBoostClock -= Time.deltaTime;

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

        private void GameOver() {
            CancelInvoke();
            moraleBoostText.Disable();
            moraleFXVolume.AnimateVolume(0);
        }

        private void DisplayMoralePoints() {
            // Display the morale points earned
            moraleData.DisplayMoralePoints();
            moraleBoostText.Disable();
        }

        private void SpendMoralePoints(SO_Item item) {
            // Spend morale points
            moraleData.SpendMoralePoints(item.price);
            DataWrangler.GetSaverLoader().SaveGame();
        }
        
        private void SpendMoralePoints(SO_Color color) {
            // Spend morale points
            moraleData.SpendMoralePoints(color.price);
            DataWrangler.GetSaverLoader().SaveGame();
        }
    }
}