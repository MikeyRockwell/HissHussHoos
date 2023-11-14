using UI;
using Utils;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine.Serialization;
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
        public Target currentPrecisionTarget;
        public int step;
        public enum ComboLength {
            one   = 0,
            three = 1,
            five  = 2
        }
        public int fiveHitComboRoundThreshold = 5;
        public float oneHitComboChance    = 0.25f;
        public float fiveHitComboChance   = 0.25f;
        public float oneHitComboTimeMult  = 0.33f;
        public float fiveHitComboTimeMult = 1.55f;
        
        // EVENTS
        public UnityEvent OnTargetsReset; // Listeners: RegularTarget

        // Precision Targets
        [TitleGroup("Precision Round")] 
        public float precisionAccuracy;
        public float perfectXPosition = 0.5f;

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