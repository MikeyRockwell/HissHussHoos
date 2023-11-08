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
        public Target currentTimeAttackTarget;
        public int step;
        public enum ComboLength {
            one   = 0,
            three = 1,
            five  = 2
        }
        public int fiveHitComboRoundThreshold = 5;
        public float oneHitComboChance = 0.25f;
        public float fiveHitComboChance = 0.25f;
        public float oneHitComboTimeMult = 0.33f;
        public float fiveHitComboTimeMult = 1.55f;
        
        // EVENTS
        public UnityEvent OnTargetsReset; // Listeners: RegularTarget

        // Bonus targets
        [TitleGroup("Bonus Round")] public List<TimingTarget> bonusTargets;
        public Vector2[] punchTargets;

        public float bonusTargetSpeedMultiplier = 1;
        public float perfectDistanceThresh = 0.02f;
        public float okDistanceThresh = 0.1f;

        public void CreateTargetSet(int length) {
            // Regular Target Set
            // Assigns random Targets to currentTarget array
            // Resets step to 0
            step = 0;
            // Create new array
            currentSet = new Target[length];
            // Fill array with random targets
            for (int i = 0; i < currentSet.Length; i++) currentSet[i] = (Target)Random.Range(0, 3);
            // Invoke the Targets Reset Event
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
            if (distance < perfectDistanceThresh)
                Log.Message("PERFECT!");
            else if (distance < okDistanceThresh)
                Log.Message("OK!");
            else
                Log.Message("NOT GREAT!");
        }
        
        public ComboLength GetComboLength(int round) {
            
            // If we are below the five hit combo round threshold
            if (round < fiveHitComboRoundThreshold)
                // Return either a one or three hit combo
                return Random.value < oneHitComboChance ? ComboLength.one : ComboLength.three;
            
            // Roll for chance at five hit combo
            if (Random.value < fiveHitComboChance) {
                return ComboLength.five;
            }

            return Random.value < oneHitComboChance ? ComboLength.one : ComboLength.three;
        }
    }
}