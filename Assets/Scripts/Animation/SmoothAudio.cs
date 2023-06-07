using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace Animation
{
    public class SmoothAudio : MonoBehaviour
    {
        [SerializeField] private MMAudioAnalyzer analyzer;
        [SerializeField] private float fullAmplitudeMultiplier;
        [FormerlySerializedAs("bassBoostMultipler")] [SerializeField] private float bassBoostMultiplier;
        [SerializeField] private float timeMultiplier;
        [SerializeField] private float idleThreshold = 0.01f;

        public float intensity;
        public float sampledIntensity;
        public bool idling;
        
        private void Update()
        {
            // Get the intensity of the music
            sampledIntensity = analyzer.NormalizedBufferedAmplitude;
            // sampledIntensity = analyzer.Amplitude;
            // sampledIntensity = analyzer.RawSpectrum[0];
            sampledIntensity = analyzer.NormalizedBandLevels[1] * bassBoostMultiplier;
            sampledIntensity += analyzer.NormalizedBufferedAmplitude * fullAmplitudeMultiplier;
            // If the intensity is NaN, set it to 0
            if (float.IsNaN(sampledIntensity)) sampledIntensity = 0;
            // Clamp the intensity between 0 and 1
            sampledIntensity = Mathf.Clamp01(sampledIntensity);
            // Smooth the intensity
            // If the intensity is below a certain threshold, set it to a smooth pingpong
            if (sampledIntensity < idleThreshold)
            {
                idling = true;
                sampledIntensity = Mathf.PingPong(Time.time, 1);
                sampledIntensity = Utils.Conversion.Remap
                    (0, 1, 0, 0.25f, sampledIntensity);
                intensity = sampledIntensity;
            }
            else
            {
                idling = false;
                intensity = Mathf.Lerp(intensity, sampledIntensity, Time.deltaTime * timeMultiplier);
            }
        }
    }
}