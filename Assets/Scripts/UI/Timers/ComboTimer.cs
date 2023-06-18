using Data;
using TMPro;
using System;
using Managers;
using UnityEngine;
using System.Text;
using DG.Tweening;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class ComboTimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerTextTop;
        [SerializeField] private TextMeshProUGUI timerTextBot;
        [SerializeField] private RectTransform timerRect;
        [SerializeField] private float widthMultiplier = 50f;
        [SerializeField] private Slider slider;
        [SerializeField] private Image fill;
        [SerializeField] private Image fill2;
        [GradientUsage(true)] [SerializeField] private Gradient gradient;
        [SerializeField] private float maxTime;
        [SerializeField] private int timerUpdateFrames = 3;

        private Vector3 defaultTextScale;
        private Vector3 defaultTimeBarScale;
        public float currentTime;
        private bool active;
        private int frameUpdate;
        private TimeSpan ts;

        private StringBuilder timeString;

        private DataWrangler.GameData gd;

        private void Awake()
        {
            gd = DataWrangler.GetGameData();
            SubscribeEvents();

            slider.value = 1;
            fill.color = gradient.Evaluate(slider.value);
            defaultTextScale = timerTextTop.rectTransform.localScale;
            defaultTimeBarScale = timerRect.localScale;
        }

        private void SubscribeEvents()
        {
            gd.eventData.OnGameInit.AddListener(NewGame);
            gd.eventData.OnGameOver.AddListener(CollapseTimer);
            gd.roundData.OnRoundBegin.AddListener(SetTimerSize);
            gd.roundData.OnComboBegin.AddListener(StartTimer);
            gd.roundData.OnComboComplete.AddListener(StopTimer);
            gd.roundData.OnTimeAttackRoundBegin.AddListener(CollapseTimer);
        }

        private void NewGame()
        {
            ScaleTimerDown();
        }

        private void ScaleTimerDown()
        {
            timerRect.localScale = Vector3.zero;
            timerTextTop.rectTransform.localScale = Vector3.zero;
        }

        private void SetTimeAttackTimer()
        {
            SetTimerSize(0);
            StartTimer(gd.roundData.timeAttackLength);
        }

        private void SetTimerSize(int round)
        {
            // float width = 0;
            //
            // switch (gd.roundData.roundType) {
            //     case RoundData.RoundType.warmup:
            //         break;
            //     case RoundData.RoundType.normal:
            //         width = gd.roundData.roundTimeLimit * widthMultiplier;
            //         break;
            //     case RoundData.RoundType.timeAttack:
            //         width = gd.roundData.timeAttackLength * widthMultiplier;
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException();
            // }
            // 
            // timerRect.sizeDelta = new Vector2(width, timerRect.sizeDelta.y);
            ScaleUpTimer();
        }

        private void ScaleUpTimer()
        {
            timerRect.DOScale(defaultTimeBarScale, 0.5f);
            timerTextTop.rectTransform.DOScale(defaultTextScale, 0.5f);
        }

        private void StartTimer(float timeLimit)
        {
            currentTime = timeLimit;
            maxTime = timeLimit;
            active = true;
        }

        private void StopTimer()
        {
            active = false;
            UpdateGraphics();
        }

        private void CollapseTimer()
        {
            active = false;
            ScaleTimerDown();
        }

        private void FormatTimer()
        {
            switch (currentTime)
            {
                case <= 0 when gd.roundData.roundType == RoundData.RoundType.normal:
                    timerTextTop.text = "TIMEOUT";
                    timerTextBot.text = "TIMEOUT";
                    timerTextTop.color = gradient.Evaluate(slider.value);
                    timerTextBot.color = gradient.Evaluate(slider.value);
                    gd.eventData.Miss();
                    active = false;
                    return;
                case <= 0 when gd.roundData.roundType == RoundData.RoundType.timeAttack:
                    gd.roundData.EndTimeAttackRound();
                    active = false;
                    ScaleTimerDown();
                    return;
            }

            timeString = new StringBuilder("TIMER: " + GetFormattedString());
            timerTextTop.text = timeString.ToString();
            timerTextTop.color = Color.black;
            timerTextBot.text = timeString.ToString();
            timerTextBot.color = gradient.Evaluate(slider.value);
        }

        private string GetFormattedString()
        {
            ts = TimeSpan.FromSeconds(currentTime);
            return $"{ts.Seconds:00}.{ts.Milliseconds:000}";
        }

        private void FixedUpdate()
        {
            if (!active) return;

            switch (gd.roundData.roundType)
            {
                // Regular round timer
                case RoundData.RoundType.normal:
                    currentTime -= Time.deltaTime;
                    break;
                case RoundData.RoundType.timeAttack:
                    currentTime = gd.roundData.timeAttackRoundClock;
                    break;
                case RoundData.RoundType.warmup:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            frameUpdate++;

            UpdateGraphics();
            // Update timer text every 3 frames
            if (frameUpdate % timerUpdateFrames == 0) FormatTimer();
        }

        private void UpdateGraphics()
        {
            slider.value = currentTime / maxTime;
            // scale fill 2 by slider value
            fill2.rectTransform.anchorMax = new Vector2(slider.value, 1);
            fill.color = gradient.Evaluate(slider.value);
        }
    }
}