﻿using Utils;
using System.Linq;
using UnityEngine;
using Data.Customization;
using System.Diagnostics;
using System.Collections.Generic;

namespace Data {
    [CreateAssetMenu(fileName = "SaveData", menuName = "ScriptableObjects/Data/SaveData", order = 0)]
    public class SO_SaveData : ScriptableObject {
        
        // This is a scriptable object - only to make life easier when accessing this object
        // It holds lists of every save-able item 
        // Items such as clothing items have save and load functions
        // These are called and made into lists - then serialized
        
        // This data is loaded from the resources folder
        public List<SO_CharacterPart> allParts;
        public List<SO_Item> allItems;

        public void InitializeLists() {
            allItems.Clear();
            foreach (SO_Item item in allParts.SelectMany(part => part.Items)) {
                allItems.Add(item);
            }
            // allItems = Resources.LoadAll<SO_Item>("").ToList();
        }
        
        public void SaveGame() {
            Stopwatch timer = Timer.StartTimer();
            SaveGameData data = GenerateSaveData();
            Serialization.Save("Save.dat", data);
            Log.Message(Timer.StopTimer(timer), Color.yellow);
        }

        public void LoadGame() {
            if (Serialization.CheckSaveExists("Save.dat")) {
                // If the save file exists
                Log.Message("Save file found - Loading Save Data");
                SaveGameData data = Serialization.Load("Save.dat");
                LoadData(data);
            }
            else {
                // Create a default save file
                Log.Message("No Save file found");
                SaveGame();
            }
        }
        
        private SaveGameData GenerateSaveData() {
            var items = allItems.Select(item => item.CreateSaveData()).ToList();
            return new SaveGameData(items);
        }

        private void LoadData(SaveGameData data) {
            for (int i = 0; i < allItems.Count; i++) {
                allItems[i].LoadSaveData(data.itemSaveData[i]);
            }
        }
        
        // Struct that holds the different data for saving
        // Currently just gets a list of SaveData from the Item Class
        
        [System.Serializable]
        public struct SaveGameData {

            public List<SO_Item.SaveData> itemSaveData;
            
            public SaveGameData(List<SO_Item.SaveData> itemSaveData) {
                this.itemSaveData = itemSaveData;
            }
        }
    }
}