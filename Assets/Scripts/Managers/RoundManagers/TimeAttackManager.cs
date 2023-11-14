using UI;
using Data;
using TMPro;
using DG.Tweening;
using UnityEngine;
using TARGET = Data.TargetData.Target;

namespace Managers {
    public class TimeAttackManager : MonoBehaviour {
        // This class manages the Time Attack round
        // Spawning a single target with a short timer before it disappears
        // If you miss a target, you lose time
        // public TARGET currentTarget;

        [SerializeField] private Transform targetPool;
        [SerializeField] private TimeAttackTarget targetPrefab;
        [SerializeField] private TextMeshProUGUI streakText;
        [SerializeField] private int targetsPerRound = 20;

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
            // Setup streak text
            streak = 0;
            FormatStreakText();
            streakText.gameObject.SetActive(true);
            streakText.DOScale(1, 0.2f).From(0);

            gd.roundData.roundStep = 0;
            SpawnNewTarget();
        }

        private void SpawnNewTarget() {
            // Check if the round is over
            if (gd.roundData.roundStep == targetsPerRound) {
                // Check if the player has a perfect streak give a bonus
                if (streak == targetsPerRound) {
                    // Add a health point if the player has less than max health
                    if (gd.playerData.health < gd.playerData.maxHealth) {
                        gd.roundData.timeAttackResult = RoundData.TimeAttackResult.addWaiirua;
                        gd.playerData.ChangeHealth(1);
                    }
                    // Otherwise add a score bonus
                    else if (gd.playerData.health == gd.playerData.maxHealth) {
                        gd.roundData.timeAttackResult = RoundData.TimeAttackResult.addScore;
                    }
                }
                // Otherwise there is no bonus
                else {
                    gd.roundData.timeAttackResult = RoundData.TimeAttackResult.none;
                }
                
                gd.roundData.EndTimeAttackRound();
                return;
            }

            // Get a random target
            int newTarget = Random.Range(0, 3);
            gd.targetData.currentTimeAttackTarget = (TARGET)newTarget;
            TimeAttackTarget target = GetTargetFromPool();
            currentTATarget = target;
            target.Init((TARGET)newTarget, gd.roundData.roundStep);

            gd.roundData.roundStep++;
        }

        private TimeAttackTarget GetTargetFromPool() {
            foreach (Transform target in targetPool) {
                if (target.gameObject.activeSelf) continue;
                return target.GetComponent<TimeAttackTarget>();
            }

            return Instantiate(targetPrefab, targetPool);
        }

        private void CheckTarget(TARGET target) { // OnPunchTimeAttack
            if (target != gd.targetData.currentTimeAttackTarget) {
                ResetStreak();
                currentTATarget.DisableSelfMiss();
                SpawnNewTarget();
                // gd.roundData.timeAttackRoundClock -= gd.roundData.timeAttackPenalty;
                return;
            }

            currentTATarget.DisableSelfHit();
            streak++;
            FormatStreakText();

            // Add score based on streak
            gd.eventData.HitTimeAttack(streak);
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
            ResetStreak();
            SpawnNewTarget();
        }

        private void EndRound(int unused) {
            streakText.DOScale(0, 0.2f).OnComplete(() => streakText.gameObject.SetActive(false));
            currentTATarget.DisableSelfMiss();
        }
    }
}