using UnityEngine;
using UnityEngine.Events;
using TARGET = Data.TargetData.Target;

namespace Data {
    
    [CreateAssetMenu(fileName = "GameEventDatta", menuName = "ScriptableObjects/GameEvents/GameEventData")]
    public class GameEventData : ScriptableObject {
        
        // GLOBAL
        public UnityEvent OnNewGame;
        public UnityEvent OnLoadGame;
        public UnityEvent<TARGET> OnPunchWarmup;
        public UnityEvent<TARGET> OnPunchNormal;
        public UnityEvent<TARGET> OnPunchBonus;
        public UnityEvent<int> OnHit;
        public UnityEvent OnMiss;
        public UnityEvent OnGameOver;
        
        public void NewGame() {
            OnNewGame?.Invoke();
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