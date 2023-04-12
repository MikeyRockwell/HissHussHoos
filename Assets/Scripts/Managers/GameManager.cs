using Data;
using UnityEngine;

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
            // Load data
            SO_SaveData saveData = DataWrangler.GetSaveData();
            saveData.InitializeLists();
            saveData.LoadGame();
            // Init game
            gd.eventData.InitializeGame();
        }


#if UNITY_ANDROID
        // private void OnApplicationPause(bool pauseStatus) {
        //     DataWrangler.GetSaveData().SaveGame();
        // }

        private void OnApplicationQuit() {
            DataWrangler.GetSaveData().SaveGame();
        }
#endif
        
#if UNITY_EDITOR        
        private void OnDisable() {
            DataWrangler.GetSaveData().SaveGame();
        }
#endif
    }
}
