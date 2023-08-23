using UnityEngine;
using UnityEngine.Events;

namespace Data.Tutorial {
    [CreateAssetMenu(fileName = "TutorialEvent", menuName = "ScriptableObjects/Tutorial/Event", order = 0)]
    public class TutorialEvent : ScriptableObject {
        public UnityEvent OnEventTriggered;
        public UnityEvent OnActionCompleted;

        public bool eventActive;
        public int eventRequirements;
        public string eventText;

        public void TriggerEvent() {
            OnEventTriggered?.Invoke();
        }

        public void CompleteAction() {
            OnActionCompleted?.Invoke();
        }
    }
}