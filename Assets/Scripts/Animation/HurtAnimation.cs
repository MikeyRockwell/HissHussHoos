using Managers;
using DG.Tweening;
using UnityEngine;

namespace Animation {
    public class HurtAnimation : MonoBehaviour {

        private SpriteRenderer spriteRenderer;
        private DataWrangler.GameData gd;
        
        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnMiss.AddListener(Hurt);
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Hurt() {
            spriteRenderer.DOKill();
            spriteRenderer.color = Color.red;
            Sequence seq = DOTween.Sequence();
            seq.PrependInterval(0.25f).Append(spriteRenderer.DOColor(Color.white, 0.5f));
        }
    }
}