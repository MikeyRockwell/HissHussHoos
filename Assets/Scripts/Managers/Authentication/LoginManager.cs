using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Managers {
    public class LoginManager : MonoBehaviour {
        private DataWrangler.GameData gd;

        [SerializeField] private UnityAuthentication authentication;
        [SerializeField] private TextMeshProUGUI loggingInText;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            // Target frame rate 
            // TODO move to a video controller 
            Application.targetFrameRate = 60;

            // This is a hack to reset the game to new
            NUKE();
        }

        private async Task NUKE() {
            int nukeCode = gd.gameState.NUKECODE;

            if (PlayerPrefs.GetInt("Nuke" + nukeCode, 1) == 1) {
                PlayerPrefs.SetInt("FirstLaunch", 1);
                PlayerPrefs.SetInt("FirstTutorial", 1);
                PlayerPrefs.SetInt("FirstRound", 1);
                PlayerPrefs.SetInt("TimeAttack", 1);
                PlayerPrefs.SetInt("Customization", 1);
                PlayerPrefs.SetInt("Nuke" + nukeCode, 0);

                gd.gameState.firstLaunch = true;

                await authentication.LogOutOfUnity();
            }
        }

        private async void Start() {
            await authentication.StartUnityServices();
            loggingInText.DOScale(0, 0.25f).SetEase(Ease.InBounce);
        }
    }
}