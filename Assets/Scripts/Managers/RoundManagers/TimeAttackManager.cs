using Data;
using UI;
using DG.Tweening;
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
        
        private DataWrangler.GameData gd;
        private TimeAttackTarget currentTATarget;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.roundData.OnTimeAttackRoundBegin.AddListener(BeginRound);
            gd.roundData.OnTimeAttackTargetTimedOut.AddListener(TargetTimeOut);
            gd.roundData.OnTimeAttackRoundComplete.AddListener(EndRound);
            gd.eventData.OnPunchTimeAttack.AddListener(CheckTarget);
        }
        
        private void BeginRound() {
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
                gd.roundData.timeAttackRoundClock -= gd.roundData.timeAttackPenalty;
                return;
            }

            currentTATarget.DisableSelfHit();
            gd.roundData.timeAttackRoundClock += gd.roundData.timeAttackReward;
            SpawnNewTarget();
        }

        private void TargetTimeOut() {
            gd.roundData.timeAttackRoundClock -= gd.roundData.timeAttackPenalty;
            SpawnNewTarget();   
        }
        
        private void EndRound(int unused) {
            currentTATarget.DisableSelfMiss();
        }
    }
}