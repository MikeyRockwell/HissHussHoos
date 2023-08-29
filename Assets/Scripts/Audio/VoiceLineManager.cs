using Data;
using System;
using Managers;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

namespace Audio {
    /// <summary>
    /// Singleton
    /// Holds references to all the voice lines as AudioEvents
    /// Plays the voice lines when called via events
    /// </summary>
    public class VoiceLineManager : Singleton<VoiceLineManager> {
        [TitleGroup("Audio Events")] [FoldoutGroup("AudioEvents")]
        public AudioEvent startGame;

        [FoldoutGroup("AudioEvents")] public AudioEvent endGame;
        [FoldoutGroup("AudioEvents")] public AudioEvent quickCombo;
        [FoldoutGroup("AudioEvents")] public AudioEvent superQuickCombo;
        [FoldoutGroup("AudioEvents")] public AudioEvent morale;
        [FoldoutGroup("AudioEvents")] public AudioEvent moraleBoost;
        [FoldoutGroup("AudioEvents")] public AudioEvent newRound;
        [FoldoutGroup("AudioEvents")] public AudioEvent hurt;
        [FoldoutGroup("AudioEvents")] public AudioEvent changeItem;
        [FoldoutGroup("AudioEvents")] public AudioEvent changeColor;
        [FoldoutGroup("AudioEvents")] public AudioEvent unlockItem;
        [FoldoutGroup("AudioEvents")] public AudioEvent personalBest;
        [FoldoutGroup("AudioEvents")] public AudioEvent perfectRound;
        [FoldoutGroup("AudioEvents")] public AudioEvent lowScore;

        [SerializeField] private AudioMixerGroup voiceLineMixerGroup;
        [SerializeField] private AudioMixerGroup voiceLineFXMixerGroup;

        private DataWrangler.GameData gd;
        public bool priorityAudioPlaying = true;

        protected override void Awake() {
            // SINGLETON
            base.Awake();
            // EVENTS
            gd = DataWrangler.GetGameData();
            gd.roundData.OnGameBegin.AddListener               (delegate { PlayVoiceLine(startGame);    });
            gd.customEvents.OnItemChanged.AddListener          (delegate { PlayVoiceLine(changeItem);   });
            gd.customEvents.OnColorChanged.AddListener         (delegate { PlayVoiceLine(changeColor);  });
            gd.playerData.md.OnMoraleBoost.AddListener         (delegate { PlayVoiceLine(moraleBoost);  });
            gd.customEvents.OnItemUnlocked.AddListener         (delegate { PlayVoiceLine(unlockItem);   });
            gd.customEvents.OnColorUnlocked.AddListener        (delegate { PlayVoiceLine(unlockItem);   });
            gd.roundData.OnTimeAttackPerfectScore.AddListener  (delegate { PlayVoiceLine(perfectRound); });
            gd.roundData.OnTimeAttackAddWaiirua.AddListener    (delegate { PlayVoiceLine(perfectRound); });
            gd.playerData.OnRegularScore.AddListener           (delegate { PlayVoiceLine(endGame);      });
            gd.playerData.OnLowScore.AddListener               (delegate { PlayVoiceLine(lowScore);     });
            gd.playerData.OnNewHighScore.AddListener           (delegate { PlayVoiceLine(personalBest); });
            gd.roundData.OnRoundComplete.AddListener           (delegate { BeginRound();                });
            gd.roundData.OnSpeedBonus.AddListener              (SpeedCombo);
            gd.eventData.OnMiss.AddListener                    (HurtAudio);
        }

        private void Start() {
            // The game starts with a priority boolean enabled
            // This stops voice lines from playing until the game is ready
            // After 3 seconds, the priority boolean is disabled
            Invoke(nameof(UnlockAudio), 3f);
        }

        private void UnlockAudio() {
            priorityAudioPlaying = false;
        }

        public void PlayVoiceLine(AudioEvent audioEvent) {
            // If the a priority audio is playing, don't play this audio
            if (priorityAudioPlaying) return;
            // If the audio is cooling down, don't play this audio
            if (audioEvent.coolingDown) return;
            // If the random chance fails, don't play this audio
            if (Random.value > audioEvent.chance) return;

            // Get the first available audio source
            AudioSource source = GetAvailableAudioSource();
            if (source == null) return;
            // Set the audio source's mixer group
            source.outputAudioMixerGroup = audioEvent.voiceEffects ? voiceLineFXMixerGroup : voiceLineMixerGroup;
            // Play the audio via the audio event
            audioEvent.PlayRandomClip(source);
            // Start the cooldown on the audio event
            StartCoroutine(audioEvent.CoolDown());
            // If the audio is not a priority, return
            if (!audioEvent.priority) return;
            // If the audio is a priority, set the priority boolean to true
            priorityAudioPlaying = true;
            // After the audio has finished playing, set the priority boolean to false
            Invoke(nameof(ResetPriorityAudio), audioEvent.currentClip.length);
        }

        private void ResetPriorityAudio() {
            priorityAudioPlaying = false;
        }

        private AudioSource GetAvailableAudioSource() {
            // Find the first available audio source
            foreach (Transform child in transform)
                if (!child.GetComponent<AudioSource>().isPlaying)
                    return child.GetComponent<AudioSource>();
            // If none are available, return null
            return null;
        }
        
        // DIFFERENT VOICE LINES
        private void HurtAudio() {
            // If player still has health, play the hurt voice line
            if (gd.playerData.health > 1) PlayVoiceLine(hurt);
        }

        private void SpeedCombo(RoundData.SpeedBonusType type) {
            // Play the correct voice line based on the speed bonus type
            switch (type) {
                case RoundData.SpeedBonusType.fast:
                    PlayVoiceLine(quickCombo);
                    break;
                case RoundData.SpeedBonusType.super:
                    PlayVoiceLine(superQuickCombo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void BeginRound() {
            // If the round is not the first round, play the new round voice line
            if (gd.roundData.currentRound > 1) PlayVoiceLine(newRound);
        }
    }
}