using Data;
using System;
using Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TARGET = Data.TargetData.Target;

namespace UI
{
    public class PunchButtons : MonoBehaviour
    {
        [SerializeField] private Button hissButton;
        [SerializeField] private Button hussButton;
        [SerializeField] private Button hoosButton;

        private bool punching;
        private DataWrangler.GameData gd;

        private void OnEnable()
        {
            gd = DataWrangler.GetGameData();

            // On click of each button invokes OnPunch event with TARGET enum arg
            hissButton.onClick.AddListener(() => Punch(TARGET.HISS));
            hussButton.onClick.AddListener(() => Punch(TARGET.HUSS));
            hoosButton.onClick.AddListener(() => Punch(TARGET.HOOS));
        }

        private void Punch(TARGET target)
        {
            if (punching) return;

            switch (gd.roundData.roundType)
            {
                case RoundData.RoundType.warmup:
                    gd.eventData.PunchWarmup(target);
                    break;
                case RoundData.RoundType.normal:
                    gd.eventData.PunchNormal(target);
                    break;
                case RoundData.RoundType.timeAttack:
                    gd.eventData.PunchTimeAttack(target);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            punching = true;
            CoolDown();
        }

        private void CoolDown()
        {
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(gd.playerData.punchSpeed).OnComplete(() => punching = false);
        }
    }
}