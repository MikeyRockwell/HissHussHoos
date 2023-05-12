using TMPro;
using System;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace UI {
    public class TimerRecording : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI bestText;

        private TimeSpan ts;
        private float currentTime;
        private string textString;
        
        private bool timerActive;
        private Vector3 defaultTextScale;

        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.roundData.OnComboBegin.AddListener(StartTimer);
            gd.roundData.OnComboComplete.AddListener(StopTimer);
            gd.eventData.OnGameOver.AddListener(CollapseTimer);
            // SetBestTimeText(PlayerPrefs.GetFloat("BestTime"));
            defaultTextScale = timerText.rectTransform.localScale;
            timerText.rectTransform.localScale = Vector3.zero;
        }

        private void CollapseTimer() {
            timerActive = false;
            timerText.text = "";
        }

        private void StartTimer(float timeLimit) {
            currentTime = 0;
            timerActive = true;
        }

        private void StopTimer() {
            timerActive = false;

            if (timerText.rectTransform.localScale == Vector3.zero) {
                timerText.rectTransform.DOScale(defaultTextScale, 0.2f);
            }
            
            textString = GetFormattedString(currentTime);
            timerText.text = "LAST: " + textString;

            if (!(currentTime < PlayerPrefs.GetFloat("BestTime"))) return;
            
            // SetBestTimeText(currentTime);
            // PlayerPrefs.SetFloat("BestTime", currentTime);
        }
        
        /*private void SetBestTimeText(float time) {

            if (time == 0.0f) {
                PlayerPrefs.SetFloat("BestTime", 999);
                bestText.text = "FASTEST COMBO: NONE";
                return;
            }
            
            string bestTimeString = GetFormattedString(time);
            bestText.text = "FASTEST COMBO: " + bestTimeString;
        }*/
        
        private string GetFormattedString(float time) {
            
            ts = TimeSpan.FromSeconds(time);
            return $"{ts.Seconds:00}.{ts.Milliseconds:000}";
        }

        private void Update() {
            
            if (!timerActive) return;

            currentTime += Time.deltaTime;
        }
    }
}