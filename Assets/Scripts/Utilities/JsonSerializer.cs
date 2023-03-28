using Data;
using UnityEngine;

namespace Utils {
    public static class JsonSerializer {
       
        public static string Save(SO_SaveData.SaveGameData saveGameData) {
            // Convert gameData to a string
            string json = JsonUtility.ToJson(saveGameData);
            return json;
        }
    }
}