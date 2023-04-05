using System.IO;
using UnityEngine;
using SaveGameData = Data.SO_SaveData.SaveGameData;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;

namespace Utils {
    public static class Serialization {

        public static bool CheckSaveExists(string saveName) {
            
            string path = Path.Combine(Application.persistentDataPath, saveName);

            return File.Exists(path);
        }
        
        public static void Save(string saveName, SaveGameData gameData) {
            
            string json = JsonSerializer.Save(gameData);
            
            BinaryFormatter formatter = GetBinaryFormatter();

            string path = Path.Combine(Application.persistentDataPath, saveName);
            
            FileStream file = new (path, FileMode.Create);
            
            formatter.Serialize(file, json);
            file.Close();
            
            Log.Message("Save Successful!");
        }

        public static SaveGameData Load(string saveName) {

            string path = Path.Combine(Application.persistentDataPath, saveName);

            if(!File.Exists(path)){
                Log.Error("Save file does not exist!");
            }
            
            BinaryFormatter formatter = GetBinaryFormatter();
            FileStream file = new (path, FileMode.Open);
            
            try {
                string loaded = formatter.Deserialize(file) as string;
                SaveGameData gameData = JsonLoad(loaded);
                file.Close();
                return gameData;
            }
            catch {
                Debug.LogErrorFormat("Failed to load file at {0}", path);
                file.Close();
                return new SaveGameData();
            }
        }

        private static SaveGameData JsonLoad(string loaded) {
            return JsonUtility.FromJson<SaveGameData>(loaded);
        }

        public static void NewGame(string saveName){
            string path = Application.persistentDataPath + "/saves/" + saveName;
            if (!File.Exists(path)) return;
            File.Delete(path);
            Debug.Log("DELETED File: " + saveName);
        }

        private static BinaryFormatter GetBinaryFormatter(){
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter;
        }
    }
}