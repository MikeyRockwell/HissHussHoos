using UI;
using Utils;
using UnityEngine;
using Unity.Services.Authentication;

namespace Managers {
    public class DebugControls : MonoBehaviour {

        [SerializeField] private UnityAuthentication authentication;
        [SerializeField] private LeaderBoard Leaderboard;
        [SerializeField] private MoraleManager moraleManager;
        
        private DataWrangler.GameData gd;
        
        private void Awake() {
            gd = DataWrangler.GetGameData();
        }
#if UNITY_EDITOR
        private void Update() {
            
            if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftShift)) {
                Log.Message("Resetting All Items", gd.uIData.HotPink);
                gd.itemData.ResetItems();
            }

            if (Input.GetKey(KeyCode.UpArrow)) {
                moraleManager.AddMorale(10);
            }

            if (Input.GetKeyDown(KeyCode.L)) {
                Leaderboard.GetPlayerRange();
            }

            if (Input.GetKeyDown(KeyCode.X)) {
                // Clear the Authentication session token
                Log.Message("Logging out from Unity Authentication", gd.uIData.HotPink);
                AuthenticationService.Instance.SignOut();
                AuthenticationService.Instance.ClearSessionToken();
                SignIn();
            }
        }

        private async void SignIn() {
            await authentication.SignInAnonymously();
        }
#endif
    }
}