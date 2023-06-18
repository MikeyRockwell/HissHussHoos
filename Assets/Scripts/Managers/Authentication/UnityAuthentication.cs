using Data;
using Utils;
using System;
using UnityEngine;
using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.Authentication;

namespace Managers
{
    // This class is used to initialize the Unity Services SDK
    public class UnityAuthentication : MonoBehaviour
    {
        [SerializeField] private AuthenticationData authData;
        [SerializeField] private AppleAuthorization appleAuthorization;

        private async void Awake()
        {
            try
            {
                await UnityServices.InitializeAsync();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }

            // Sign in anonymously for testing - possibly solution??
            await SignInAnonymously();
            // Here we will put apple and google sign in
            // appleAuthorization.LoginToApple();
        }

        public async Task SignInAnonymously()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Log.Message("Successfully signed in anonymously", Color.green);
                Log.Message($"PlayerID: {AuthenticationService.Instance.PlayerId}", Color.yellow);
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error messages
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error messages
                Debug.LogException(ex);
            }

            Log.Message("PLAYER NAME: " + AuthenticationService.Instance.PlayerName, Color.magenta);
            CheckPlayerName();
        }

        private void CheckPlayerName()
        {
            // Check to see if the player signed in has a name
            // If not, prompt the player to enter a name
            if (AuthenticationService.Instance.PlayerName == null) authData.PlayerNameRequired();
        }
    }
}