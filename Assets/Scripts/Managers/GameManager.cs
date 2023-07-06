﻿using System.Threading.Tasks;
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