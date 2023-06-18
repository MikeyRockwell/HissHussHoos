using Data;
using Utils;
using UnityEngine;
using UI.Statistics;
using Unity.Services.Authentication;

namespace Managers
{
    public class PlayerNameInput : UIWindow
    {
        // This class is used to get the player's name from an input field
        [SerializeField] private AuthenticationData authData;
        [SerializeField] private TMPro.TMP_InputField inputField;

        protected override void Awake()
        {
            base.Awake();
            authData.OnPlayerNameRequired.AddListener(CheckWindowStatus);
            inputField.onSubmit.AddListener(SubmitName);
        }

        private static void SubmitName(string input)
        {
            AuthenticationService.Instance.UpdatePlayerNameAsync(input);
            PlayerPrefs.SetString("PlayerName", input);
            Log.Message($"Player name set to {input}");
        }
    }
}