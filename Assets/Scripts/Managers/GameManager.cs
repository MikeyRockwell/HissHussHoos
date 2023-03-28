using Data;
using Utils;
using UnityEngine;

namespace Managers {
    public class GameManager : MonoBehaviour {

        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnGameOver.AddListener(GameOver);
        }

        private void GameOver() {
            Log.Message("GAME OVER");
            // gd.eventData.NewGame();
        }

        private void Start() {
            // Start new game - this might be called from the menu screen?
            SO_SaveData saveData = DataWrangler.GetSaveData();
            saveData.InitializeLists();
            saveData.LoadGame();
            gd.eventData.NewGame();
        }

        private void OnDisable() {
            DataWrangler.GetSaveData().SaveGame();
        }
    }
}