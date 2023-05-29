using UI;
using Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using TARGET = Data.TargetData.Target;

namespace Managers {
    public class TimeAttackManager : MonoBehaviour {
        
        // This class manages the Time Attack round
        // Spawning a single target with a short timer before it disappears
        // If you miss a target, you lose time
        public TARGET currentTarget;
        
        [SerializeField] private Transform targetPool;
        [SerializeField] private TimeAttackTarget targetPrefab;
        [SerializeField] private TextMeshProUGUI streakText;
        
        private DataWrangler.GameData gd;
        private TimeAttackTarget currentTATarget;
        private int streak; 

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.roundData.OnTimeAttackRoundBegin.AddListener(BeginRound);
            gd.roundData.OnTimeAttackTargetTimedOut.AddListener(TargetTimeOut);
            gd.roundData.OnTimeAttackRoundComplete.AddListener(EndRound);
            gd.eventData.OnPunchTimeAttack.AddListener(CheckTarget);
        }
        
        private void BeginRound() {
            streak = 0;
            FormatStreakText();
            streakText.gameObject.SetActive(true);
            streakText.DOScale(1, 0.2f).From(0);
            SpawnNewTarget();
            StartCoroutine(gd.roundData.TimeAttackRoundTimer());
        }
        
        private void SpawnNewTarget() {
            if (gd.roundData.roundType != RoundData.RoundType.timeAttack) return;
            // Get a random index
            int newTarget = Random.Range(0, 3);
            // Set the current target index
            currentTarget = (TARGET)newTarget;
            // Get a target from the pool
            TimeAttackTarget target = GetTargetFromPool();
            // Set the current target object
            currentTATarget = target;
            // Initialize the target
            target.Init((TARGET)newTarget);
        }
        
        private TimeAttackTarget GetTargetFromPool() {
            foreach (Transform target in targetPool) {
                if (target.gameObject.activeSelf) continue;
                return target.GetComponent<TimeAttackTarget>();
            }

            return Instantiate(targetPrefab, targetPool);
        }
        
        private void CheckTarget(TARGET target) {
            if (target != currentTarget) {
                ResetStreak();
                gd.roundData.timeAttackRoundClock -= gd.roundData.timeAttackPenalty;
                return;
            }

            currentTATarget.DisableSelfHit();
            streak++;
            FormatStreakText();
            // Add score based on streak
            gd.playerData.UpdateScore(1 + streak);
            SpawnNewTarget();
        }

        private void ResetStreak() {
            streak = 0;
            FormatStreakText();
        }

        private void FormatStreakText() {
            streakText.text = "STREAK: " + streak;
        }

        private void TargetTimeOut() {
            gd.roundData.timeAttackRoundClock -= gd.roundData.timeAttackPenalty;
            SpawnNewTarget();   
        }
        
        private void EndRound(int unused) {
            streakText.DOScale(0, 0.2f).OnComplete(()=>streakText.gameObject.SetActive(false));
            currentTATarget.DisableSelfMiss();
        }
    }
}