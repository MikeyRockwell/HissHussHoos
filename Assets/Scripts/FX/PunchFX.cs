using FX;
using Data;
using System;
using Managers;
using UnityEngine;
using Data.Customization;

namespace Animation {
    // Plays impact FX at the point of impact
    // Enables and disables glove FX objects that play other effects
    public class PunchFX : MonoBehaviour {

        public enum GloveFXType {
            none,
            rainbow,
            golden,
            xmas
        }
        
        [SerializeField] private SO_Category gloves;
        // Default smoke puffs
        [SerializeField] private ParticleSystem[] defaultParticleSystems;
        [SerializeField] private int puffsToEmit = 5;
        // Glove FX
        [SerializeField] private GloveFX rainbowFX;
        [SerializeField] private Transform goldenFX;
        [SerializeField] private Transform xmasFX;
        [SerializeField] private ParticleSystem[] goldenParticleSystems;
        [SerializeField] private ParticleSystem[] xmasParticleSystems;
        
        private DataWrangler.GameData gd;


        private void Awake() {
            // EVENTS
            gd = DataWrangler.GetGameData();
            gd.eventData.OnPunchWarmup.AddListener(CheckCustomizationMenuOpen);
            gd.eventData.OnPunchNormal.AddListener(PlayImpactFX);
            gd.eventData.OnPunchTimeAttack.AddListener(PlayImpactFX);
            gd.eventData.OnHitPrecisionFX.AddListener(PlayImpactFX);
            gloves.OnChangeItemColor.AddListener(SetGloveFX);
        }

        private void CheckCustomizationMenuOpen(TargetData.Target target) {
            // If the customization menu is open, play the impact FX
            if (gd.customEvents.MenuOpen) {
                PlayImpactFX(target);
            }
            if (xmasFX.gameObject.activeSelf) {
                xmasParticleSystems[(int)target].Emit(15);
            }
        }
        
        private void PlayImpactFXInt(int target) {
            PlayImpactFX((TargetData.Target)target);
        }

        private void PlayImpactFX(TargetData.Target target) { // Emits the default smoke puffs
            defaultParticleSystems[(int)target].Emit(puffsToEmit);

            if (goldenFX.gameObject.activeSelf) {
                goldenParticleSystems[(int)target].Emit(15);
            }
            if (xmasFX.gameObject.activeSelf) {
                xmasParticleSystems[(int)target].Emit(15);
            }
        }

        private void SetGloveFX(SO_Item item, Color color) { // Sets the glove FX based on the glove type
            switch (item.fxType) {
                case GloveFXType.none:
                    rainbowFX.gameObject.SetActive(false);
                    goldenFX.gameObject.SetActive(false);
                    xmasFX.gameObject.SetActive(false);
                    break;
                case GloveFXType.rainbow:
                    rainbowFX.gameObject.SetActive(true);
                    goldenFX.gameObject.SetActive(false);
                    xmasFX.gameObject.SetActive(false);
                    break;
                case GloveFXType.golden:
                    goldenFX.gameObject.SetActive(true);
                    rainbowFX.gameObject.SetActive(false);
                    xmasFX.gameObject.SetActive(false);
                    break;
                case GloveFXType.xmas:
                    goldenFX.gameObject.SetActive(false);
                    rainbowFX.gameObject.SetActive(false);
                    xmasFX.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}