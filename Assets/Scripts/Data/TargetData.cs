using UI;
using Utils;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TARGET = Data.TargetData.Target;

namespace Data {
    
    [CreateAssetMenu(fileName = "TargetData", menuName = "ScriptableObjects/Targets/TargetData")]
    public class TargetData : ScriptableObject {
        
        public enum Target {
            HISS = 0, 
            HUSS = 1, 
            HOOS = 2
        }
        
        // Regular Targets
        [TitleGroup("Regular Game Mode", "HISS HUSS HOOS")]
        public Target[] currentSet = new Target[3];
        public int step;
        
        public UnityEvent OnTargetsReset;
        
        // Bonus targets
        [TitleGroup("Bonus Round")] 
        public List<BonusTarget> bonusTargets;
        public Vector2[] punchTargets;
        
        public float bonusTargetSpeedMultiplier = 1;
        public float perfectDistanceThresh = 0.02f;
        public float okDistanceThresh = 0.1f;
        
        public void CreateTargetSet(int length) {
            // Regular Target Set
            // Assigns random Targets to currentTarget array
            step = 0;
            currentSet = new Target[length];
            for (int i = 0; i < currentSet.Length; i++) {
                currentSet[i] = (Target)Random.Range(0, 3);
            }
            OnTargetsReset?.Invoke();
        }

        public void CheckBonusPunch(TARGET target) {

            if (bonusTargets.Count == 0) return;
            if (target != bonusTargets[0].type) return;
            
            // Get distance
            float distance = 
                Vector2.Distance(punchTargets[(int)target], bonusTargets[0].transform.position);
            bonusTargets[0].DisableSelf();
            
            // Compare with threshold
            if (distance < perfectDistanceThresh) {
                Log.Message("PERFECT!");
            }
            else if (distance < okDistanceThresh) {
                Log.Message("OK!");
            }
            else {
                Log.Message("NOT GREAT!");
            }
        }
    }
}