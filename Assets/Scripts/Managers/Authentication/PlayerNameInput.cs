using Data;
using TMPro;
using Utils;
using UnityEngine;
using UI.Statistics;
using Unity.Services.Authentication;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers {
    public class PlayerNameInput : UIWindow {
        // This class is used to get the player's name from an input field
        [SerializeField] private AuthenticationData authData;
        [SerializeField] private SceneManager sceneManager;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private ProfanityFilter filter;
        [SerializeField] private Button submitButton;

        public string inputName;

        protected override void Awake() {
            base.Awake();
            authData.OnPlayerNameRequired.AddListener(OpenWindow);

            submitButton.interactable = false;

            inputField.onSubmit.AddListener(EnableSubmitButton);
            submitButton.onClick.AddListener(() => SubmitName(inputName));
        }

        private void EnableSubmitButton(string input) {
            inputName = input;
            submitButton.interactable = true;
        }

        private async void SubmitName(string input) {
            switch (input.Length) {
                case < 3:
                    Log.Message("Player name must be at least 3 characters long");
                    infoText.text = "Name must be at least 3 characters long";
                    return;
                case > 20:
                    Log.Message("Player name must be less than 20 characters long");
                    infoText.text = "Name must be less than 20 characters long";
                    return;
            }

            // if (!filter.CheckForProfanity(input))
            // {
            //     Log.Message("Player name contains a naughty word!");
            //     infoText.text = "Name contains a naughty word!";
            //     return;
            // }

            await AuthenticationService.Instance.UpdatePlayerNameAsync(input);
            PlayerPrefs.SetString("PlayerName", input);
            Log.Message($"Player name set to {input}");

            sceneManager.SceneTransition(02);
        }
    }
}