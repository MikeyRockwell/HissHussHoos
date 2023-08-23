using UnityEngine;

namespace Animation {
    // Bounces the sprite based on the music amplitude
    public class SpriteBounce : MonoBehaviour {
        
        [SerializeField] private SmoothAudio smoothAudio;
        [SerializeField] private bool positionBounce;
        [SerializeField] private float minBounceHeight;
        [SerializeField] private float maxBounceHeight = 0.8f;
        [SerializeField] private float timeMultiplier = 0.8f;
        [SerializeField] private float bounceSubtraction = 0.08f;

        private Transform xf;
        private float bounce;

        private Vector3 currentScale;
        private float currentY;

        private void Awake() {
            // COMPONENTS
            xf = transform;
            // DEFAULTS
            minBounceHeight = !positionBounce ? xf.localScale.y : xf.position.y;
        }

        private void Update() {
            // Sample the current transform scale
            currentScale = xf.localScale;
            // Sample the current transform y position
            currentY = xf.position.y;
            // Convert the audio intensity to a bounce height
            // based on the min and max bounce heights
            bounce = Utils.Conversion.Remap
                (0, 1, minBounceHeight, maxBounceHeight, smoothAudio.intensity);
            // Bounce the sprite position or scale
            if (positionBounce) BouncePosition();
            else BounceScale();
        }

        private void BounceScale() {
            // update the bounce value by interpolating between the current scale and the bounce height
            bounce = Mathf.Lerp(currentScale.y, bounce, Time.deltaTime * timeMultiplier);
            // set the current scale to a new scale with the updated bounce value in the y axis
            currentScale = new Vector3(currentScale.x, bounce, currentScale.z);
            // set the transform scale to the current scale
            xf.localScale = currentScale;
        }

        private void BouncePosition() {
            // sample the current position
            Vector3 position = xf.position;
            // if the audio is idling, subtract the bounce subtraction value from the bounce height
            if (!smoothAudio.idling) bounce -= bounceSubtraction;
            // update the bounce value by interpolating between the current position and the bounce height
            currentY = Mathf.Lerp(currentY, bounce, Time.deltaTime * timeMultiplier);
            // set the current position to a new position with the updated bounce value in the y axis
            position = new Vector3(position.x, currentY, position.z);
            // set the transform position to the current position
            xf.position = position;
        }
    }
}