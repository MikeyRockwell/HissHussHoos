using System;
using Unity.Services.Authentication;
using UnityEngine;
using Utils;

namespace Managers {
    public class PlayerName : MonoBehaviour {
        
        // This class is used to get the player's name from an input field
        [SerializeField] private TMPro.TMP_InputField inputField;

        private void Awake() {
            inputField.onSubmit.AddListener(SubmitName);
        }

        private void SubmitName(string input) {
            string playerName = input;
            AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
            PlayerPrefs.SetString("PlayerName", playerName);
            Log.Message($"Player name set to {playerName}");
        }
    }
}