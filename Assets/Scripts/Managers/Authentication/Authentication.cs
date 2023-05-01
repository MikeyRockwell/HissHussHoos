using Utils;
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
        async void SelectLoginSystem() {
            
        #region UNITY EDITOR -- Guest Login
        #if UNITY_EDITOR
            // If we are in the editor, we will use the guest login system
            Log.Message("Attempting Guest Login as Unity Editor");
            username = "Unity Editor";
            await LoginLootLockerGuest();
        #endif
        #endregion

        #region ANDROID DEVICE -- Google Login
        #if UNITY_ANDROID && !UNITY_EDITOR
            // If we are on an android device, we will use the google login system
            await AuthenticateGoogle();
            await LoginLootLockerGuest();
        #endif
        #endregion

        #region IOS DEVICE -- Apple Login
        #if UNITY_IOS
                    // If we are on an IOS device, we will use the IOS login system
                    AuthenticateIOS();
        #endif
        #endregion
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        private void OnApplicationFocus(bool hasFocus) {
        SelectLoginSystem();
        }
#endif

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