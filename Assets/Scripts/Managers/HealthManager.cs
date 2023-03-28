using UnityEngine;

namespace Managers {
    public class HealthManager : MonoBehaviour {

        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnMiss.AddListener(RemoveHealth);
            gd.eventData.OnNewGame.AddListener(NewGame);
        }

        private void NewGame() {
            gd.playerData.ResetHealth();
        }

        private void RemoveHealth() {
            
            gd.playerData.ChangeHealth(-1);
            if (gd.playerData.health == 0) {
                gd.eventData.GameOver();
            }
        }
    }
}