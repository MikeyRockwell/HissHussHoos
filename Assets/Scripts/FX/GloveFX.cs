using Managers;
using UnityEngine;
using TARGET = Data.TargetData.Target;

namespace FX {
    public class GloveFX : MonoBehaviour {
        
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private float[] punchAngles;
        [SerializeField] private Vector2[] punchOffsets;
        
        private DataWrangler.GameData gd;
        private Transform p_xf;
        private ParticleSystem.EmissionModule p_emission;
        
        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnPunchNormal.AddListener(Punch);
            gd.eventData.OnPunchWarmup.AddListener(Punch);
            gd.eventData.OnPunchTimeAttack.AddListener(Punch);
            p_xf = transform;
            p_emission = particles.emission;
            p_emission.enabled = false;
        }
        
        private void Punch(TARGET target) {
            p_emission.enabled = true;
            p_xf.localRotation = Quaternion.Euler(0, 0, punchAngles[(int)target]);
            p_xf.localPosition = punchOffsets[(int)target];
            // Play particles
            particles.Play();
            Invoke(nameof(DisableParticles), 0.1f);
        }

        private void DisableParticles() {
            p_emission.enabled = false;
        }
    }
}