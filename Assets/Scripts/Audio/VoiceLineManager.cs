using System;
using Data;
using Data.Customization;
using Managers;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using Utils;
using Random = UnityEngine.Random;

namespace Audio
{
    public class VoiceLineManager : Singleton<VoiceLineManager>
    {
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

        [SerializeField] private AudioMixerGroup voiceLineMixerGroup;
        [SerializeField] private AudioMixerGroup voiceLineFXMixerGroup;

        private DataWrangler.GameData gd;
        public bool priorityAudioPlaying;

        protected override void Awake()
        {
            base.Awake();
            gd = DataWrangler.GetGameData();

            gd.roundData.OnGameBegin.AddListener(delegate { PlayVoiceLine(startGame); });
            gd.eventData.OnGameOver.AddListener(delegate { PlayVoiceLine(endGame); });
            gd.eventData.OnMiss.AddListener(HurtAudio);
            gd.roundData.OnSpeedBonus.AddListener(SpeedCombo);
            gd.customEvents.OnItemChanged.AddListener(delegate { PlayVoiceLine(changeItem); });
            gd.customEvents.OnColorChanged.AddListener(delegate { PlayVoiceLine(changeColor); });
            gd.playerData.md.OnMoraleBoost.AddListener(delegate { PlayVoiceLine(moraleBoost); });
            gd.roundData.OnRoundComplete.AddListener(delegate { BeginRound(); });
            gd.customEvents.OnItemUnlocked.AddListener(delegate { PlayVoiceLine(unlockItem); });
            gd.customEvents.OnColorUnlocked.AddListener(delegate { PlayVoiceLine(unlockItem); });
        }

        public void PlayVoiceLine(AudioEvent audioEvent)
        {
            if (priorityAudioPlaying) return;
            if (audioEvent.coolingDown) return;
            if (Random.value > audioEvent.chance) return;

            // Get the first available audio source
            AudioSource source = GetAvailableAudioSource();
            if (source == null) return;
            source.outputAudioMixerGroup = audioEvent.voiceEffects ? voiceLineFXMixerGroup : voiceLineMixerGroup;

            audioEvent.PlayRandomClip(source);

            // Start the cooldown on the audio event
            StartCoroutine(audioEvent.CoolDown());

            // If the audio is a priority, make voice lines unable to play
            if (!audioEvent.priority) return;

            priorityAudioPlaying = true;
            Invoke(nameof(ResetPriorityAudio), audioEvent.currentClip.length);
        }

        private void ResetPriorityAudio()
        {
            priorityAudioPlaying = false;
        }

        private AudioSource GetAvailableAudioSource()
        {
            // Find the first available audio source
            foreach (Transform child in transform)
                if (!child.GetComponent<AudioSource>().isPlaying)
                    return child.GetComponent<AudioSource>();
            // If none are available, return null
            return null;
        }

        private void HurtAudio()
        {
            if (gd.playerData.health > 1) PlayVoiceLine(hurt);
        }

        private void SpeedCombo(RoundData.SpeedBonusType type)
        {
            switch (type)
            {
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

        private void BeginRound()
        {
            if (gd.roundData.currentRound > 1) PlayVoiceLine(newRound);
        }
    }
}