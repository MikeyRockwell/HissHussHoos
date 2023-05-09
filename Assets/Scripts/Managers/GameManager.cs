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

            if (gd.gameState.firstLaunch) {
                gd.gameState.firstLaunch = false;
            }
            
            // Load data
            LoadSaveData loadSaveData = DataWrangler.GetSaveData();
            loadSaveData.LoadGame();
            // Init game
            gd.eventData.InitializeGame();
        }
    }
}
