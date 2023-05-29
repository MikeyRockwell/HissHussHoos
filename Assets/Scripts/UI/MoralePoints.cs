using Data;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;

namespace UI {
    public class MoralePoints : MonoBehaviour {

        // Class to display the morale points
        // And the morale points earned
        [SerializeField] private TextMeshProUGUI moralePointsText;
        [SerializeField] private TextMeshProUGUI moralePointsEarnedText;
        [SerializeField] private float jumpPower = 2f;

        private DataWrangler.GameData gd;
        private MoraleData md;
        private float moralePointsAtRoundStart;

        private void Awake() {
            // Subscribe to events
            gd = DataWrangler.GetGameData();
            md = gd.playerData.md;
            gd.eventData.OnGameInit.AddListener(InitMoralePoints);
            gd.roundData.OnGameBegin.AddListener(StoreMoralePoints);
            md.OnMoralePointsEarned.AddListener(UpdateMoralePoints);
            md.OnMoralePointsSpent.AddListener(SpendMoralePoints);
            // Hide the morale points earned
            moralePointsEarnedText.transform.localScale = Vector3.zero;
            moralePointsEarnedText.text = 0.ToString();
        }

        private void InitMoralePoints() {
            // Load morale points and set the text
            float moralePoints = gd.playerData.md.LoadMoralePoints();
            moralePointsText.text = moralePoints.ToString($"0");
        }

        private void StoreMoralePoints(int i) {
            // Store the morale points at the start of the round
            moralePointsAtRoundStart = gd.playerData.md.moralePoints;
        }

        private void SpendMoralePoints(float moralePoints, float moralePointsSpent) {
            // Spend morale points
            DOTween.To(
                () => moralePoints, x => moralePointsText.text = x.ToString($"0"), 
                moralePoints - moralePointsSpent, 0.5f
            );
        }

        private void UpdateMoralePoints(float moralePoints) {
            // Update the morale points earned
            moralePointsEarnedText.color = gd.uIData.LaserGreen;
            moralePointsEarnedText.transform.position = Vector3.zero;
            moralePointsEarnedText.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce);
            // Count up the morale points earned from zero to moralePoints in whole numbers
            DOTween.To(
                () => 0, x => moralePointsEarnedText.text = x.ToString(), 
                Mathf.RoundToInt(moralePoints), 1f
            ).OnComplete(AnimateToTotal);
            
        }

        private void AnimateToTotal() {
            // Animate the morale points earned to the total points position
            // In an arc
            moralePointsEarnedText.transform.DOJump(
                moralePointsText.transform.position, jumpPower, 1, 1
            );
            moralePointsEarnedText.transform.DOScale(
                Vector3.zero, 1f).SetEase(Ease.InBounce).OnComplete(UpdateTotal);
        }

        private void UpdateTotal() {
            // Update the morale points total with a counting animation
            DOTween.To(
                () => moralePointsAtRoundStart, x => moralePointsText.text = x.ToString($"0"), 
                (int)gd.playerData.md.moralePoints, 0.5f
            );
        }
    }
}