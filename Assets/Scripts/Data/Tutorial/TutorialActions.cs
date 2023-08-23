using System;
using Managers;
using TMPro;
using UnityEngine;

namespace Data.Tutorial {
    public class TutorialActions : MonoBehaviour {
        [SerializeField] private TutorialEvent HISSTutorial;
        [SerializeField] private TutorialEvent HUSSTutorial;
        [SerializeField] private TutorialEvent HOOSTutorial;

        [SerializeField] private TextMeshProUGUI actionText;

        private int HISSPunches;
        private int HUSSPunches;
        private int HOOSPunches;

        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnPunchWarmup.AddListener(PunchListener);

            HISSTutorial.OnEventTriggered.AddListener(InitHISSActions);
            HUSSTutorial.OnEventTriggered.AddListener(InitHUSSActions);
            HOOSTutorial.OnEventTriggered.AddListener(InitHOOSActions);
        }

        private void InitHISSActions() {
            actionText.text = "HISS " + "<color=yellow>" + HISSPunches + "</color>/" + HISSTutorial.eventRequirements;
            HISSTutorial.eventActive = true;
        }

        private void InitHUSSActions() {
            actionText.text = "HUSS " + "<color=yellow>" + HUSSPunches + "</color>/" + HUSSTutorial.eventRequirements;
            HUSSTutorial.eventActive = true;
        }

        private void InitHOOSActions() {
            actionText.text = "HOOS " + "<color=yellow>" + HOOSPunches + "</color>/" + HOOSTutorial.eventRequirements;
            HOOSTutorial.eventActive = true;
        }

        private void PunchListener(TargetData.Target target) {
            switch (target) {
                case TargetData.Target.HISS:
                    if (HISSTutorial.eventActive) {
                        HISSPunches++;
                        actionText.text = "HISS " + "<color=yellow>" + HISSPunches + "</color>/" +
                                          HISSTutorial.eventRequirements;
                        if (HISSPunches >= HISSTutorial.eventRequirements) {
                            HISSTutorial.CompleteAction();
                            HISSTutorial.eventActive = false;
                        }
                    }

                    break;
                case TargetData.Target.HUSS:
                    if (HUSSTutorial.eventActive) {
                        HUSSPunches++;
                        actionText.text = "HUSS " + "<color=yellow>" + HUSSPunches + "</color>/" +
                                          HUSSTutorial.eventRequirements;
                        if (HUSSPunches >= HUSSTutorial.eventRequirements) {
                            HUSSTutorial.CompleteAction();
                            HUSSTutorial.eventActive = false;
                        }
                    }

                    break;
                case TargetData.Target.HOOS:
                    if (HOOSTutorial.eventActive) {
                        HOOSPunches++;
                        actionText.text = "HOOS " + "<color=yellow>" + HOOSPunches + "</color>/" +
                                          HOOSTutorial.eventRequirements;
                        if (HOOSPunches >= HOOSTutorial.eventRequirements) {
                            HOOSTutorial.CompleteAction();
                            HOOSTutorial.eventActive = false;
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(target), target, null);
            }
        }
    }
}