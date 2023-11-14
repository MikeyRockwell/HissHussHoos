using Managers;
using DG.Tweening;
using UnityEngine;
using TARGET = Data.TargetData.Target;

namespace Animation {
    // Switches the sprite to a punch sprite when punching
    // Applied to each part of the character
    // Also applied to the punching bag
    public class PunchAnimation : MonoBehaviour {
        
        public Sprite[] punchSprites;
        public Sprite[] maskSprites;

        private SpriteRenderer spriteRenderer;
        private DataWrangler.GameData gd;

        private void Awake() {
            // EVENTS
            gd = DataWrangler.GetGameData();
            gd.eventData.OnPunchNormal.AddListener(Punch);
            gd.eventData.OnPunchWarmup.AddListener(Punch);
            gd.eventData.OnPunchTimeAttack.AddListener(Punch);
            gd.eventData.OnPunchPrecision.AddListener(Punch);
            // COMPONENTS
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Punch(TARGET punch) {
            // Switches the sprite to a punch sprite when punching
            // Returns to the default sprite after a delay
            // Delay is based on the punch speed in the player data
            Sequence seq = DOTween.Sequence();
            spriteRenderer.sprite = punchSprites[(int)punch + 1];
            seq.AppendInterval(gd.playerData.punchSpeed).OnComplete(() => spriteRenderer.sprite = punchSprites[0]);
        }
    }
}