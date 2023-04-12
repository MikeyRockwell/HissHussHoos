using TMPro;
using Managers;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace UI {
    public class HealthGraphics : MonoBehaviour {
        
        [SerializeField] private Slider slider;
        [SerializeField] private Image fill;
        [GradientUsage(true)]
        [SerializeField] private Gradient gradient;
        [SerializeField] private TextMeshProUGUI underText;
        
        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.playerData.OnHealthChange.AddListener(RemoveHealth);
        }

        private void RemoveHealth(int amount) {
            
            DOTween.To(x => slider.value = x,
                slider.value,
                gd.playerData.GetHealth(),
                0.5f);
            fill.DOColor(gradient.Evaluate(gd.playerData.GetHealth()), 0.5f);
            underText.DOColor(gradient.Evaluate(gd.playerData.GetHealth()), 0.5f);
        }
    }
}