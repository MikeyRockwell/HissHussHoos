using Utils;
using System;
using UnityEngine;
// using GooglePlayGames;
using LootLocker.Requests;
using System.Threading.Tasks;
// using GooglePlayGames.BasicApi;

namespace Managers {
    public class Authentication : MonoBehaviour {
        
        private string username;
        private string idToken;
        
        
        private void Start() {
            SelectLoginSystem();
        }
        
        // private void OnApplicationFocus(bool hasFocus) {
        //     SelectLoginSystem();
        // }

        async void SelectLoginSystem() {
            
            #if UNITY_EDITOR
            
                Log.Message("Attempting Guest Login");
                username = "Unity Editor";
                await LoginLootLockerGuest();
            
            #endif

            #if UNITY_ANDROID
            
                // await AuthenticateGoogle();
                await LoginLootLockerGuest();   
            
            #endif
            
            #if UNITY_IOS
                
                AuthenticateIOS();
            
            #endif
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

#if UNITY_ANDROID
        
        /* private Task AuthenticateGoogle() {
            
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
        } */

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
        
#endif

#if UNITY_IOS
        
        private void AuthenticateIOS() {
            // DO IOS login thing here
        }
        
#endif
        
    }
}