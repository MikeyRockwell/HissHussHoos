using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Data {
    [CreateAssetMenu(fileName = "UIData", menuName = "ScriptableObjects/UI/UIData", order = 0)]
    public class UIData : ScriptableObject {

        public float MenuAnimSpeed = 0.2f;
        
        public Ease DefaultMenuEase = Ease.InOutCirc;

        public Color MenuBackgroundColor;
        public Color HotPink;
        public Color LaserGreen;
        public Color Gold;
        public Color DisabledComboText;
        
        public UnityEvent<float> OnMoraleUpdated;
        public UnityEvent<int> OnMoralePointsEarned;
        
        public void UpdateMoraleUI(float morale) {
            OnMoraleUpdated?.Invoke(morale);
        }
        
        public void DisplayMoralePoints(int moraleEarned) {
            OnMoralePointsEarned?.Invoke(moraleEarned);
        }
    }
}