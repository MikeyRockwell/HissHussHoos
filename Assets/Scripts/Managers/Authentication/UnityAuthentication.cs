using Utils;
using System;
using UnityEngine;
using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.Authentication;

namespace Managers {
    
    // This class is used to initialize the Unity Services SDK
    public class UnityAuthentication : MonoBehaviour {
        private async void Awake() {
            try {
                await UnityServices.InitializeAsync();
            }
            catch(Exception e) {
                Log.Error(e.ToString());
            }
            await SignInAnonymously();
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
        }
    }
    
    
}