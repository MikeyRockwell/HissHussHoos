using System;
using Managers;
using UnityEngine;

namespace Data.Tutorial {
    public class TutorialObjectDisabler : MonoBehaviour {
        [SerializeField] private GameObject[] objectsToDisable;

        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnDialogueStart.AddListener(delegate { DisableObjects(); });
            gd.eventData.OnDialogueEnd.AddListener(delegate { EnableObjects(); });
        }

        private void DisableObjects() {
            foreach (GameObject obj in objectsToDisable) obj.SetActive(false);
        }

        private void EnableObjects() {
            foreach (GameObject obj in objectsToDisable) obj.SetActive(true);
        }
    }
}