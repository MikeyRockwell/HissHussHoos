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
            golden
        }
        
        [SerializeField] private SO_Category gloves;
        // Default smoke puffs
        [SerializeField] private ParticleSystem[] defaultParticleSystems;
        [SerializeField] private int puffsToEmit = 5;
        // Glove FX
        [SerializeField] private GloveFX rainbowFX;
        [SerializeField] private GloveFX goldenFX;
        
        private DataWrangler.GameData gd;


        private void Awake() {
            // EVENTS
            gd = DataWrangler.GetGameData();
            gd.eventData.OnPunchNormal.AddListener(PlayImpactFX);
            gd.eventData.OnPunchTimeAttack.AddListener(PlayImpactFX);
            gloves.OnChangeItemColor.AddListener(SetGloveFX);
        }

        private void PlayImpactFX(TargetData.Target target) { // Emits the default smoke puffs
            defaultParticleSystems[(int)target].Emit(puffsToEmit);
        }

        private void SetGloveFX(SO_Item item, Color color) { // Sets the glove FX based on the glove type
            switch (item.fxType) {
                case GloveFXType.none:
                    rainbowFX.gameObject.SetActive(false);
                    goldenFX.gameObject.SetActive(false);
                    break;
                case GloveFXType.rainbow:
                    rainbowFX.gameObject.SetActive(true);
                    goldenFX.gameObject.SetActive(false);
                    break;
                case GloveFXType.golden:
                    goldenFX.gameObject.SetActive(true);
                    rainbowFX.gameObject.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}