using System;
using DG.Tweening;
using Managers;
using UnityEngine;
using TARGET = Data.TargetData.Target;

namespace Animation
{
    public class PunchAnimation : MonoBehaviour
    {   
        public Sprite[] punchSprites;
        public Sprite[] maskSprites;
       
        private SpriteRenderer spriteRenderer;
        private DataWrangler.GameData gd;
        
        private void Awake()
        {
            gd = DataWrangler.GetGameData();
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            gd.eventData.OnPunchNormal.AddListener(Punch);
            gd.eventData.OnPunchWarmup.AddListener(Punch);
            gd.eventData.OnPunchTimeAttack.AddListener(Punch);
        }

        private void Punch(TARGET punch)
        {
            Sequence seq = DOTween.Sequence();
            spriteRenderer.sprite = punchSprites[(int)punch + 1];
            seq.AppendInterval(gd.playerData.punchSpeed).OnComplete(() => spriteRenderer.sprite = punchSprites[0]);
        }
    }
}