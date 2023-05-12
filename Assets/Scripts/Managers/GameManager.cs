using Data;
using UnityEngine;
using Utils;

namespace Managers {
    public class GameManager : MonoBehaviour {

        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            // Target frame rate 
            // TODO move to a video controller 
            Application.targetFrameRate = 60;
        }

        private void Start() {
            
            DataSaverLoader dataSaverLoader = DataWrangler.GetSaverLoader();

            if (gd.gameState.firstLaunch) {
                gd.gameState.firstLaunch = false;
                
                // Reset game to new
                gd.eventData.FirstLaunch();
                dataSaverLoader.ResetData();
                gd.eventData.InitializeGame();
                return;
            }
            
            // Load data
            dataSaverLoader.LoadGame();
            // Init game
            gd.eventData.InitializeGame();
        }
    }
}
