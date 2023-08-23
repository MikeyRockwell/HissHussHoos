using Data;
using Utils;
using System;
using UnityEngine;
using Unity.Services.Core;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;

namespace Managers {
    // This class is used to initialize the Unity Services SDK
    public class UnityAuthentication : MonoBehaviour {
        [SerializeField] private AuthenticationData authData;
        [SerializeField] private SceneManager sceneManager;

        public async Task StartUnityServices() {
            await InitUnityServices();
            authData.OnPlayerNameSubmitted.AddListener(SubmitPlayerName);
        }
        
        private async Task InitUnityServices() {
            try {
                await UnityServices.InitializeAsync();
            }
            catch (Exception e) {
                Log.Error(e.ToString());
            }

            await SignInAnonymously();
        }

        public async Task LogOutOfUnity() {
            await UnityServices.InitializeAsync();
            AuthenticationService.Instance.SignOut();
            AuthenticationService.Instance.ClearSessionToken();
        }

        public async Task SignInAnonymously() {
            try {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Log.Message("Successfully signed in anonymously", Color.green);
                Log.Message($"PlayerID: {AuthenticationService.Instance.PlayerId}", Color.yellow);
            }
            catch (AuthenticationException ex) {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error messages
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex) {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error messages
                Debug.LogException(ex);
            }

            Log.Message("PLAYER NAME: " + AuthenticationService.Instance.PlayerName, Color.magenta);
            CheckPlayerName();
        }

        private void CheckPlayerName() {
            // Check to see if the player signed in has a name
            // If not, prompt the player to enter a name
            if (AuthenticationService.Instance.PlayerName == null) authData.PlayerNameRequired();
            else sceneManager.SceneTransition(2);
        }

        private async void SubmitPlayerName(string playerName) {
            await SubmitPlayerNameAsync(playerName);
            sceneManager.SceneTransition(2);
        }

        private async Task SubmitPlayerNameAsync(string playerName) {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
            Log.Message($"Player name set to {playerName}");
        }
    }
}