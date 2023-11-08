using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Sirenix.OdinInspector;

namespace Data {
    [CreateAssetMenu(fileName = "RoundData", menuName = "ScriptableObjects/Data/RoundData", order = 0)]
    public class RoundData : ScriptableObject {
        public enum RoundType {
            warmup,
            normal,
            timeAttack,
            precision
        }

        public RoundType roundType;

        public enum SpeedBonusType {
            fast,
            super
        }

        public enum TimeAttackResult {
            none,
            addWaiirua,
            addScore,
        }
        
        public TimeAttackResult timeAttackResult;

        public float roundTextTime = 2.1f;
        public int currentRound;
        public int roundLength;
        public int roundStep;
        
        public float maxRoundTime;
        public float minRoundTime;
        public float roundTimeLimit;
        public bool roundActive;

        // Regular Game Mode HHH
        [FoldoutGroup("Regular Events")] public UnityEvent OnGameBeginDelayed;
        [FoldoutGroup("Regular Events")] public UnityEvent<int> OnGameBegin;
        [FoldoutGroup("Regular Events")] public UnityEvent<int> OnRoundInit;
        [FoldoutGroup("Regular Events")] public UnityEvent<int> OnRoundBegin;
        [FoldoutGroup("Regular Events")] public UnityEvent<int> OnRoundComplete;
        [FoldoutGroup("Regular Events")] public UnityEvent<float> OnComboBegin;
        [FoldoutGroup("Regular Events")] public UnityEvent OnComboComplete;
        [FoldoutGroup("Regular Events")] public UnityEvent<SpeedBonusType> OnSpeedBonus;
        [FoldoutGroup("Regular Events")] public UnityEvent<int, float> OnLogTimer;

        // Time Attack Game Mode
        [TitleGroup("TIME ATTACK ROUND")] public int timeAttackRoundDivisor = 4;
        public float timeAttackLength;
        public float timeAttackRoundClock;
        public int timeAttackPerfectScoreBonus = 100;

        // Events
        [FoldoutGroup("Time Attack Events")] public UnityEvent OnTimeAttackRoundBegin;
        [FoldoutGroup("Time Attack Events")] public UnityEvent OnTimeAttackTargetTimedOut;
        [FoldoutGroup("Time Attack Events")] public UnityEvent<int> OnTimeAttackRoundComplete;
        [FoldoutGroup("Time Attack Events")] public UnityEvent<string> OnTimeAttackAddWaiirua;
        [FoldoutGroup("Time Attack Events")] public UnityEvent OnTimeAttackPerfectScore;

        // Precision Game Mode
        [TitleGroup("PRECISION ROUND")] public int precisionRoundDivisor = 8;

        // Score Events
        [FoldoutGroup("Score Events")] public UnityEvent<int> OnScoreAdded;
        [FoldoutGroup("Score Events")] public UnityEvent<int> OnBonusScoreAdded;


        private void CalcRoundTime() {
            // Calculation for round time limit - needs refining
            roundTimeLimit = maxRoundTime - currentRound * 0.33f;
            roundTimeLimit = Mathf.Max(roundTimeLimit, minRoundTime);
        }

        public void BeginGameDelayed() {
            OnGameBeginDelayed?.Invoke();
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(1.0f).OnComplete(BeginGame);
        }

        private void BeginGame() {
            OnGameBegin?.Invoke(1);
        }

        public void InitRound() {
            OnRoundInit?.Invoke(currentRound);
        }


        public void BeginRound() {
            roundStep = 0;
            OnRoundBegin?.Invoke(currentRound);
        }

        public void BeginCombo() {
            CalcRoundTime();
            OnComboBegin?.Invoke(roundTimeLimit);
        }

        public void CompleteCombo() {
            // Increase the round step
            roundStep++;
            if (roundStep == roundLength) {
                // If we are at the end of the round
                currentRound++;
                OnComboComplete?.Invoke();
                OnRoundComplete?.Invoke(currentRound);
                return;
            }

            // Combo complete but still within round
            OnComboComplete?.Invoke();
            BeginCombo();
        }

        public void LogTimer(int combo, float elapsedTime) {
            OnLogTimer?.Invoke(combo, elapsedTime);
        }

        public void SpeedBonus(SpeedBonusType type) {
            OnSpeedBonus?.Invoke(type);
        }

        public void BeginTimeAttackRound() {
            OnTimeAttackRoundBegin?.Invoke();
        }

        public void TimeAttackTargetTimedOut() {
            OnTimeAttackTargetTimedOut?.Invoke();
        }

        public void EndTimeAttackRound() {
            currentRound++;
            OnTimeAttackRoundComplete?.Invoke(currentRound);

            switch (timeAttackResult) {
                case TimeAttackResult.none:
                    break;
                case TimeAttackResult.addWaiirua:
                    OnTimeAttackAddWaiirua?.Invoke("+1 WAIIRUA");
                    break;
                case TimeAttackResult.addScore:
                    OnTimeAttackPerfectScore?.Invoke();
                    break;
            }
        }

        public IEnumerator TimeAttackRoundTimer() {
            timeAttackRoundClock = timeAttackLength;
            while (timeAttackRoundClock > 0) {
                timeAttackRoundClock -= Time.deltaTime;
                yield return null;
            }
        }

        public void ScoreAdded(int score) {
            OnScoreAdded?.Invoke(score);
        }

        public void BonusScoreAdded(int score) {
            OnBonusScoreAdded?.Invoke(score);
        }
    }
}