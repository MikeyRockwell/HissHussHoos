using UI;
using Data;
using Utils;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Random = UnityEngine.Random;
using TARGET = Data.TargetData.Target;

namespace Managers {
    public class GameTester : MonoBehaviour {

#if UNITY_EDITOR        

        // Automated game player
        public bool AutoPlay;
        [Range(0, 100)] public float skillLevel;
        public PunchButton[] PunchButtons;
        [Range(1,5)]
        public float DelayMultiplier;
        public StartGameButton StartButton;
        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnGameReady.AddListener(StartGame);
            gd.roundData.OnComboBegin.AddListener(ComboBegin);
            gd.roundData.OnTimeAttackRoundBegin.AddListener(()=>Punch(0));
            gd.eventData.OnHitTimeAttack.AddListener(Punch);
            gd.eventData.OnHit.AddListener(Punch);
            gd.eventData.OnMiss.AddListener(()=> Punch(0));
        }

        private void StartGame() {
            if (!AutoPlay) return;
            
            Log.Message("Starting Game", Color.green);
            StartButton.GetComponent<Button>().onClick.Invoke();
        }
        
        private void ComboBegin(float arg0) {
            if (!AutoPlay) return;
            
            Log.Message("Starting Combo", Color.blue);
            Punch(0);
        }

        private void Punch(int arg0) {
            // if (gd.targetData.step == gd.targetData.currentSet.Length) return;
            if (!AutoPlay) return;
            switch (gd.roundData.roundType) {
                case RoundData.RoundType.warmup:
                    break;
                case RoundData.RoundType.normal:
                    StartCoroutine(nameof(AttemptPunch));
                    break;
                case RoundData.RoundType.timeAttack:
                    StartCoroutine(nameof(TimeAttackPunch));
                    break;
                case RoundData.RoundType.precision:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator AttemptPunch() {
            if (!AutoPlay) yield break;
            Log.Message("Attempting Punch", Color.gray);
            // Create random delay
            // float delay = Random.Range(0, (100 - skillLevel) * 0.01f);
            // delay = Mathf.Clamp(delay, gd.playerData.punchSpeed * DelayMultiplier, 1);
            float delay = gd.playerData.punchSpeed * DelayMultiplier;
            Log.Message("Delay: " + delay, Color.gray);
            yield return new WaitForSeconds(delay);
            
            // Roll for correct punch
            float roll = Random.Range(0, 100);
            Log.Message("Rolled: " + roll, Color.blue);
            
            if (roll > skillLevel) {
                Log.Message("Missed", Color.red);
                // Select a random punch that is not the correct punch
                int correctPunch = (int)gd.targetData.currentSet[gd.targetData.step];
                int punch = Random.Range(0, PunchButtons.Length);
                while (punch == correctPunch) {
                    punch = Random.Range(0, PunchButtons.Length);
                }
                PunchButtons[punch].Punch();
                yield break;
            }
            
            Log.Message("Hit", Color.green);
            if (gd.playerData.punching) {
                yield return new WaitForSeconds(0.1f);
            }
            PunchButtons[(int)gd.targetData.currentSet[gd.targetData.step]].Punch();
        }

        private IEnumerator TimeAttackPunch() {
            if (!AutoPlay) yield break;
            
            Log.Message("Attempting Time Attack Punch", Color.gray);
            float delay = gd.playerData.punchSpeed * DelayMultiplier;
            yield return new WaitForSeconds(delay);
            
            // Roll for correct punch
            float roll = Random.Range(0, 100);
            Log.Message("Rolled: " + roll, Color.blue);
            
            if (roll > skillLevel) {
                Log.Message("Missed", Color.red);
                // Select a random punch that is not the correct punch
                int correctPunch = (int)gd.targetData.currentTimeAttackTarget;
                int punch = Random.Range(0, PunchButtons.Length);
                while (punch == correctPunch) {
                    punch = Random.Range(0, PunchButtons.Length);
                }
                PunchButtons[punch].Punch();
                yield break;
            }
            if (gd.playerData.punching) {
                yield return new WaitForSeconds(0.1f);
            }
            
            Log.Message("Hit", Color.green);
            PunchButtons[(int)gd.targetData.currentTimeAttackTarget].Punch();
        }
    }
#endif
}