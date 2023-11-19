using UI;
using UnityEngine;
using System.Collections;
using TARGET = Data.TargetData.Target;

namespace Managers {
    public class PrecisionRoundManager : MonoBehaviour {
        
        // Manager to handle the Precision Round
        // This is the round where the player must hit the target at the right time
        // The target will move across the screen and the player must hit it when it is in the center
        
        [SerializeField] private Transform targetPool;
        [SerializeField] private PrecisionTarget[] targetPrefabs;
        [SerializeField] private float roundStartDelay = 1f;
        
        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.roundData.OnPrecisionRoundBegin.AddListener(BeginRound);
        }
        
        private void BeginRound() {
            gd.targetData.step = 0;
            StopAllCoroutines();
            StartCoroutine(nameof(StartTargetsDelayed));
        }

        private IEnumerator StartTargetsDelayed() {
            yield return new WaitForSeconds(roundStartDelay);
            SpawnNewTarget();
        }
        
        private void SpawnNewTarget() {
            // Check if the round is over
            if (gd.targetData.step == gd.roundData.precisionRoundLength) {
                return;
            }
            
            // Spawn a new target
            TARGET newTargetType = (TARGET)Random.Range(0, 3);
            gd.targetData.currentPrecisionTarget = newTargetType;
            PrecisionTarget target = GetTargetFromPool();
            bool finalTarget = gd.targetData.step + 1 == gd.roundData.precisionRoundLength;
            target.Init(newTargetType, finalTarget, gd.targetData.step);
            
            // Increment the target step
            gd.targetData.step++;
            
            // Start the timer to spawn the next target
            Vector2 intervalRange = gd.roundData.precisionIntervalRange;
            float interval = Random.Range(intervalRange.x, intervalRange.y);
            Invoke(nameof(SpawnNewTarget), interval);
        }
        
        private PrecisionTarget GetTargetFromPool() {
            // foreach (Transform target in targetPool) {
            //     if (target.gameObject.activeSelf) continue;
            //     return target.GetComponentInChildren<PrecisionTarget>();
            // }
            return Instantiate(targetPrefabs[Random.Range(0, targetPrefabs.Length)], targetPool);
        }
   
        
    }
}