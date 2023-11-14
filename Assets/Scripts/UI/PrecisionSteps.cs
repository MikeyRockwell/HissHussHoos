using TMPro;
using Managers;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TARGET = Data.TargetData.Target;

namespace UI {
    public class PrecisionSteps : MonoBehaviour {
        
        [SerializeField] private TextMeshProUGUI accuracyText;
        
        private DataWrangler.GameData gd;
        private Image[] steps;
        private int targetsHit;
        private int punchesThrown;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.roundData.OnPrecisionRoundBegin.AddListener(AnimateStepsOn);
            gd.roundData.OnPrecisionRoundComplete.AddListener(AnimateStepsOff);
            gd.eventData.OnMissPrecision.AddListener(ColorStepMiss);
            gd.eventData.OnHitPrecision.AddListener(ColorStepHit);
            gd.eventData.OnPunchPrecision.AddListener(CountPunchDelay);
            steps = GetComponentsInChildren<Image>();
            gameObject.SetActive(false);
        }

        private void AnimateStepsOn() {
            gameObject.SetActive(true);

            Sequence sequence = DOTween.Sequence();

            // Animate the steps
            for (int i = 0; i < transform.childCount; i++) {
                steps[i].gameObject.SetActive(true);
                steps[i].color = gd.uIData.DisabledComboText;
                sequence.Append(
                    transform.GetChild(i).DOScale(1, 0.02f)
                        .From(0)
                        .SetUpdate(true)
                        .SetEase(Ease.OutBounce)).SetUpdate(true);
            }
            
            punchesThrown = 0;
            targetsHit = 0;
            InitAccuracy();
        }

        private void InitAccuracy() {
            accuracyText.gameObject.SetActive(true);
            accuracyText.transform.DOScale(1, gd.uIData.MenuAnimSpeed);
            accuracyText.text = "ACCURACY 100%";
            gd.targetData.precisionAccuracy = 1;
        }

        private void ColorStepHit(int index) {
            
            CancelInvoke();
            
            steps[index].DOColor(gd.uIData.LaserGreen, 0.5f);
            targetsHit++;
            punchesThrown++;
            CalcAccuracy();
        }
        
        private void ColorStepMiss(int index) {
            steps[index].DOColor(Color.red, 0.5f);
            punchesThrown++;
            CalcAccuracy();
        }
        
        private void CountPunchDelay(TARGET target) {
            // Only count the punch if there isn't a hit within a certain time
            Invoke(nameof(IncrementPunches), gd.playerData.punchSpeed + 0.1f);
        }

        private void IncrementPunches() {
            punchesThrown++;
            CalcAccuracy();
        }
        
        private void CalcAccuracy() {
            float accuracy = (float)targetsHit / punchesThrown;
            gd.targetData.precisionAccuracy = accuracy;
            accuracyText.text = $"ACCURACY {accuracy:P0}";
        }

        private void AnimateStepsOff(int arg0) {
            Sequence sequence = DOTween.Sequence();

            // Animate the steps
            for (int i = 0; i < transform.childCount; i++)
                sequence.Append(
                        transform.GetChild(i).DOScale(0, 0.02f).From(1).SetEase(Ease.InBounce))
                    .OnComplete(() => gameObject.SetActive(false));
            // Animate the accuracy text
            accuracyText.transform.DOScale(
                0, 
                gd.uIData.MenuAnimSpeed).OnComplete(()=> accuracyText.gameObject.SetActive(false));
        }
    }
}