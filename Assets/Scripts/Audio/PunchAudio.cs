using Managers;
using UnityEngine;
using TARGET = Data.TargetData.Target;

namespace Audio {
    public class PunchAudio : MonoBehaviour {

        [SerializeField] private AudioEvent[] voiceEvents;
        [SerializeField] private AudioEvent bagPunch;
        [SerializeField] private AudioSource voiceSource;
        [SerializeField] private AudioSource bagSource;
        
        private DataWrangler.GameData gd;

        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnPunchNormal.AddListener(PunchBag);
            gd.eventData.OnPunchWarmup.AddListener(PunchOnly);
            gd.eventData.OnPunchBonus.AddListener(PunchOnly);
        }

        private void PunchBag(TARGET target) {
            voiceEvents[(int)target].Play(voiceSource);
            bagPunch.Play(bagSource);
        }

        private void PunchOnly(TARGET target) {
            voiceEvents[(int)target].Play(voiceSource);
        }
    }
}