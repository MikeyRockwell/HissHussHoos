using UnityEngine;
using MoreMountains.Tools;

namespace Animation {
    // Class to smooth the audio intensity
    // Using different linear interpolation multipliers
    // If the intensity is below a certain threshold, set it to a smooth ping-pong
    public class SmoothAudio : MonoBehaviour {
        
        [SerializeField] private MMAudioAnalyzer analyzer;
        [SerializeField] private float fullAmplitudeMultiplier;
        [SerializeField] private float bassBoostMultiplier;
        [SerializeField] private float timeMultiplier;
        [SerializeField] private float idleThreshold = 0.01f;
        [SerializeField] private float idleMultiplier = 1.25f;

        public float intensity;
        public float rawIntensity;
        public float sampledIntensity;
        public bool idling;

        private void Update() {
            // Get the intensity of the music
            sampledIntensity = analyzer.NormalizedBufferedAmplitude;
            sampledIntensity = analyzer.NormalizedBandLevels[1] * bassBoostMultiplier;
            sampledIntensity += analyzer.NormalizedBufferedAmplitude * fullAmplitudeMultiplier;
            // If the intensity is NaN, set it to 0
            if (float.IsNaN(sampledIntensity)) sampledIntensity = 0;
            // Clamp the intensity between 0 and 1
            sampledIntensity = Mathf.Clamp01(sampledIntensity);
            
            // Smooth the intensity
            // If the intensity is below a certain threshold, set it to a smooth ping-pong
            if (sampledIntensity < idleThreshold) {
                idling = true;
                // Sample a ping-pong instead of the audio value
                sampledIntensity = Mathf.PingPong(Time.time * idleMultiplier, 1);
                // Smooth the raw intensity
                rawIntensity = Mathf.Lerp(rawIntensity, sampledIntensity, Time.deltaTime * timeMultiplier);
                sampledIntensity = Utils.Conversion.Remap
                    (0, 1, 0, 0.25f, rawIntensity);
                // Smooth the intensity from what it is to the new ping-pong value
                intensity = Mathf.Lerp(intensity, sampledIntensity, Time.deltaTime * timeMultiplier);
            }
            else { // Otherwise, smooth the intensity from what it is to the audio value
                idling = false;
                intensity = Mathf.Lerp(intensity, sampledIntensity, Time.deltaTime * timeMultiplier);
                rawIntensity = intensity;
            }
        }
    }
}