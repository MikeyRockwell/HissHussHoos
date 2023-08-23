using System.Collections;
using UnityEngine;
using Data.Tutorial;
using System.Collections.Generic;
using Audio;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using Sentence = Data.Tutorial.SO_Dialogue.Sentence;

namespace Managers.Tutorial {
    public class DialogueManager : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI dialogueHeader;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private TextMeshProUGUI actionText;
        [SerializeField] private RectTransform dialogueBox;
        [SerializeField] private Button nextButton;
        [SerializeField] private SoundFXPlayer sfxPlayer;

        private SO_Dialogue currentDialogue;
        private Queue<Sentence> sentences = new();
        private Sentence currentSentence;
        private int index;

        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnDialogueStart.AddListener(InitDialogue);
            nextButton.onClick.AddListener(EndSentence);
        }

        private void InitDialogue(SO_Dialogue dialogue) {
            currentDialogue = dialogue;
            // handle input?
            foreach (Sentence sentence in dialogue.sentences) sentences.Enqueue(sentence);
            index = 0;

            // Set the header
            dialogueHeader.text = dialogue.dialogueName;
            dialogueText.text = "";

            // Show the dialogue box
            bool left = currentDialogue.position == SO_Dialogue.Position.Left;
            float xPivot = left ? 0 : 1;
            // Anchor the dialogue box to the left or right
            dialogueBox.anchorMax = left ? new Vector2(0, 0.5f) : new Vector2(1, 0.5f);
            dialogueBox.anchorMin = left ? new Vector2(0, 0.5f) : new Vector2(1, 0.5f);

            dialogueBox.pivot = new Vector2(xPivot, 0.5f);
            dialogueBox.gameObject.SetActive(true);
            dialogueBox.DOScaleX(1, 0.2f).SetUpdate(true).OnComplete(DisplayNextSentence);
        }

        private void DisplayNextSentence() {
            if (sentences.Count == 0) {
                EndDialogue();
                return;
            }

            currentSentence = sentences.Dequeue();

            bool hasAction = currentSentence.hasAction;

            if (hasAction) {
                currentSentence.tutorialEvent.OnActionCompleted.RemoveListener(DisplayNextSentence);
                currentSentence.tutorialEvent.OnActionCompleted.AddListener(DisplayNextSentence);
                currentSentence.tutorialEvent.TriggerEvent();
            }

            nextButton.gameObject.SetActive(!hasAction);
            actionText.gameObject.SetActive(hasAction);

            StopAllCoroutines();
            dialogueText.maxVisibleCharacters = 0;
            string formattedText = Utils.TextFormatter.FormatDialogue(currentSentence.text);
            dialogueText.text = formattedText;
            StartCoroutine(nameof(WriteSentence));

            index++;
        }

        private IEnumerator WriteSentence() {
            foreach (char unused in currentSentence.text) {
                dialogueText.maxVisibleCharacters++;

                // play random audio boop
                sfxPlayer.PlayRandomAudio();
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }

        private void EndSentence() {
            if (currentSentence.text.ToCharArray().Length > dialogueText.maxVisibleCharacters) {
                StopCoroutine(nameof(WriteSentence));
                dialogueText.maxVisibleCharacters = dialogueText.text.Length;
                return;
            }

            DisplayNextSentence();
        }

        public void EndDialogue() {
            sentences.Clear();

            // hide the dialogue box
            dialogueBox.DOScaleX(0, 0.2f).OnComplete(() => dialogueBox.gameObject.SetActive(false));
            // handle input?
            // send event to tutorial manager
            gd.eventData.EndDialogue(currentDialogue);
        }
    }
}