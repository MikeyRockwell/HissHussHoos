using System;
using Data.Customization;
using Data.Tutorial;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Managers.Tutorial {
    public class TutorialManager : MonoBehaviour {
        
        // Tutorial manager calls into action tutorials at different game points
        // This will send a message to the Dialogue Manager to start dialogue
        // And control Manager to control inputs

        public SO_Dialogue firstGameIntro;
        public SO_Dialogue firstRoundIntro;
        public SO_Dialogue timeAttackIntro;
        public SO_Dialogue customizationIntro;
        public SO_Dialogue precisionIntro;
        public SO_Dialogue moraleIntro;
        public bool overrideTutorial;

        [SerializeField] private MMF_Player timeScaleFX;
        [SerializeField] private CanvasGroup shopOverlay;
        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnGameReady.AddListener(FirstGameIntro);
            gd.roundData.OnRoundBegin.AddListener(delegate { FirstRoundIntro(); });
            gd.roundData.OnTimeAttackRoundBegin.AddListener(TimeAttackIntro);
            gd.customEvents.OnMenuOpened.AddListener(CustomizationIntro);
            gd.eventData.OnDialogueEnd.AddListener(EndTutorial);
            gd.roundData.OnPrecisionRoundBegin.AddListener(PrecisionIntro);
            gd.roundData.OnRoundBegin.AddListener(delegate { MoraleIntro(); });
        }

        private void EndTutorial(SO_Dialogue arg0) {
            timeScaleFX.StopFeedbacks();
            shopOverlay.interactable = true;
        }

        private void FirstGameIntro() {
            if (!overrideTutorial)
                if (PlayerPrefs.GetInt("FirstTutorial", 1) != 1)
                    return;
            PlayerPrefs.SetInt("FirstTutorial", 0);
            gd.eventData.StartDialogue(firstGameIntro);
        }

        private void FirstRoundIntro() {
            if (gd.roundData.currentRound != 1) return;
            if (!overrideTutorial)
                if (PlayerPrefs.GetInt("FirstRound", 1) != 1)
                    return;
            PlayerPrefs.SetInt("FirstRound", 0);
            timeScaleFX.PlayFeedbacks();
            gd.eventData.StartDialogue(firstRoundIntro);
        }

        private void TimeAttackIntro() {
            if (!overrideTutorial)
                if (PlayerPrefs.GetInt("TimeAttack", 1) != 1)
                    return;
            PlayerPrefs.SetInt("TimeAttack", 0);
            timeScaleFX.PlayFeedbacks();
            gd.eventData.StartDialogue(timeAttackIntro);
        }

        private void CustomizationIntro(SO_Category arg0) {
            if (!overrideTutorial)
                if (PlayerPrefs.GetInt("Customization", 1) != 1)
                    return;
            PlayerPrefs.SetInt("Customization", 0);
            shopOverlay.interactable = false;
            // timeScaleFX.PlayFeedbacks();
            gd.eventData.StartDialogue(customizationIntro);
        }
        
        private void PrecisionIntro() {
            if (!overrideTutorial)
                if (PlayerPrefs.GetInt("Precision", 1) != 1)
                    return;
            PlayerPrefs.SetInt("Precision", 0);
            timeScaleFX.PlayFeedbacks();
            gd.eventData.StartDialogue(precisionIntro);
        }
        
        private void MoraleIntro() {
            if (gd.roundData.currentRound != 2) return;
            if (!overrideTutorial)
                if (PlayerPrefs.GetInt("MoraleBoost", 1) != 1)
                    return;
            PlayerPrefs.SetInt("MoraleBoost", 0);
            timeScaleFX.PlayFeedbacks();
            gd.eventData.StartDialogue(moraleIntro);
        }
    }
}