using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class StartGameButton : MonoBehaviour {

        private Button button;
        private DataWrangler.GameData gd;

        private void Awake() {
            
            gd = DataWrangler.GetGameData();
            button = GetComponent<Button>();
            button.onClick.AddListener(()=> gd.roundData.BeginGame());
            gd.roundData.OnGameBegin.AddListener(Disable);
        }

        private void Disable (int arg0) {
            gameObject.SetActive(false);
        }
    }
}