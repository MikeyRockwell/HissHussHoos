using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using Data;

namespace Utils {
    public static class Serialization {
        public static bool CheckSaveExists(string saveName) {
            string path = Path.Combine(Application.persistentDataPath, saveName);
            return File.Exists(path);
        }

        public static void Save(string saveName, ItemData.ItemSaveData data) {
            string json = JsonSerializer.Save(data);
            BinaryFormatter formatter = GetBinaryFormatter();
            string path = Path.Combine(Application.persistentDataPath, saveName);
            FileStream file = new(path, FileMode.Create);

            formatter.Serialize(file, json);
            file.Close();

            Log.Message("Save Successful!");
        }

        public static ItemData.ItemSaveData Load(string saveName) {
            string path = Path.Combine(Application.persistentDataPath, saveName);

            if (!File.Exists(path)) Log.Error("Save file does not exist!");

            BinaryFormatter formatter = GetBinaryFormatter();
            FileStream file = new(path, FileMode.Open);

            try {
                string loaded = formatter.Deserialize(file) as string;
                ItemData.ItemSaveData data = JsonLoad(loaded);
                file.Close();
                return data;
            }
            catch {
                Debug.LogErrorFormat("Failed to load file at {0}", path);
                file.Close();
                return new ItemData.ItemSaveData();
            }
        }

        private static ItemData.ItemSaveData JsonLoad(string loaded) {
            return JsonUtility.FromJson<ItemData.ItemSaveData>(loaded);
        }

        public static void NewGame(string saveName) {
            string path = Application.persistentDataPath + "/saves/" + saveName;
            if (!File.Exists(path)) return;
            File.Delete(path);
            Debug.Log("DELETED File: " + saveName);
        }

        private static BinaryFormatter GetBinaryFormatter() {
            BinaryFormatter formatter = new();
            return formatter;
        }
    }
}