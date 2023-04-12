using UnityEngine;
using TARGET = Data.TargetData.Target;

namespace Managers {
    public class TargetManager : MonoBehaviour {

        private DataWrangler.GameData gd;

        private int targetStep;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            // Game events
            gd.eventData.OnPunchNormal.AddListener(CheckTarget);
            // Regular Rounds
            gd.roundData.OnRoundBegin.AddListener(BeginRound);
            gd.roundData.OnComboBegin.AddListener(BeginCombo);
        }
        
        // REGULAR GAME MODE
        private void BeginRound(int round) {
            targetStep = 0;
            gd.targetData.CreateTargetSet(3);
            gd.roundData.BeginCombo();
        }

        private void BeginCombo(float time) {
            gd.targetData.CreateTargetSet(3);
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
