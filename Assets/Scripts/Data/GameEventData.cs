using UnityEngine;
using UnityEngine.Events;
using Utils;
using TARGET = Data.TargetData.Target;

namespace Data {
    
    [CreateAssetMenu(fileName = "GameEventDatta", menuName = "ScriptableObjects/GameEvents/GameEventData")]
    public class GameEventData : ScriptableObject {
            
        // GAME EVENTS
        // Subscribable and callable events 
        // For game objects to link behaviour to
        
        // Called when the game is launched for the first time
        public UnityEvent OnGameFirstLaunch;
        
        // Called when the game data is first loaded
        public UnityEvent OnGameInit;
        
        // Different punches
        public UnityEvent<TARGET> OnPunchWarmup;
        public UnityEvent<TARGET> OnPunchNormal;
        public UnityEvent<TARGET> OnPunchBonus;

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
            OnGameInit?.Invoke();
        }

        public void PunchWarmup(TARGET target) {
            OnPunchWarmup?.Invoke(target);
        }
        public void PunchNormal(TARGET target) {
            OnPunchNormal?.Invoke(target);
        }

        public void PunchBonus(TARGET target) {
            OnPunchBonus?.Invoke(target);
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