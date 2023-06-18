using Managers;
using UnityEngine;
using TARGET = Data.TargetData.Target;

namespace Audio
{
    public class PunchAudio : MonoBehaviour
    {
        [SerializeField] private SoundFXPlayer[] voiceSFXPlayers;
        [SerializeField] private SoundFXPlayer bagSFX;
        [SerializeField] private SoundFXPlayer wooshSFX;

        private DataWrangler.GameData gd;

        private void Awake()
        {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnPunchWarmup.AddListener(PunchOnly);
            gd.eventData.OnPunchNormal.AddListener(PunchBag);
            gd.eventData.OnPunchTimeAttack.AddListener(PunchBag);
        }

        private void PunchBag(TARGET target)
        {
            voiceSFXPlayers[(int)target].PlayRandomAudio();
            bagSFX.PlayRandomAudio();
        }

        private void PunchOnly(TARGET target)
        {
            if (gd.customEvents.MenuOpen)
            {
                PunchBag(target);
                return;
            }

            voiceSFXPlayers[(int)target].PlayRandomAudio();
            wooshSFX.PlayRandomAudio();
        }
    }
}