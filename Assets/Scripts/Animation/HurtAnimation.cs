using System;
using Managers;
using DG.Tweening;
using UnityEngine;
using Data.Customization;
using MoreMountains.Feedbacks;

namespace Animation
{
    public class HurtAnimation : MonoBehaviour
    {
        [SerializeField] private SO_CharacterPart part;
        
        private SpriteRenderer spriteRenderer;
        private DataWrangler.GameData gd;
        private CharacterSpriteManager charAnim;
        private static readonly int Color1 = Shader.PropertyToID("_Color");

        private void Awake()
        {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnMiss.AddListener(Hurt);
            spriteRenderer = GetComponent<SpriteRenderer>();
            charAnim = GetComponent<CharacterSpriteManager>();
        }

        private void Hurt()
        {
            if (part.CurrentItem.colorMask) ColorMaskAnim();

            Color current = part.CurrentItem.colorMask ? Color.white : part.CurrentItem.color;
            DOTween.Kill(spriteRenderer);
            spriteRenderer.color = Color.red;
            Sequence seq = DOTween.Sequence(spriteRenderer);
            seq.PrependInterval(0.25f).Append(spriteRenderer.DOColor(current, 0.5f));
        }

        private void ColorMaskAnim()
        {
            Color current = charAnim.mat.GetColor(Color1);
            DOTween.Kill(charAnim.mat);
            charAnim.mat.SetColor(Color1, Color.red);
            Sequence seq = DOTween.Sequence(charAnim.mat);
            seq.PrependInterval(0.25f).Append(charAnim.mat.DOColor(current, 0.5f));
        }
    }
}