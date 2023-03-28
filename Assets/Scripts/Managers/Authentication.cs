using System;
using UnityEngine;
using GooglePlayGames;
using LootLocker.Requests;
using System.Threading.Tasks;
using GooglePlayGames.BasicApi;

namespace Managers {
    public class Authentication : MonoBehaviour {
        
        private string username;
        private string idToken;
        
        async void Start() {

            switch (Application.platform) {
                case RuntimePlatform.WindowsEditor:
                    Debug.Log("Attempting Guest Login");
                    await LoginLootLockerGuest();
                    break;
                case RuntimePlatform.IPhonePlayer:
                    AuthenticateIOS();
                    break;
                case RuntimePlatform.Android:
                    await AuthenticateGoogle();
                    await LoginLootLockerGuest();
                    // await LootLockerLoginGoogle();
                    Debug.Log("Attempting Google Login");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Task AuthenticateGoogle() {
            
            var tcs = new TaskCompletionSource<object>();
            PlayGamesPlatform.Instance.Authenticate(delegate(SignInStatus status) { });
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success) => {
                username = PlayGamesPlatform.Instance.localUser.userName;
                idToken = PlayGamesPlatform.Instance.localUser.id;
                tcs.SetResult(null);
            });
            return tcs.Task;
        }

        private Task LootLockerLoginGoogle() {
            
            LootLockerSDKManager.StartGoogleSession(idToken,response => {
                if (!response.success) {
                    Debug.Log("Error starting LootLocker GUEST session");
                    return;
                }

                Debug.Log("Successfully started LootLocker GUEST Session");
            });
            return Task.CompletedTask;
        }
        
        private Task LoginLootLockerGuest() {
            
            LootLockerSDKManager.StartGuestSession(username,response => {
                if (!response.success) {
                    Debug.Log("Error starting LootLocker GUEST session");
                    return;
                }

                Debug.Log("Successfully started LootLocker GUEST Session");
                PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
            });
            return Task.CompletedTask;
        }


        private void AuthenticateIOS() {

        }
        
    }
}