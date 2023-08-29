using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;

namespace FX {
    
    // Basic controller for the volume FX
    
    public class VolumeFX : MonoBehaviour {
        
        [Required]
        [SerializeField] private Volume volume;
        [SerializeField] private float maxWeight = 1;
        [SerializeField] private float duration = 0.25f;
        
        private void Awake() {
            volume.weight = 0;
        }
        
        public void AnimateVolume(float value) {
            // Tween the volume weight
            DOTween.To(() => volume.weight, x => volume.weight = x, value, duration);
        }
    }
}