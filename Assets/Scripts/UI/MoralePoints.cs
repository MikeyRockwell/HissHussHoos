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
        private int moralePointsAtRoundStart;

        private void Awake() {
            // Subscribe to events
            gd = DataWrangler.GetGameData();
            gd.uIData.OnMoralePointsEarned.AddListener(UpdateMoralePoints);
            gd.roundData.OnGameBegin.AddListener(StoreMoralePoints);
            // Hide the morale points earned
            moralePointsEarnedText.transform.localScale = Vector3.zero;
            moralePointsEarnedText.text = 0.ToString();
            moralePointsText.text = gd.playerData.moralePoints.ToString();
        }
        
        private void StoreMoralePoints(int arg0) {
            // Store the morale points at the start of the round
            moralePointsAtRoundStart = gd.playerData.moralePoints;
        }


        private void UpdateMoralePoints(int moralePoints) {
            // Update the morale points earned
            moralePointsEarnedText.transform.position = Vector3.zero;
            moralePointsEarnedText.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce);
            // Count up the morale points earned from zero to moralePoints
            DOTween.To(
                () => 0, x => moralePointsEarnedText.text = x.ToString(), moralePoints, 3f
            ).OnComplete(AnimateToTotal);

        }

        private void AnimateToTotal() {
            // Animate the morale points earned to the total points position
            // In an arc
            moralePointsEarnedText.transform.DOJump(
                moralePointsText.transform.position, jumpPower, 1, 1
            );
            moralePointsEarnedText.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InBounce).OnComplete(UpdateTotal);
        }

        private void UpdateTotal() {
            // Update the morale points total with a counting animation
            DOTween.To(
                () => moralePointsAtRoundStart, x => moralePointsText.text = x.ToString(), 
                gd.playerData.moralePoints, 0.5f
            );
            
        }
    }
}