using Audio;
using UnityEngine;
using UnityEngine.Serialization;

namespace FX {
    public class AnimatedFX : MonoBehaviour {
        // This class plays the animation and then deactivates the game object

        public float animationLength;

        [SerializeField] private Animator animator;

        [FormerlySerializedAs("whipCrack")] [SerializeField]
        private SoundFXPlayer primarySFX;

        [SerializeField] private SoundFXPlayer secondarySFX;

        private static readonly int Play = Animator.StringToHash("Play");


        public void Init() {
            gameObject.SetActive(true);
            animator.SetTrigger(Play);
            // Play audio events
            primarySFX.PlayRandomAudio();
            Invoke(nameof(PlaySecondaryAudio), animationLength * 0.65f);
            // Start a timer to deactivate the game object
            Invoke(nameof(Deactivate), animationLength);
        }

        // Play the secondary audio event
        private void PlaySecondaryAudio() {
            secondarySFX.PlayRandomAudio();
        }

        // When the animation finishes playing, deactivate the game object
        public void Deactivate() {
            gameObject.SetActive(false);
        }
    }
}