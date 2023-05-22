using UnityEngine;

namespace FX {
    public class AnimatedFX : MonoBehaviour {
        
        // This class plays the animation and then deactivates the game object

        public float animationLength;
        
        [SerializeField] private Animator animator;
        
        public void Init() {
            gameObject.SetActive(true);
            animator.SetTrigger("Play");
            // Start a timer to deactivate the game object
            // Get the length of the animation
            Invoke(nameof(Deactivate), animationLength);
        }
        
        // When the animation finishes playing, deactivate the game object
        public void Deactivate() {
            gameObject.SetActive(false);
        }
    }
}