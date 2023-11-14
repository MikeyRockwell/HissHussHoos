using System;
using Data.Tutorial;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Utils;
using TARGET = Data.TargetData.Target;

namespace Data {
    [CreateAssetMenu(fileName = "GameEventDatta", menuName = "ScriptableObjects/GameEvents/GameEventData")]
    public class GameEventData : ScriptableObject {
        // GAME EVENTS
        // Subscribable and callable events 
        // For game objects to link behaviour to

        // Override inputs on punch buttons etc
        public bool inputEnabled;

        // Called when the game is launched for the first time
        public UnityEvent OnGameFirstLaunch;

        // Called when the game data is first loaded
        public UnityEvent OnGameInit;
        public UnityEvent OnGameInitComplete;
        public UnityEvent OnGameReady;
        
        public int initMethods;

        // Different punches
        public UnityEvent<TARGET> OnPunchWarmup;
        public UnityEvent<TARGET> OnPunchNormal;
        public UnityEvent<TARGET> OnPunchTimeAttack;
        public UnityEvent<TARGET> OnPunchPrecision;

        // Hit and Miss target
        public UnityEvent<int> OnHit;
        public UnityEvent<int> OnHitTimeAttack;
        public UnityEvent      OnMiss;
        public UnityEvent<int> OnHitPrecision;
        public UnityEvent<int> OnMissPrecision;

        // Game over
        public UnityEvent OnGameOver;

        // Tutorials
        public UnityEvent<SO_Dialogue> OnDialogueStart;
        public UnityEvent<SO_Dialogue> OnDialogueEnd;

        public void FirstLaunch() {
            Log.Message("Initializing Brand New Game", Color.green);
            OnGameFirstLaunch?.Invoke();
        }

        public void InitializeGame() {
            // Get the number of listeners of the OnGameInit event
            // initMethods = OnGameInit.GetPersistentEventCount();
            // Log.Message("Initialization methods: " + initMethods, Color.green);
            OnGameInit?.Invoke();
        }

        public void RegisterCallBack() {
            initMethods--;
            if (initMethods > 0) return;
            OnGameInitComplete?.Invoke();
        }

        public void GameReady() {
            OnGameReady?.Invoke();
        }

        public void PunchWarmup(TARGET target) {
            OnPunchWarmup?.Invoke(target);
        }

        public void PunchNormal(TARGET target) {
            if (!inputEnabled) return;
            OnPunchNormal?.Invoke(target);
        }

        public void PunchTimeAttack(TARGET target) {
            if (!inputEnabled) return;
            OnPunchTimeAttack?.Invoke(target);
        }
        
        public void PunchPrecision(TARGET target) {
            if (!inputEnabled) return;
            OnPunchPrecision?.Invoke(target);
        }
        
        public void HitTimeAttack(int streak) {
            OnHitTimeAttack?.Invoke(streak);
        }

        public void Hit(int step) {
            OnHit?.Invoke(step);
        }

        public void Miss() {
            OnMiss?.Invoke();
        }

        public void HitPrecision(int index) {
            OnHitPrecision?.Invoke(index);
        }

        public void MissPrecision(int index) {
            OnMissPrecision?.Invoke(index);            
        }

        public void GameOver() {
            OnGameOver?.Invoke();
        }

        public void StartDialogue(SO_Dialogue dialogue) {
            OnDialogueStart?.Invoke(dialogue);
        }

        public void EndDialogue(SO_Dialogue dialogue) {
            OnDialogueEnd?.Invoke(dialogue);
        }
    }
}