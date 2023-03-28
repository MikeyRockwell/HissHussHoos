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
            gd.eventData.OnPunchNormal.AddListener(Punch);
            gd.eventData.OnPunchWarmup.AddListener(Punch);
            gd.eventData.OnPunchBonus.AddListener(Punch);
        }

        private void Punch(TARGET target) {
            voiceEvents[(int)target].Play(voiceSource);
            bagPunch.Play(bagSource);
        }
    }
}