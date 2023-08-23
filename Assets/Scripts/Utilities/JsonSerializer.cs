using Data;
using UnityEngine;

namespace Utils {
    public static class JsonSerializer {
        public static string Save(ItemData.ItemSaveData itemSaveData) {
            // Convert gameData to a string
            string json = JsonUtility.ToJson(itemSaveData);
            return json;
        }
    }
}