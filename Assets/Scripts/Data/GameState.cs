using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Data
{
    [CreateAssetMenu(fileName = "GameState", menuName = "ScriptableObjects/Data/GameState", order = 0)]
    public class GameState : ScriptableObject
    {
        [Button(ButtonSizes.Gigantic)]
        [GUIColor(0, 1, 0)]
        [DisableIf("firstLaunch")]
        [PropertyOrder(-1)]
        private void NewGame()
        {
            firstLaunch = !firstLaunch;
        }

        public bool firstLaunch;

        [Serializable]
        public struct SaveData
        {
            // This is the save data for the game state
            public bool firstLaunch;
        }
    }
}