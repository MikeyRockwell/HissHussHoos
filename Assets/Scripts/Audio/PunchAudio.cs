using Managers;
using UnityEngine;
using TARGET = Data.TargetData.Target;

namespace Audio {
    // Plays audio when punching
    public class PunchAudio : MonoBehaviour {
        [SerializeField] private SoundFXPlayer[] voiceSFXPlayers;
        [SerializeField] private SoundFXPlayer bagSFX;
        [SerializeField] private SoundFXPlayer wooshSFX;

        private DataWrangler.GameData gd;

        private void Awake() {
            // EVENTS
            gd = DataWrangler.GetGameData();
            gd.eventData.OnPunchWarmup.AddListener(PunchOnly);
            gd.eventData.OnPunchNormal.AddListener(PunchBag);
            gd.eventData.OnPunchTimeAttack.AddListener(PunchBag);
            gd.eventData.OnPunchPrecision.AddListener(PunchOnly);
        }

        private void PunchBag(TARGET target) {
            // Plays the voice SFX and the bag SFX
            voiceSFXPlayers[(int)target].PlayRandomAudio();
            bagSFX.PlayRandomAudio();
        }

        private void PunchOnly(TARGET target) {
            // If the menu is open, only play the bag SFX
            if (gd.customEvents.MenuOpen) {
                PunchBag(target);
                return;
            }
            // Plays the voice SFX and the woosh SFX
            voiceSFXPlayers[(int)target].PlayRandomAudio();
            wooshSFX.PlayRandomAudio();
        }
    }
}