using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;
using Utils;

namespace Data {
    [CreateAssetMenu(fileName = "RoundData", menuName = "ScriptableObjects/Data/RoundData", order = 0)]
    public class RoundData : ScriptableObject {

        public enum RoundType {warmup, normal, timeAttack, precision}
        public RoundType roundType;
        public enum SpeedBonusType {fast, super}

        public float roundTextTime = 2.1f;
        public int currentRound;
        public int roundLength;
        public int roundStep;
        public float maxRoundTime;
        public float minRoundTime;
        public float roundTimeLimit;
        public float lastComboTime;
        
        // Regular Game Mode HHH
        [FoldoutGroup("Regular Events")]       public UnityEvent<int> OnGameBegin;
        [FoldoutGroup("Regular Events")]       public UnityEvent<int> OnRoundInit;
        [FoldoutGroup("Regular Events")]       public UnityEvent<int> OnRoundBegin;
        [FoldoutGroup("Regular Events")]       public UnityEvent<int> OnRoundComplete;
        [FoldoutGroup("Regular Events")]       public UnityEvent<float> OnComboBegin;
        [FoldoutGroup("Regular Events")]       public UnityEvent OnComboComplete;
        [FoldoutGroup("Regular Events")]       public UnityEvent<SpeedBonusType> OnSpeedBonus;
        [FoldoutGroup("Regular Events")]       public UnityEvent<int, float> OnLogTimer;

        // Time Attack Game Mode
        [TitleGroup("TIME ATTACK ROUND")]
        public int timeAttackRoundDivisor = 4;
        public float timeAttackLength;
        public float timeAttackRoundClock;
        public float timeAttackPenalty;
        public float timeAttackReward;
        // Events
        [FoldoutGroup("Time Attack Events")]public UnityEvent OnTimeAttackRoundBegin;
        [FoldoutGroup("Time Attack Events")]public UnityEvent OnTimeAttackTargetTimedOut;
        [FoldoutGroup("Time Attack Events")]public UnityEvent<int> OnTimeAttackRoundComplete;
        
        // Precision Game Mode
        [TitleGroup("PRECISION ROUND")]
        public int precisionRoundDivisor = 8;
        

        private void CalcRoundTime() {
            // Calculation for round time limit - needs refining
            roundTimeLimit = maxRoundTime - currentRound * 0.5f;
            roundTimeLimit = Mathf.Max(roundTimeLimit, minRoundTime);
        }

        public void BeginGame() {
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
            // Log the time of the combo
            OnLogTimer?.Invoke(combo, elapsedTime);
        }

        public void SpeedBonus(SpeedBonusType type) {
            OnSpeedBonus?.Invoke(type);
        }

        public void BeginTimeAttackRound() {
            Log.Message("Beginning Time Attack Round!", Color.blue);            
            OnTimeAttackRoundBegin?.Invoke();
            // currentRound++;
        }
        
        public void TimeAttackTargetTimedOut() {
            OnTimeAttackTargetTimedOut?.Invoke();
        }

        public void EndTimeAttackRound() {
            currentRound++;
            OnTimeAttackRoundComplete?.Invoke(currentRound);
        }

        public IEnumerator TimeAttackRoundTimer() {
            timeAttackRoundClock = timeAttackLength;
            while (timeAttackRoundClock > 0) {
                timeAttackRoundClock -= Time.deltaTime;
                yield return null;
            }
        }
    }
}