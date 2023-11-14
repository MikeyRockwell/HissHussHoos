using UI;
using Data;
using System;
using UnityEngine;
using TARGET = Data.TargetData.Target;

namespace Managers {
    public class TargetManager : MonoBehaviour {

        [SerializeField] private int numTargets = 3;
        [SerializeField] private Transform targetsParent;
        [SerializeField] private Transform mainCanvas;
        [SerializeField] private Transform singleTargetPool;
        [SerializeField] private Vector3[] singleTargetPositions;
        [SerializeField] private GameObject targetPrefab;
        private DataWrangler.GameData gd;
        private int targetStep;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            // Game events
            gd.eventData.OnPunchNormal.AddListener(CheckTarget);
            // Regular Rounds
            gd.roundData.OnRoundBegin.AddListener(BeginRound);
            gd.roundData.OnComboBegin.AddListener(BeginCombo);
            // Targets
            gd.targetData.OnTargetsReset.AddListener(InitTargets);
        }

        // REGULAR GAME MODE
        private void BeginRound(int round) {
            targetStep = 0;
            gd.targetData.step = targetStep;
            gd.roundData.BeginCombo();
        }
        
        // Determines the target set length for the current combo
        // This is called at the beginning of each combo
        // The length is determined by the current round
        private void BeginCombo(float time) {

            // Determine the length of the target set
            TargetData.ComboLength comboLength = gd.targetData.GetComboLength(gd.roundData.currentRound);

            switch (comboLength) {
                case TargetData.ComboLength.one:
                    numTargets = 1;
                    break;
                case TargetData.ComboLength.three:
                    numTargets = 3;
                    break;
                case TargetData.ComboLength.five:
                    numTargets = 5;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            gd.targetData.CreateTargetSet(numTargets);
        }

        private void InitTargets() {
            // Create new targets
            for (int i = 0; i < numTargets; i++) {
                Transform newTarget = GetTargetFromPool();
                newTarget.SetParent(targetsParent);
                newTarget.localPosition = numTargets == 1 ? Vector3.zero : singleTargetPositions[i];
                newTarget.localScale = Vector3.zero;
                newTarget.GetComponent<RegularTarget>().Init(i);
            }
        }

        private Transform GetTargetFromPool() {
            foreach (Transform target in singleTargetPool) {
                if (target.gameObject.activeSelf) continue;
                target.gameObject.SetActive(true);
                return target;        
            }
            return Instantiate(targetPrefab.transform);
        }
    

        private void CheckTarget(TARGET hit) {
            // MISS
            if (hit != gd.targetData.currentSet[targetStep]) {
                gd.eventData.Miss();
                return;
            }
            // HIT
            targetStep++;
            gd.targetData.step = targetStep;
            gd.eventData.Hit(targetStep - 1);

            if (targetStep != gd.targetData.currentSet.Length) return;

            CompleteCombo();
        }

        private void CompleteCombo() {
            gd.roundData.CompleteCombo();
            targetStep = 0;
        }
    }
}