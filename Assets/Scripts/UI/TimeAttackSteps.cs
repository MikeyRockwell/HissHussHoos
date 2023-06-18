using Data;
using Managers;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace UI
{
    public class TimeAttackSteps : MonoBehaviour
    {
        private DataWrangler.GameData gd;
        private Image[] steps;

        private void Awake()
        {
            gd = DataWrangler.GetGameData();
            gd.roundData.OnTimeAttackRoundBegin.AddListener(AnimateStepsOn);
            gd.roundData.OnTimeAttackTargetTimedOut.AddListener(delegate { ColorStep(false); });
            gd.roundData.OnTimeAttackRoundComplete.AddListener(AnimateStepsOff);
            gd.eventData.OnPunchTimeAttack.AddListener(CheckTarget);

            steps = GetComponentsInChildren<Image>();
            gameObject.SetActive(false);
        }

        private void AnimateStepsOn()
        {
            gameObject.SetActive(true);

            Sequence sequence = DOTween.Sequence();

            // Animate the steps
            for (int i = 0; i < transform.childCount; i++)
            {
                steps[i].gameObject.SetActive(true);
                steps[i].color = gd.uIData.DisabledComboText;
                sequence.Append(
                    transform.GetChild(i).DOScale(1, 0.02f).From(0).SetEase(Ease.OutBounce));
            }

            sequence.Play();
        }

        private void CheckTarget(TargetData.Target target)
        {
            ColorStep(target == gd.targetData.currentTimeAttackTarget);
        }

        private void ColorStep(bool correct)
        {
            steps[gd.roundData.roundStep - 1].DOColor(correct ? Color.green : Color.red, 0.5f);
        }

        private void AnimateStepsOff(int arg0)
        {
            Sequence sequence = DOTween.Sequence();

            // Animate the steps
            for (int i = 0; i < transform.childCount; i++)
                sequence.Append(
                        transform.GetChild(i).DOScale(0, 0.02f).From(1).SetEase(Ease.InBounce))
                    .OnComplete(() => gameObject.SetActive(false));
        }
    }
}