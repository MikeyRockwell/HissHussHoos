using Managers;
using DG.Tweening;
using UnityEngine;
using Data.Customization;

namespace Animation {
    public class HurtAnimation : MonoBehaviour {

        private SpriteRenderer spriteRenderer;
        private DataWrangler.GameData gd;
        private SO_CharacterPart part;
        
        private void Awake() {
            gd = DataWrangler.GetGameData();
            gd.eventData.OnMiss.AddListener(Hurt);

            part = GetComponent<CharacterAnimation>().part;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Hurt() {
            Color current = part.CurrentItem.colorMask ?  Color.white : part.CurrentItem.color ;

            DOTween.Kill(spriteRenderer);
            spriteRenderer.color = Color.red;
            Sequence seq = DOTween.Sequence(spriteRenderer);
            seq.PrependInterval(0.25f).Append(spriteRenderer.DOColor(current, 0.5f));
        }

    }
}