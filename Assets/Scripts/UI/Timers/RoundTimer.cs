using TMPro;
using System;
using Managers;
using UnityEngine;
using System.Text;
using Data;
using DG.Tweening;
using UnityEngine.UI;

namespace UI {
    public class RoundTimer: MonoBehaviour {
        
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private RectTransform timerRect;
        [SerializeField] private float widthMultiplier = 50f;
        [SerializeField] private Slider slider;
        [SerializeField] private Image fill;
        [GradientUsage(true)]
        [SerializeField] private Gradient gradient;
        [SerializeField] private float maxTime;
        [SerializeField] private int timerUpdateFrames = 3;

        private Vector3 defaultTextScale;
        private Vector3 defaultTimeBarScale;
        private float currentTime;
        private bool active;
        private int frameUpdate;
        private TimeSpan ts;

        private StringBuilder timeString;

        private DataWrangler.GameData gd;
        
        private void Awake() {
            
            gd = DataWrangler.GetGameData();
            SubscribeEvents();

            slider.value = 1;
            fill.color = gradient.Evaluate(slider.value);
            defaultTextScale = timerText.rectTransform.localScale;
            defaultTimeBarScale = timerRect.localScale;
        }

        private void SubscribeEvents() {
            gd.eventData.OnNewGame.AddListener(NewGame);
            gd.eventData.OnGameOver.AddListener(CollapseTimer);
            gd.roundData.OnRoundBegin.AddListener(SetTimerSize);
            gd.roundData.OnComboBegin.AddListener(StartTimer);
            gd.roundData.OnComboComplete.AddListener(StopTimer);
            gd.roundData.OnBonusRoundBegin.AddListener(SetBonusTimer);
        }

        private void NewGame() {
            ScaleTimerDown();
        }

        private void ScaleTimerDown() {
            timerRect.localScale = Vector3.zero;
            timerText.rectTransform.localScale = Vector3.zero;
        }

        private void SetBonusTimer() {
            SetTimerSize(0);
            StartTimer(gd.roundData.bonusRoundLength);
        }

        private void SetTimerSize(int round) {
            
            float width = 0;
            
            switch (gd.roundData.roundType) {
                case RoundData.RoundType.warmup:
                    break;
                case RoundData.RoundType.normal:
                    width = gd.roundData.roundTimeLimit * widthMultiplier;
                    break;
                case RoundData.RoundType.bonus:
                    width = gd.roundData.bonusRoundLength * widthMultiplier / 2;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            timerRect.sizeDelta = new Vector2(width, timerRect.sizeDelta.y);
            ScaleUpTimer();
        }

        private void ScaleUpTimer() {
            timerRect.DOScale(defaultTimeBarScale, 0.5f);
            timerText.rectTransform.DOScale(defaultTextScale, 0.5f);
        }

        private void StartTimer(float timeLimit) {
            currentTime = timeLimit;
            maxTime = timeLimit;
            active = true;
        }

        private void StopTimer() {
            active = false;
            UpdateGraphics();
        }

        private void CollapseTimer() {
            active = false;
            ScaleTimerDown();
            
        }
        
        private void FormatTimer() {
            
            switch (currentTime) {
                case <= 0 when gd.roundData.roundType == RoundData.RoundType.normal:
                    timerText.text = "TIMEOUT";
                    timerText.color = gradient.Evaluate(slider.value);
                    gd.eventData.Miss();
                    active = false;
                    return;
                case <= 0 when gd.roundData.roundType == RoundData.RoundType.bonus:
                    gd.roundData.EndBonusRound();
                    active = false;
                    ScaleTimerDown();
                    return;
            }

            timeString = new StringBuilder("TIMER: "+ GetFormattedString());
            timerText.text = timeString.ToString();
            timerText.color = gradient.Evaluate(slider.value);
        }
        
        private string GetFormattedString() {
            
            ts = TimeSpan.FromSeconds(currentTime);
            return $"{ts.Seconds:00}.{ts.Milliseconds:000}";
        }

        private void FixedUpdate() {

            if (!active) return;

            currentTime -= Time.deltaTime;
            frameUpdate++;

            UpdateGraphics();
            if (frameUpdate % timerUpdateFrames == 0) {
                FormatTimer();
            }
        }

        private void UpdateGraphics() {
            slider.value = currentTime / maxTime;
            fill.color = gradient.Evaluate(slider.value);
        }
    }
}