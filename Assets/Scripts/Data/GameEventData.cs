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
        public bool inputEnabled;
        // Called when the game is launched for the first time
        public UnityEvent OnGameFirstLaunch;
        
        // Called when the game data is first loaded
        public UnityEvent OnGameInit;
        
        // Different punches
        public UnityEvent<TARGET> OnPunchWarmup;
        public UnityEvent<TARGET> OnPunchNormal;
        public UnityEvent<TARGET> OnPunchTimeAttack;

        // Hit and Miss target
        public UnityEvent<int> OnHit;
        public UnityEvent OnMiss;
        
        // Game over
        public UnityEvent OnGameOver;
        
        public void FirstLaunch() {
            Log.Message("Initializing Brand New Game", Color.green);
            OnGameFirstLaunch?.Invoke();
        }
        
        public void InitializeGame() {
            inputEnabled = true;
            OnGameInit?.Invoke();
        }

        public void PunchWarmup(TARGET target) {
            OnPunchWarmup?.Invoke(target);
        }
        public void PunchNormal(TARGET target) {
            Log.Message("Punching Normal");
            if (!inputEnabled) return;
            OnPunchNormal?.Invoke(target);
        }

        public void PunchTimeAttack(TARGET target) {
            if (!inputEnabled) return;
            OnPunchTimeAttack?.Invoke(target);
        }

        public void Hit(int step) {
            OnHit?.Invoke(step);
        }
        
        public void Miss() {
            OnMiss?.Invoke();
        }
        
        public void GameOver() {
            OnGameOver?.Invoke();
        }
    }
}