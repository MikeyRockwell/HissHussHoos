using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Data {
    [CreateAssetMenu(fileName = "RoundData", menuName = "ScriptableObjects/Data/RoundData", order = 0)]
    public class RoundData : ScriptableObject {

        public enum RoundType {warmup, normal, bonus}
        public RoundType roundType;
        public enum SpeedBonusType {fast, super}

        public float roundTextTime = 1.5f;
        public int currentRound;
        public int roundLength;
        public int roundStep;
        public int bonusRound = 1;
        public float maxRoundTime;
        public float minRoundTime;
        public float roundTimeLimit;
        public float lastComboTime;
        
        // Regular Game Mode HHH
        [FoldoutGroup("Regular Events")]            public UnityEvent<int> OnGameBegin;
        [FoldoutGroup("Regular Events")]            public UnityEvent<int> OnRoundInit;
        [FoldoutGroup("Regular Events")]            public UnityEvent<int> OnRoundBegin;
        [FoldoutGroup("Regular Events")]            public UnityEvent<int> OnRoundComplete;
        [FoldoutGroup("Regular Events")]            public UnityEvent<float> OnComboBegin;
        [FoldoutGroup("Regular Events")]            public UnityEvent OnComboComplete;
        [FoldoutGroup("Regular Events")]            public UnityEvent<SpeedBonusType> OnSpeedBonus;
        [FoldoutGroup("Regular Events")]            public UnityEvent<int, float> OnLogTimer;

        // Bonus Round Game Mode
        [FoldoutGroup("Bonus Round Events")]        public float bonusRoundLength;
        [FoldoutGroup("Bonus Round Events")]        public float bonusTime;
        [FoldoutGroup("Bonus Round Events")]        public UnityEvent OnBonusRoundBegin;
        [FoldoutGroup("Bonus Round Events")]        public UnityEvent<int> OnBonusRoundComplete;

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
            currentRound++;
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

        public void BeginBonusRound() {
            OnBonusRoundBegin?.Invoke();
            currentRound++;
        }

        public void EndBonusRound() {
            OnBonusRoundComplete?.Invoke(currentRound);
        }

        public IEnumerator BonusRoundTimer() {
            bonusTime = bonusRoundLength;
            while (bonusTime > 0) {
                bonusTime -= Time.deltaTime;
                yield return null;
            }
        }
    }
}