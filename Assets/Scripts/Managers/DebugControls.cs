using System;
using UI;
using FX;
using Data;
using Utils;
using UnityEngine;
using Managers.Tutorial;
using Unity.Services.Authentication;
using UnityEngine.UI;
using TARGET = Data.TargetData.Target;

namespace Managers {
    public class DebugControls : MonoBehaviour {
        [SerializeField] private UnityAuthentication authentication;
        [SerializeField] private LeaderBoard Leaderboard;
        [SerializeField] private MoraleManager moraleManager;
        [SerializeField] private RoundPopUps roundPops;
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private StartGameButton start;

        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
        }
#if UNITY_EDITOR
        private void Update() {
            if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftShift)) {
                Log.Message("Resetting All Items", gd.uIData.HotPink);
                gd.itemData.ResetItems();
                PlayerPrefs.SetInt("HighScore", 0);
            }

            if (Input.GetKey(KeyCode.UpArrow)) moraleManager.AddMorale(10);

            if (Input.GetKeyDown(KeyCode.L)) Leaderboard.GetPlayerRange();

            if (Input.GetKeyDown(KeyCode.X)) {
                // Clear the Authentication session token
                Log.Message("Logging out from Unity Authentication", gd.uIData.HotPink);
                AuthenticationService.Instance.SignOut();
                AuthenticationService.Instance.ClearSessionToken();
                // SignIn();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1)) roundPops.InitRoundPopup(RoundData.RoundType.timeAttack);
            if (Input.GetKey(KeyCode.KeypadPlus)) gd.playerData.md.moralePoints += 10;
            if (Input.GetKeyUp(KeyCode.KeypadPlus)) gd.playerData.md.SaveMoralePoints();
            if (Input.GetKeyDown(KeyCode.Escape)) dialogueManager.EndDialogue();
            if (Input.GetKeyDown(KeyCode.Space)) {
                TARGET target;
                switch (gd.roundData.roundType) {
                    case RoundData.RoundType.warmup:
                        start.GetComponent<Button>().onClick.Invoke();
                        break;
                    case RoundData.RoundType.normal:
                        target = gd.targetData.currentSet[gd.targetData.step];
                        gd.eventData.PunchNormal(target);
                        break;
                    case RoundData.RoundType.timeAttack:
                        target = gd.targetData.currentTimeAttackTarget;
                        gd.eventData.PunchTimeAttack(target);
                        break;
                    case RoundData.RoundType.precision:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private async void SignIn() {
            await authentication.SignInAnonymously();
        }
#endif
    }
}