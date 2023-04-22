using System;
using UnityEngine;
using Utils;

namespace Managers {
    public class DebugControls : MonoBehaviour {

        [SerializeField] private MoraleManager moraleManager;
        private DataWrangler.GameData gd;
        

        private void Awake() {
            gd = DataWrangler.GetGameData();
        }

        private void Update() {
            
            if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftShift)) {
                Log.Message("Resetting All Items", gd.uIData.HotPink);
                gd.itemData.ResetItems();
            }

            if (Input.GetKey(KeyCode.UpArrow)) {
                moraleManager.AddMorale(10);
            }
        }
    }
}