using System;
using Data;
using Data.Customization;
using Managers;
using UnityEngine;

namespace Animation
{
    public class PunchFX : MonoBehaviour
    {
        private DataWrangler.GameData gd;

        [SerializeField] private Animator[] animator;
        [SerializeField] private SO_CharacterPart gloves;
        [SerializeField] private SpriteRenderer[] renderers;
        [SerializeField] private ParticleSystem[] particleSystems;
        [SerializeField] private int puffsToEmit = 5;

        private static readonly int FX1 = Animator.StringToHash("FX");
        private static readonly int GlowColor = Shader.PropertyToID("GlowColor");

        private void Awake()
        {
            gd = DataWrangler.GetGameData();

            gloves.OnChangeItemColor.AddListener(ChangeSpriteColor);

            gd.eventData.OnPunchWarmup.AddListener(PlayPunchFX);
            gd.eventData.OnPunchNormal.AddListener(PlayPunchFX);
            gd.eventData.OnPunchNormal.AddListener(PlayImpactFX);
            gd.eventData.OnPunchTimeAttack.AddListener(PlayImpactFX);
            gd.eventData.OnPunchTimeAttack.AddListener(PlayPunchFX);
        }

        private void ChangeSpriteColor(SO_Item arg0, Color color)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.SetColor(GlowColor, color);
                renderers[i].color = color;
            }
        }

        private void PlayPunchFX(TargetData.Target target)
        {
            if (gd.customEvents.MenuOpen) PlayImpactFX(target);
            animator[(int)target].SetTrigger(FX1);
        }

        private void PlayImpactFX(TargetData.Target target)
        {
            particleSystems[(int)target].Emit(puffsToEmit);
        }
    }
}