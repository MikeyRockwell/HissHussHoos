using Data;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private DataWrangler.GameData gd;

        private void Awake()
        {
            gd = DataWrangler.GetGameData();
            // Target frame rate 
            // TODO move to a video controller 
            Application.targetFrameRate = 60;

            // This is a hack to reset the game to new
            NUKE();
        }

        // TODO DELETE THIS
        private void NUKE()
        {
            if (PlayerPrefs.GetInt("Nuke4", 1) == 1)
            {
                PlayerPrefs.SetInt("FirstLaunch", 1);
                PlayerPrefs.SetInt("Nuke4", 0);
            }
        }

        private void Start()
        {
            DataSaverLoader dataSaverLoader = DataWrangler.GetSaverLoader();

            if (gd.gameState.firstLaunch)
                // Check if the game has been launched!
                if (PlayerPrefs.GetInt("FirstLaunch", 1) == 1)
                {
                    // Set first launch to false
                    gd.gameState.firstLaunch = false;
                    PlayerPrefs.SetInt("FirstLaunch", 0);

                    // Reset game to new
                    ResetGameToDefault(dataSaverLoader);
                    return;
                }

            // Load data
            dataSaverLoader.LoadGame();
            // Init game
            gd.eventData.InitializeGame();
        }

        private void ResetGameToDefault(DataSaverLoader dataSaverLoader)
        {
            gd.eventData.FirstLaunch();
            dataSaverLoader.ResetData();
            gd.eventData.InitializeGame();
        }
    }
}